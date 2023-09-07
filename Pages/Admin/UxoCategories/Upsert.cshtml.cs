using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;

namespace Mined.Pages.Admin.UxoCategories
{
	[BindProperties]
	public class UpsertModel : PageModel
	{
        public int Id { get; set; }
        public Category? Category { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _hostEnvironment;
		public UpsertModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_hostEnvironment = hostEnvironment;
			Category = new();
		}
		public void OnGet(int id)
		{
			if (id != 0)
			{
				//Update item
				Category = _unitOfWork.Category.GetFirstOrDefault(x => x.Category_ID == id);
			}
		}
		public async Task<IActionResult> OnPost()
		{
			if (Category.Category_ID == 0)
			{
                //Add new Category

				//Check if Category does not already exist:
				Categories = _unitOfWork.Category.GetAll();
				if (Categories.Any(c => c.CategoryName == Category.CategoryName))
				{
					ModelState.AddModelError(string.Empty, "This Category already Exists");
				}
				if (ModelState.IsValid)
				{
					_unitOfWork.Category.Add(Category);
					_unitOfWork.Save();
					TempData["success"] = "Category added successfully";
					return RedirectToPage("Index");
				}
			}
			else
			{
				//Update existing Category

				if (ModelState.IsValid)
				{
					_unitOfWork.Category.Update(Category);
					_unitOfWork.Save();
				}
				TempData["success"] = "Category saved successfully";
				return RedirectToPage("Index");
			}
            TempData["error"] = "Something went wrong...";
            return Page();
		}
	}
}


