using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtikalAPI.Models
{
    public class Artikal
    {
        public int Id { get; set; }
        public string Sifra { get; set; }
        public string Naziv { get; set; }
        public string JedinicaMjere { get; set; }
        public List<Atribut> Atributi { get; set; }
    }
}
