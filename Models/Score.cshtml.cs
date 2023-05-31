using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mined.Domain
{
    public class Score
    {       
        public int Id {get; set;}
        public string? Nickname {get; set;}
        public int? NumberOfMistakes {get; set;}
        public string? UxoMistakes {get; set;}
        public int? PlayerScore {get; set;}
    }
}