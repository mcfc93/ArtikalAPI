using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtikalAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace ArtikalAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AtributController : ControllerBase
    {
        private ArtikalContext Context { get; }

        public AtributController(ArtikalContext context)
        {
            Context = context;
        }

        // GET: api/Atribut
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Atribut>>> GetAtribut()
        {
            return await Context.Atributi.Include(a => a.Vrsta).ToListAsync();
        }

        // GET: api/Atribut/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Atribut>> GetAtribut(int id)
        {
            var atribut = await Context.Atributi.Include(a => a.Vrsta).FirstOrDefaultAsync(atribut => atribut.Id == id);

            if (atribut == null)
            {
                return NotFound();
            }

            return atribut;
        }

        // PUT: api/Atribut/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAtribut(int id, Atribut atribut)
        {
            if (id != atribut.Id)
            {
                return BadRequest();
            }

            Context.Entry(atribut).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AtributExists(id))
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

        // POST: api/Atribut
        [HttpPost]
        public async Task<ActionResult<Atribut>> PostAtribut(Atribut atribut)
        {
            Context.Atributi.Add(atribut);
            await Context.SaveChangesAsync();

            return CreatedAtAction("GetAtribut", new { id = atribut.Id }, atribut);
        }

        // DELETE: api/Atribut/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAtribut(int id)
        {
            var atribut = await Context.Atributi.FindAsync(id);
            if (atribut == null)
            {
                return NotFound();
            }

            Context.Atributi.Remove(atribut);
            await Context.SaveChangesAsync();

            return NoContent();
        }

        private bool AtributExists(int id)
        {
            return Context.Atributi.Any(e => e.Id == id);
        }
    }
}
