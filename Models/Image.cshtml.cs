using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mined.Domain
{
    public class Image
    {     
        public int Id {get; set;}
        public string? UxoName {get; set;}
        public virtual Uxo Uxo {get; set;}
        public byte[]? UxoImage {get; set;}
    }
}