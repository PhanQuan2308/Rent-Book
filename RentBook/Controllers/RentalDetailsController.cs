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
        private readonly ILogger<RentalDetailsController> _logger;

        public RentalDetailsController(RentBookContext context, ILogger<RentalDetailsController> logger) // Sửa tên controller trong constructor
        {
            _context = context;
            _logger = logger;
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

        [HttpPost]
        public async Task<ActionResult<Rental>> PostRental(Rental rental)
        {
            _logger.LogInformation("Creating new rental for customer {customerId}.", rental.CustomerID);

            var customerExists = await _context.Customers.AnyAsync(c => c.CustomerID == rental.CustomerID);
            if (!customerExists)
            {
                _logger.LogWarning("Customer with ID {customerId} does not exist.", rental.CustomerID);
                return BadRequest(new { error = "Invalid CustomerID. Customer does not exist." });
            }

            _context.Rentals.Add(rental);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Rental with ID {rentalId} created.", rental.RentalID);
            return CreatedAtAction(nameof(GetRentalDetails), new { id = rental.RentalID }, rental);
        }







    }
}