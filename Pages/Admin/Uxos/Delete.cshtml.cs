using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Mined.DataAccess.Data;
using Mined.Models;


namespace Mined.Pages.Admin.Uxos
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
        private readonly MinedDbContext _db;
		public Uxo Uxo { get; set; }
		public Image Image { get; set; }
		public Category Category { get; set; }
		public IEnumerable<Category> Categories { get; set; } 
		public DeleteModel(MinedDbContext db)
		{
			_db = db;
		}

		public void OnGet(int UXO_ID)
        {
			Uxo = _db.Uxos.Find(UXO_ID);
        }
		public async Task<IActionResult> OnPost()
        {
				var uxoFromDb = _db.Uxos.Find(Uxo.Uxo_ID);
				var imageFromDb = _db.Images.Find(Image.Image_ID);
				if (uxoFromDb != null && imageFromDb != null) 
				{
					_db.Uxos.Remove(Uxo);
					_db.Images.Remove(Image);
					await _db.SaveChangesAsync();
					TempData["success"] = "U.X.O. deleted successfully";
					return RedirectToPage("Index");
				}
            return Page();
        }
    }
}


