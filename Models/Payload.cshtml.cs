using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mined.Domain
{
    public class Payload
    {     
        public int Id {get; set;}
        public string? AbbreviationRussian {get; set;}
        public string? AbbreviationNato {get; set;}
        public string? PayloadTypeEng {get; set;}
        public string? PayloadTypeNl {get; set;}
        public string? ChemicalPayload {get; set;}
    }
}