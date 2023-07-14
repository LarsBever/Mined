using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mined.DataAccess.Data;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;


namespace Mined.Pages.Admin.Uxos
{
    [BindProperties]
    public class EditModel : PageModel
    {
		private readonly IUnitOfWork _unitOfWork;
		public EditModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public Uxo Uxo { get; set; }
		public Image Image { get; set; }
		public Category Category { get; set; }
		public IEnumerable<Category> Categories { get; set; } 
		public void OnGet(int UXO_ID)
        {
			Uxo = _unitOfWork.Uxo.GetFirstOrDefault(u=>u.Uxo_ID==Uxo.Uxo_ID);
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
				_unitOfWork.Uxo.Update(Uxo);
				_unitOfWork.Category.Update(Category);
				_unitOfWork.Image.Update(Image);
				_unitOfWork.Save();
				TempData["success"] = "U.X.O. Edited successfully";
				return RedirectToPage("Index");
            }
            return Page();
        }
    }
}


