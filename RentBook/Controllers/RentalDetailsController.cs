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

        public RentalDetailsController(RentBookContext context, ILogger<RentalDetailsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("getall")]
        public async Task<ActionResult<IEnumerable<RentalDetail>>> GetRentalDetails()
        {
            var rentalDetails = await _context.RentalDetails
                .Include(rd => rd.Rental)
                .ThenInclude(r => r.Customer)
                .Include(rd => rd.ComicBook)
                .ToListAsync();

            return Ok(rentalDetails);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RentalDetail>> GetRentalDetail(int id)
        {
            var rentalDetail = await _context.RentalDetails
                .Include(rd => rd.Rental)
                .ThenInclude(r => r.Customer)
                .Include(rd => rd.ComicBook)
                .FirstOrDefaultAsync(rd => rd.RentalDetailID == id);

            if (rentalDetail == null)
            {
                return NotFound();
            }

            return Ok(rentalDetail);
        }

        [HttpPost("create")]
        public async Task<ActionResult<RentalDetail>> PostRentalDetail(RentalDetail rentalDetail)
        {
            var rentalExists = await _context.Rentals.AnyAsync(r => r.RentalID == rentalDetail.RentalID);
            if (!rentalExists)
            {
                return BadRequest(new { error = "Invalid RentalID. Rental does not exist." });
            }

            var comicBookExists = await _context.ComicBooks.AnyAsync(c => c.ComicBookID == rentalDetail.ComicBookID);
            if (!comicBookExists)
            {
                return BadRequest(new { error = "Invalid ComicBookID. ComicBook does not exist." });
            }

            _context.RentalDetails.Add(rentalDetail);
            await _context.SaveChangesAsync();

            var rentalDetailWithInfo = await _context.RentalDetails
                .Where(rd => rd.RentalDetailID == rentalDetail.RentalDetailID)
                .Include(rd => rd.Rental)  
                    .ThenInclude(r => r.Customer)  
                .Include(rd => rd.ComicBook) 
                .FirstOrDefaultAsync();

            if (rentalDetailWithInfo == null)
            {
                return NotFound(new { error = "RentalDetail not found." });
            }

            return CreatedAtAction(nameof(GetRentalDetail), new { id = rentalDetailWithInfo.RentalDetailID }, rentalDetailWithInfo);
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









    }
}