using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;

    public CommentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Comment>> GetAllAsync()
    {
        return await _context.Comments.ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
        return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Comment> CreateAsync(Comment comment)
    {
        await _context.Comments.AddAsync(comment);
        await _context.SaveChangesAsync();

        return comment;
    }

    public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
    {
        var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);

        if (comment == null)
        {
            return null;
        }
        
        comment.Title = commentModel.Title;
        comment.Content = commentModel.Content;
        
        await _context.SaveChangesAsync();

        return comment;
    }

    public async Task<Comment?> DeleteAsync(int id)
    {
        var comment = _context.Comments.FirstOrDefault(c => c.Id == id);

        if (comment == null)
        {
            return null;
        }
        
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
        return comment;
    }
}