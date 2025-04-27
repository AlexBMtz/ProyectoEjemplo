using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedModels.Models;
using TestMAUIAppApi.Data;

namespace TestMAUIAppApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EditorialController : ControllerBase
    {
        private readonly DataContext _context;

        public EditorialController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Editorial
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Editorial>>> GetEditorials()
        {
            return await _context.Editorials.ToListAsync();
        }

        // GET: api/Editorial/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Editorial>> GetEditorial(int id)
        {
            var editorial = await _context.Editorials.FindAsync(id);

            if (editorial == null)
            {
                return NotFound();
            }

            return editorial;
        }

        // PUT: api/Editorial/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEditorial(int id, Editorial editorial)
        {
            if (id != editorial.Id)
            {
                return BadRequest();
            }

            _context.Entry(editorial).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EditorialExists(id))
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

        // POST: api/Editorial
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Editorial>> PostEditorial(Editorial editorial)
        {
            _context.Editorials.Add(editorial);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEditorial", new { id = editorial.Id }, editorial);
        }

        // DELETE: api/Editorial/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEditorial(int id)
        {
            var editorial = await _context.Editorials.FindAsync(id);
            if (editorial == null)
            {
                return NotFound();
            }

            _context.Editorials.Remove(editorial);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EditorialExists(int id)
        {
            return _context.Editorials.Any(e => e.Id == id);
        }
    }
}
