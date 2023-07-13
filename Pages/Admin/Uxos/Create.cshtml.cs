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
		public Uxo Uxo { get; set; }
		public Image Image { get; set; }
		public Category Category { get; set; }
		public CreateModel(MinedDbContext db)
		{
			_db = db;
		}

		public void OnGet()
        {
        }
		public async Task<IActionResult> OnPost()
        {
			if (ModelState.IsValid) 
            {
				await _db.Uxos.AddAsync(Uxo);
				await _db.Categories.AddAsync(Category);
				await _db.Images.AddAsync(Image);
				await _db.SaveChangesAsync();
				return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
