using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mined.Domain
{
    public class Category
    {     
        public string CategoryName {get; set;}  
        public string? MainCategoryNatoEng {get; set;}
        public string? SubCategoryNatoEng {get; set;}
        public string? MainCategoryUsEng {get; set;}
        public string? SubCategoryUsEng {get; set;}
        public string? MainCategoryNatoNl {get; set;}
        public string? SubCategoryNatoNl {get; set;}
        public string? MainCategoryUsNl {get; set;}
        public string? SubCategoryUsNl {get; set;}
    }
}