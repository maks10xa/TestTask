using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class AuthorService(ApplicationDbContext context) : IAuthorService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly int _selectedYear = 2015;

        public async Task<Author> GetAuthor()
        {
            var authorsWithLongestTitles = await _context.Authors.Select(a => new
            {
                Author = a,
                MaxTitleLength = _context.Books.Where(b => b.AuthorId == a.Id).Max(b => b.Title.Length)
            }).ToListAsync();

            var author = authorsWithLongestTitles
                            .OrderByDescending(i => i.MaxTitleLength)
                            .ThenBy(i => i.Author.Id)
                            .FirstOrDefault().Author;
            return author;
        }

        public async Task<List<Author>> GetAuthors() => await _context.Books
        .Where(b => b.PublishDate.Year > _selectedYear)
        .GroupBy(b => b.AuthorId)
        .Where(i => i.Count() % 2 == 0)
        .Select(i => _context.Authors.FirstOrDefault(a => a.Id == i.Key)) 
        .AsNoTracking()
        .ToListAsync();
    }
}
