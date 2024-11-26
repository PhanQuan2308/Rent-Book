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



        [HttpGet("{id}")]
        public async Task<ActionResult<Rental>> GetRental(int id)
        {
            var rental = await _context.Rentals
                .Where(r => r.RentalID == id)
                .Include(r => r.Customer)
                .Include(r => r.RentalDetails)
                .FirstOrDefaultAsync();

            if (rental == null)
            {
                return NotFound(new { message = "Rental not found." });
            }

            return Ok(rental);
        }




        [HttpPost("create")]
        public async Task<ActionResult<Rental>> PostRental(Rental rental)
        {
            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerID == rental.CustomerID);
            if (!customerExists)
            {
                return BadRequest(new { error = "Invalid CustomerID. Customer does not exist." });
            }

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            foreach (var rentalDetail in rental.RentalDetails)
            {
                rentalDetail.RentalID = rental.RentalID;
                _context.RentalDetails.Add(rentalDetail);
            }

            await _context.SaveChangesAsync();

            var rentalWithCustomer = await _context.Rentals
                .Where(r => r.RentalID == rental.RentalID)
                .Include(r => r.Customer)
                .FirstOrDefaultAsync();

            return CreatedAtAction(nameof(GetRental), new { id = rentalWithCustomer.RentalID }, rentalWithCustomer);
        }





        [HttpPut("{id}")]
        public async Task<IActionResult> PutRental(int id, Rental rental)
        {
            if (id != rental.RentalID)
            {
                return BadRequest(new { error = "RentalID mismatch." });
            }

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

        [HttpGet("report")]
        public async Task<ActionResult<IEnumerable<RentalDetail>>> GetRentalReport(DateTime startDate, DateTime endDate)
        {
            var rentals = await _context.Rentals
                .Where(r => r.RentalDate >= startDate && r.RentalDate <= endDate)
                .Include(r => r.RentalDetails)
                    .ThenInclude(rd => rd.ComicBook)
                .Include(r => r.Customer)
                .ToListAsync();

            if (!rentals.Any())
            {
                return NotFound(new { message = "No rentals found for the selected dates." });
            }

            var reportData = rentals.SelectMany(r => r.RentalDetails, (r, rd) => new
            {
                bookName = rd.ComicBook.Title,
                rentalDate = r.RentalDate,
                returnDate = r.ReturnDate,
                customerName = r.Customer.FullName,
                quantity = rd.Quantity
            }).ToList();

            return Ok(reportData);
        }

    }
}