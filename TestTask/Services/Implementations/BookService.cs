using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations
{
    public class BookService(ApplicationDbContext context) : IBookService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly string _requiredTitleWord = "Red";
        private readonly DateTime _carolusRexReleaseDate = new DateTime(2012, 5, 22, 12, 0, 0);

        public async Task<Book> GetBook() => await _context.Books
            .OrderByDescending(i => i.Price * i.QuantityPublished)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        public async Task<List<Book>> GetBooks() => await _context.Books
            .Where(b => b.Title.Contains(_requiredTitleWord) && b.PublishDate > _carolusRexReleaseDate)
            .AsNoTracking()
            .ToListAsync();
    }
}
