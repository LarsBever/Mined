using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mined.DataAccess.Data;
using Mined.Models;

namespace Mined.Pages.Admin.Uxos
{
    [BindProperties]
    public class CreateModel : PageModel
    {
        private readonly MinedDbContext _db;
        public CreateModel(MinedDbContext db)
        {
            _db = db;
        }
        public Uxo Uxo { get; set; }
		public Payload Payload { get; set; }
        public Category Category { get; set; }
        public UxoPayload UxoPayload { get; set; }

		public void OnGet()
        {
        }
		public async Task<IActionResult> OnPostAddUxo()
        {
			if (ModelState.IsValid) 
            {
				await _db.Uxos.AddAsync(Uxo);
				await _db.Categories.AddAsync(Category);
				await _db.Payloads.AddAsync(Payload);
				await _db.UxoPayloads.AddAsync(UxoPayload);
				await _db.SaveChangesAsync();
				return RedirectToPage("Index");
            }
            return Page();
        }
    }
}