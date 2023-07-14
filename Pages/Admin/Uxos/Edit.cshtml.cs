using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mined.DataAccess.Data;
using Mined.Models;


namespace Mined.Pages.Admin.Uxos
{
    [BindProperties]
    public class EditModel : PageModel
    {
        private readonly MinedDbContext _db;
		public Uxo Uxo { get; set; }
		public Image Image { get; set; }
		public Category Category { get; set; }
		public IEnumerable<Category> Categories { get; set; } 
		public EditModel(MinedDbContext db)
		{
			_db = db;
		}

		public void OnGet(int UXO_ID)
        {
			Uxo = _db.Uxos.Find(UXO_ID);
        }
		public async Task<IActionResult> OnPost()
        {
			if(Category.MainCategoryNato == Category.SubCategoryNato)
			{
				ModelState.AddModelError(string.Empty, "The Main Category and Subcategory cannot be the same");
			}
			if (Uxo.NameNato == Category.MainCategoryNato)
			{
				ModelState.AddModelError(string.Empty, "The Main Category and U.X.O. Name cannot be the same");
			}
			if (Uxo.NameNato == Category.SubCategoryNato)
			{
				ModelState.AddModelError(string.Empty, "The Subcategory and U.X.O. Name cannot be the same");
			}
			if (ModelState.IsValid) 
            {
				_db.Uxos.Update(Uxo);
				_db.Categories.Update(Category);
				_db.Images.Update(Image);
				await _db.SaveChangesAsync();
				TempData["success"] = "U.X.O. Edited successfully";
				return RedirectToPage("Index");
            }
            return Page();
        }
    }
}


