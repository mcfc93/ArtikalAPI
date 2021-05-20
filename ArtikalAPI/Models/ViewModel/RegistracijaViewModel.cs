using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtikalAPI.Models.ViewModel
{
    public class RegistracijaViewModel
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string BrojTelefona { get; set; }
        public string Email { get; set; }
        public string Lozinka { get; set; }
        public string LozinkaPonovo { get; set; }
    }
}
