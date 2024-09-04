using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using api.Repositories;

namespace api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly IStockRepository _repo;

    public StockController(IStockRepository stockRepository)
    {
        _repo = stockRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var stocks = await _repo.GetAllAsync(query);
        var stockDto = stocks.Select(s => s.ToStockDto());
        return Ok(stockDto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var stock = await _repo.GetByIdAsync(id);
        if (stock == null) return NotFound();
        
        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockDto createDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var stockModel = createDto.ToStockFromCreateDto();
        await _repo.CreateAsync(stockModel);
        
        return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockDto updateDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var stockModel = await _repo.UpdateAsync(id, updateDto);
        if (stockModel == null) return NotFound();
        
        return Ok(stockModel.ToStockDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var stockModel = await _repo.DeleteAsync(id);
        if (stockModel == null) return NotFound();
        
        return NoContent();
    }
}