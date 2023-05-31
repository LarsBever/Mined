using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mined.Domain
{
    public class User
    {       
        public int Id {get; set;}
        public string? UserName {get; set;}
        public int PasswordId {get; set;}
        public virtual Password Password {get; set;}
    }
}