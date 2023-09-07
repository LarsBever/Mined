using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;

namespace Mined.Pages.Admin.UxoCategories
{
	[BindProperties]
	public class DeleteModel : PageModel
	{
        public Category? Category { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        public DeleteModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
		}

		public void OnGet(int id)
		{
			//Retrieve all available categories from the database
			Category = _unitOfWork.Category.GetFirstOrDefault(x => x.Category_ID == id);
		}
		public async Task<IActionResult> OnPost()
		{
			//Delete catagory
			Category = _unitOfWork.Category.GetFirstOrDefault(x => x.Category_ID == Category.Category_ID);
			if (Category != null)
			{
				_unitOfWork.Category.Remove(Category);
				_unitOfWork.Save();

				TempData["success"] = "Category deleted successfully";
				return RedirectToPage("Index");
			}
            TempData["error"] = "Something went wrong...";
            return Page();
		}
	}
}


