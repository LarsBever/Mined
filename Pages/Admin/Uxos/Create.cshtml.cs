using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mined.Models;

namespace Mined.Pages.Admin.Uxos
{
    public class CreateModel : PageModel
    {
        public Uxo Uxo { get; set; }
		public Payload Payload { get; set; }
        public Category Category { get; set; }
		public void OnGet()
        {
        }
    }
}
