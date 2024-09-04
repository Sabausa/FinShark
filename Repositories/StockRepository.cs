using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDbContext _context;

    public StockRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Stock>> GetAllAsync(QueryObject query)
    {
        var stocks = _context.Stocks.Include(s => s.Comments).AsQueryable();
        
        if(!string.IsNullOrWhiteSpace(query.CompanyName)) stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
        if(!string.IsNullOrWhiteSpace(query.Symbol)) stocks = stocks.Where(s => s.CompanyName.Contains(query.Symbol));
        
        return await stocks.ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await _context.Stocks.Include(s => s.Comments).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Stock> CreateAsync(Stock stock)
    {
        await _context.Stocks.AddAsync(stock);
        await _context.SaveChangesAsync();
        return stock;
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockDto updateDto)
    {
        var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);
        if (stockModel == null)
        {
            return null;
        }

        stockModel.Symbol = updateDto.Symbol;
        stockModel.CompanyName = updateDto.CompanyName;
        stockModel.Purchase = updateDto.Purchase;
        stockModel.LastDiv = updateDto.LastDiv;
        stockModel.Industry = updateDto.Industry;
        stockModel.MarketCap = updateDto.MarketCap;

        await _context.SaveChangesAsync();

        return stockModel;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
        var stockModel = await _context.Stocks.Include(stock => stock.Comments).FirstOrDefaultAsync(x => x.Id == id);

        if (stockModel == null)
        {
            return null;
        }

        foreach (var stockModelComment in stockModel.Comments)
        {
            _context.Comments.Remove(stockModelComment);
        }

        _context.Stocks.Remove(stockModel);
        await _context.SaveChangesAsync();

        return stockModel;
    }

    public async Task<bool> StockExists(int id)
    {
        return await _context.Stocks.AnyAsync(s => s.Id == id);
    }
}