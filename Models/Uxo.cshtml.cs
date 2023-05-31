using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mined.Domain
{
    public class Uxo
    {   
        public int Id {get; set;}         
        public string? UxoName {get; set;}
        public string? CategoryName {get; set;}
        public virtual Category Category {get; set;}
        public string? NameNato {get; set;}
        public string? NameRussian {get; set;}
        public string? NameOrigin {get; set;}
        public string? NickName {get; set;}
        public string? Dod_Code {get; set;}
        public string? Nato_Code {get; set;}
        public string? description_Nl {get; set;}
        public string? description_Eng {get; set;}
    }
}