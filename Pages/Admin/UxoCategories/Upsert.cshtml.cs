using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Mined.DataAccess.Data;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;
using System.ComponentModel;
using System.Linq;

namespace Mined.Pages.Admin.UxoCategories
{
	[BindProperties]
	public class UpsertModel : PageModel
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _hostEnvironment;
		public UpsertModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_hostEnvironment = hostEnvironment;
			Category = new();
		}
		public int Id { get; set; }
		public Category? Category { get; set; }
		public IEnumerable<SelectListItem>? CategoryList { get; set; }
		public IEnumerable<Category> Categories { get; set; }

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
				//Add
				
				//Check if Category does not already exist:
				List<string> categoryNames = new List<string>();
				Categories = _unitOfWork.Category.GetAll();
				foreach (Category category in Categories)
				{
					categoryNames.Add(category.CategoryName);
				}
				if (categoryNames.Contains(Category.CategoryName))
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
				//Update
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


