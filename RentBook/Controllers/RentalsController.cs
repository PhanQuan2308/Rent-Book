using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RentBook.Model;

namespace RentBook.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly RentBookContext _context;

        public RentalsController(RentBookContext context)
        {
            _context = context;
        }

        // GET: api/v1/Rentals
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rental>>> GetRentals()
        {
            // Lấy danh sách tất cả Rentals, kèm thông tin Customer
            return await _context.Rentals
                                 .Include(r => r.CustomerID)
                                 .ToListAsync();
        }

        // GET: api/v1/Rentals/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Rental>> GetRental(int id)
        {
            // Tìm Rental dựa theo RentalID
            var rental = await _context.Rentals
                                       .Include(r => r.CustomerID)
                                       .FirstOrDefaultAsync(r => r.RentalID == id);

            if (rental == null)
            {
                return NotFound(new { message = "Rental not found." });
            }

            return rental;
        }

        // POST: api/v1/Rentals
        [HttpPost]
        public async Task<ActionResult<Rental>> PostRental(Rental rental)
        {
            // Kiểm tra nếu CustomerID hợp lệ
            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerID == rental.CustomerID);
            if (!customerExists)
            {
                return BadRequest(new { error = "Invalid CustomerID. Customer does not exist." });
            }

            // Tạo mới Rental
            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRental), new { id = rental.RentalID }, rental);
        }

        // PUT: api/v1/Rentals/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRental(int id, Rental rental)
        {
            if (id != rental.RentalID)
            {
                return BadRequest(new { error = "RentalID mismatch." });
            }

            // Đánh dấu thực thể là đã được sửa đổi
            _context.Entry(rental).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentalExists(id))
                {
                    return NotFound(new { message = "Rental not found." });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/v1/Rentals/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRental(int id)
        {
            var rental = await _context.Rentals.FindAsync(id);
            if (rental == null)
            {
                return NotFound(new { message = "Rental not found." });
            }

            _context.Rentals.Remove(rental);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RentalExists(int id)
        {
            return _context.Rentals.Any(e => e.RentalID == id);
        }
    }
}
