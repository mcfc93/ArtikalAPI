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
    public class VrstaController : ControllerBase
    {
        private ArtikalContext Context { get; }

        public VrstaController(ArtikalContext context)
        {
            Context = context;
        }

        // GET: api/Vrsta
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vrsta>>> GetVrsta()
        {
            return await Context.Vrste.ToListAsync();
        }

        // GET: api/Vrsta/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vrsta>> GetVrsta(int id)
        {
            var vrsta = await Context.Vrste.FindAsync(id);

            if (vrsta == null)
            {
                return NotFound();
            }

            return vrsta;
        }

        // PUT: api/Vrsta/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVrsta(int id, Vrsta vrsta)
        {
            if (id != vrsta.Id)
            {
                return BadRequest();
            }

            Context.Entry(vrsta).State = EntityState.Modified;

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VrstaExists(id))
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

        // POST: api/Vrsta
        [HttpPost]
        public async Task<ActionResult<Vrsta>> PostVrsta(Vrsta vrsta)
        {
            Context.Vrste.Add(vrsta);
            await Context.SaveChangesAsync();

            return CreatedAtAction("GetVrsta", new { id = vrsta.Id }, vrsta);
        }

        // DELETE: api/Vrsta/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVrsta(int id)
        {
            var vrsta = await Context.Vrste.FindAsync(id);
            if (vrsta == null)
            {
                return NotFound();
            }

            Context.Vrste.Remove(vrsta);
            await Context.SaveChangesAsync();

            return NoContent();
        }

        private bool VrstaExists(int id)
        {
            return Context.Vrste.Any(e => e.Id == id);
        }
    }
}
