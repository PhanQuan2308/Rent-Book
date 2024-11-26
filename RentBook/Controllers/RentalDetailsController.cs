using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentBook.Model;

namespace RentBook.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RentalDetailsController : ControllerBase
    {
        private readonly RentBookContext _context;

        public RentalDetailsController(RentBookContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentalDetail>>> GetRentalDetails()
        {
            return await _context.RentalDetails
                                 .Include(rd => rd.RentalID)
                                 .Include(rd => rd.ComicBookID)
                                 .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RentalDetail>> GetRentalDetail(int id)
        {
            var rentalDetail = await _context.RentalDetails
                                             .Include(rd => rd.RentalID)
                                             .Include(rd => rd.ComicBookID)
                                             .FirstOrDefaultAsync(rd => rd.RentalDetailID == id);

            if (rentalDetail == null)
            {
                return NotFound();
            }

            return rentalDetail;
        }

        [HttpPost]
        public async Task<ActionResult<RentalDetail>> PostRentalDetail(RentalDetail rentalDetail)
        {
            _context.RentalDetails.Add(rentalDetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRentalDetail), new { id = rentalDetail.RentalDetailID }, rentalDetail);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRentalDetail(int id, RentalDetail rentalDetail)
        {
            if (id != rentalDetail.RentalDetailID)
            {
                return BadRequest();
            }

            _context.Entry(rentalDetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentalDetailExists(id))
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
        public async Task<IActionResult> DeleteRentalDetail(int id)
        {
            var rentalDetail = await _context.RentalDetails.FindAsync(id);
            if (rentalDetail == null)
            {
                return NotFound();
            }

            _context.RentalDetails.Remove(rentalDetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RentalDetailExists(int id)
        {
            return _context.RentalDetails.Any(e => e.RentalDetailID == id);
        }

        [HttpGet("report")]
        public async Task<ActionResult<IEnumerable<object>>> GetRentalReport(DateTime startDate, DateTime endDate)
        {
            var report = await _context.Rentals
                .Where(r => r.RentalDate >= startDate && r.RentalDate <= endDate)  
                .SelectMany(r => r.RentalDetails, (rental, rentalDetail) => new  
                {
                    BookName = _context.ComicBooks
                        .Where(cb => cb.ComicBookID == rentalDetail.ComicBookID)
                        .Select(cb => cb.Title)
                        .FirstOrDefault(),  
                    RentalDate = rental.RentalDate.ToString("dd/MM/yyyy"),  
                    ReturnDate = rental.ReturnDate.HasValue
                        ? rental.ReturnDate.Value.ToString("dd/MM/yyyy")
                        : null, 
                    CustomerName = _context.Customers
                        .Where(c => c.CustomerID == rental.CustomerID)  
                        .Select(c => c.FullName)
                        .FirstOrDefault(), 
                    Quantity = rentalDetail.Quantity  
                })
                .ToListAsync();

            return Ok(report);
        }




    }
}