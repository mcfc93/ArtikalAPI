using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtikalAPI.Models;
using Microsoft.AspNetCore.Authorization;
using ArtikalAPI.Models.ViewModel;

namespace ArtikalAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ArtikalController : ControllerBase
    {
        private ArtikalContext Context { get; }

        public ArtikalController(ArtikalContext context)
        {
            Context = context;
        }

        // GET: api/Artikal
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Artikal>>> GetArtikal([FromQuery]PretragaViewModel pretragaViewModel)
        {
            var artikli = await Context.Artikli.Include(artikal => artikal.Atributi).ThenInclude(atribut => atribut.Vrsta).ToListAsync();

            //?sifra=...&naziv=...&jm=...&atribut[...]=...&atribut[...]=...

            if (pretragaViewModel.sifra != null)
            {
                artikli = artikli.Where(a => a.Sifra.ToLower().Contains(pretragaViewModel.sifra.ToLower())).ToList();
            }
            if (pretragaViewModel.naziv != null)
            {
               artikli = artikli.Where(a => a.Naziv.ToLower().Contains(pretragaViewModel.naziv.ToLower())).ToList();
            }
            if (pretragaViewModel.jm != null)
            {
                artikli = artikli.Where(a => a.JedinicaMjere.ToLower().Contains(pretragaViewModel.jm.ToLower())).ToList();
            }
            if (pretragaViewModel.atribut != null)
            {
                foreach (string key in pretragaViewModel.atribut.Keys)
                {
                    Console.WriteLine(key);
                    Console.WriteLine(pretragaViewModel.atribut[key]);
                    artikli = artikli.Where(artikal =>
                        artikal.Atributi.Where(atr =>
                            atr.Vrsta.Naziv.Equals(key)
                            && atr.Vrijednost.ToLower().Contains(pretragaViewModel.atribut[key].ToLower())
                        ).Any()
                    ).ToList();
                }
            }
            return artikli;
        }

        // GET: api/Artikal/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Artikal>> GetArtikal(int id)
        {
            var artikal = await Context.Artikli.Include(a => a.Atributi).ThenInclude(atribut => atribut.Vrsta).FirstOrDefaultAsync(artikal => artikal.Id == id);

            if (artikal == null)
            {
                return NotFound();
            }

            return artikal;
        }

        // PUT: api/Artikal/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArtikal(int id, Artikal artikal)
        {
            if (id != artikal.Id)
            {
                return BadRequest();
            }

            //Context.Entry(artikal).State = EntityState.Modified;
            Context.Update(artikal);

            try
            {
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtikalExists(id))
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

        // POST: api/Artikal
        [HttpPost]
        public async Task<ActionResult<Artikal>> PostArtikal(Artikal artikal)
        {
            Context.Artikli.Add(artikal);
            await Context.SaveChangesAsync();

            return CreatedAtAction("GetArtikal", new { id = artikal.Id }, artikal);
        }

        // DELETE: api/Artikal/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtikal(int id)
        {
            var artikal = await Context.Artikli.FindAsync(id);
            if (artikal == null)
            {
                return NotFound();
            }

            Context.Artikli.Remove(artikal);
            await Context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArtikalExists(int id)
        {
            return Context.Artikli.Any(e => e.Id == id);
        }
    }
}
