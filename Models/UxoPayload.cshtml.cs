using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mined.Domain
{
    public class UxoPayload
    {     
        public int Id {get; set;}        
        public int? UxoName {get; set;}
        public virtual Uxo Uxo {get; set;}
        public int? PayloadId {get; set;}
        public virtual Payload Payload {get; set;}
    }
}