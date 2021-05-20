using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ArtikalAPI.Models
{
    public class Atribut
    {
        public int Id { get; set; }
        public string Vrijednost { get; set; }
        public int VrstaId { get; set; }
        public Vrsta Vrsta { get; set; }
        public int ArtikalId { get; set; }
        [JsonIgnore]
        public Artikal Artikal { get; set; }
    }
}
