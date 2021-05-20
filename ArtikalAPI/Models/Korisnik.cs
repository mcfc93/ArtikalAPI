using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtikalAPI.Models
{
    public class Korisnik
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string BrojTelefona { get; set; }
        public string Email { get; set; }
        public string Hash { get; set; }
        public string Salt { get; set; }
    }
}
