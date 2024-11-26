using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentBook.Model;

namespace RentBook.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ComicBooksController : ControllerBase
    {
        private readonly RentBookContext _context;

        public ComicBooksController(RentBookContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComicBook>>> GetComicBooks()
        {
            return await _context.ComicBooks.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<ComicBook>> CreateComicBook(ComicBook comicBook)
        {
            _context.ComicBooks.Add(comicBook);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComicBooks), new { id = comicBook.ComicBookID }, comicBook);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutComicBook(int id, ComicBook comicBook)
        {
            if (id != comicBook.ComicBookID)
            {
                return BadRequest();
            }

            _context.Entry(comicBook).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ComicBookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComicBook(int id)
        {
            var comicBook = await _context.ComicBooks.FindAsync(id);
            if (comicBook == null)
            {
                return NotFound();
            }

            _context.ComicBooks.Remove(comicBook);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ComicBookExists(int id)
        {
            return _context.ComicBooks.Any(e => e.ComicBookID == id);
        }
    }
}