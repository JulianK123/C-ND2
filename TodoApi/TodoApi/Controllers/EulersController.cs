using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EulersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EulersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Eulers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Euler>>> GetEulers()
        {
            try
            {
                return await _context.Eulers.ToListAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        // GET: api/Eulers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Euler>> GetEuler(int id)
        {
            var euler = await _context.Eulers.FindAsync(id);

            if (euler == null)
            {
                return NotFound();
            }

            return euler;
        }
        [HttpGet("search/{search}")]
        public async Task<ActionResult<IEnumerable<Euler>>> SearchEulers(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return BadRequest("Query cannot be empty.");
            }
            try
            {
                var products = await _context.Eulers
                .Where(p => p.description.Contains(search))
                    .ToListAsync();

                if (!products.Any())
                {
                    return new List<Euler>();
                }


                return Ok(products);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        // PUT: api/Eulers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEuler(int id, Euler euler)
        {
            if (id != euler.Id)
            {
                return BadRequest();
            }

            _context.Entry(euler).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EulerExists(id))
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

        // POST: api/Eulers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("add")]
        public async Task<ActionResult<Euler>> PostEuler(Euler euler)
        {
            try
            {
                var eu = await _context.Eulers.FindAsync(euler.number);
                if (eu != null)
                {
                    return null;
                }
                _context.Eulers.Add(euler);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetEuler", new { id = euler.Id }, euler);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        // DELETE: api/Eulers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEuler(int id)
        {
            var euler = await _context.Eulers.FindAsync(id);
            if (euler == null)
            {
                return NotFound();
            }

            _context.Eulers.Remove(euler);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EulerExists(int id)
        {
            return _context.Eulers.Any(e => e.Id == id);
        }
    }
}
