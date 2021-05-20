using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtikalAPI.Models.ViewModel
{
    public class PretragaViewModel
    {
        public string sifra { get; set; }
        public string naziv { get; set; }
        public string jm { get; set; }
        public Dictionary<string, string> atribut { get; set; }
    }
}
