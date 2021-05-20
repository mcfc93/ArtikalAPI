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
using System.Security.Cryptography;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text.RegularExpressions;

namespace ArtikalAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        private ArtikalContext Context { get; }
        private IConfiguration Configuration { get; }

        public KorisnikController(ArtikalContext context, IConfiguration configuration)
        {
            Context = context;
            Configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("prijava")]
        public async Task<ActionResult<string>> Prijava(PrijavaViewModel prijavaViewModel)
        {
            Korisnik korisnik;
            if ((korisnik = await Context.Korisnici.Where(korisnik => korisnik.Email == prijavaViewModel.Email).SingleOrDefaultAsync()) != null)
            {
                //Console.WriteLine(korisnik.Hash);
                //Console.WriteLine(Hash(prijavaViewModel.Lozinka + korisnik.Salt));
                if (korisnik.Hash == Hash(prijavaViewModel.Lozinka + korisnik.Salt))
                {
                    List<Claim> claims = new List<Claim> {
                        new Claim(ClaimTypes.NameIdentifier, korisnik.Id.ToString()),
                        new Claim(ClaimTypes.Name, korisnik.Email)
                    };

                    SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(claims),
                        Expires = DateTime.Now.AddDays(1),
                        SigningCredentials = new SigningCredentials(
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"])),
                            SecurityAlgorithms.HmacSha256Signature
                        ),
                        Issuer = Configuration["JWT:Issuer"],
                        Audience = Configuration["JWT:Audience"]
                    };
                    JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                    SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

                    return Ok(new { token = tokenHandler.WriteToken(token) });
                }
            }
            return NotFound(new { message = "Korisničko ime ili lozinka pogrešni." });
        }

        [AllowAnonymous]
        [HttpPost("registracija")]
        public async Task<ActionResult<Korisnik>> Registracija(RegistracijaViewModel registracijaViewModel)
        {
            if(registracijaViewModel.Lozinka != registracijaViewModel.LozinkaPonovo)
            {
                return BadRequest(new { message = "Lozinke se ne poklapaju." });
            }
            if(!Regex.IsMatch(registracijaViewModel.Email, "^([a-z0-9]+-?[a-z0-9]+@[a-z0-9]{2,}\\.[a-z]{2,})?$"))
            {
                return BadRequest(new { message = "Email adresa ima nekorektan format." });
            }
            if (Context.Korisnici.Any(k => k.Email.Equals(registracijaViewModel.Email)))
            {
                return BadRequest(new { message = "Email adresa zauzeta." });
            }
            string salt = GenerisiSalt();
            string hash = Hash(registracijaViewModel.Lozinka + salt);
            Korisnik korisnik = new Korisnik() {
                Id = 0,
                Ime = registracijaViewModel.Ime,
                Prezime = registracijaViewModel.Prezime,
                BrojTelefona = registracijaViewModel.BrojTelefona,
                Email = registracijaViewModel.Email,
                Hash = hash,
                Salt = salt
            };
            Context.Korisnici.Add(korisnik);
            await Context.SaveChangesAsync();
            return korisnik;
        }

        private static string GenerisiSalt()
        {
            using RandomNumberGenerator random = new RNGCryptoServiceProvider();
            byte[] bytes = new byte[8];
            random.GetNonZeroBytes(bytes);

            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }

        private static string Hash(string lozinka)
        {
            using SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(lozinka));

            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }

        // GET: api/Korisnik
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Korisnik>>> GetKorisnici()
        {
            return await Context.Korisnici.ToListAsync();
        }

        // GET: api/Korisnik/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Korisnik>> GetKorisnik(int id)
        {
            var korisnik = await Context.Korisnici.FindAsync(id);

            if (korisnik == null)
            {
                return NotFound();
            }

            return korisnik;
        }

        // DELETE: api/Korisnik/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKorisnik(int id)
        {
            var korisnik = await Context.Korisnici.FindAsync(id);
            if (korisnik == null)
            {
                return NotFound();
            }

            Context.Korisnici.Remove(korisnik);
            await Context.SaveChangesAsync();

            return NoContent();
        }
    }
}
