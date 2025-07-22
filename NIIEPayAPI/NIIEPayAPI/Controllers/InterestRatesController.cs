using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NIIEPayAPI.Data;

namespace NIIEPayAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterestRatesController : ControllerBase
    {
        private readonly NiiepayContext _context;

        public InterestRatesController(NiiepayContext context)
        {
            _context = context;
        }

        // GET: api/InterestRates
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InterestRate>>> GetInterestRates()
        {
            return await _context.InterestRates.ToListAsync();
        }

        // GET: api/InterestRates/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InterestRate>> GetInterestRate(int id)
        {
            var interestRate = await _context.InterestRates.FindAsync(id);

            if (interestRate == null)
            {
                return NotFound();
            }

            return interestRate;
        }

        // PUT: api/InterestRates/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInterestRate(int id, InterestRate interestRate)
        {
            if (id != interestRate.TermMonths)
            {
                return BadRequest();
            }

            _context.Entry(interestRate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterestRateExists(id))
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

        // POST: api/InterestRates
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InterestRate>> PostInterestRate(InterestRate interestRate)
        {
            _context.InterestRates.Add(interestRate);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (InterestRateExists(interestRate.TermMonths))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetInterestRate", new { id = interestRate.TermMonths }, interestRate);
        }

        // DELETE: api/InterestRates/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInterestRate(int id)
        {
            var interestRate = await _context.InterestRates.FindAsync(id);
            if (interestRate == null)
            {
                return NotFound();
            }

            _context.InterestRates.Remove(interestRate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InterestRateExists(int id)
        {
            return _context.InterestRates.Any(e => e.TermMonths == id);
        }
    }
}
