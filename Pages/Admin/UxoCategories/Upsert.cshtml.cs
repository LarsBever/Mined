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

				//Validate whether the given U.X.O. Name is not the same as the category names
				//if (Uxo.NameNato == Category.MainCategoryNato)
				//{
				//	ModelState.AddModelError(string.Empty, "The Main Category and U.X.O. Name cannot be the same");
				//}
				//if (Uxo.NameNato == Category.SubCategoryNato)
				//{
				//	ModelState.AddModelError(string.Empty, "The Subcategory and U.X.O. Name cannot be the same");
				//}
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

				//Validate whether the given U.X.O. Name is not the same as the category names
				//if (Uxo.NameNato == Category.MainCategoryNato)
				//{
				//	ModelState.AddModelError(string.Empty, "The Main Category and U.X.O. Name cannot be the same");
				//}
				//if (Uxo.NameNato == Category.SubCategoryNato)
				//{
				//	ModelState.AddModelError(string.Empty, "The Subcategory and U.X.O. Name cannot be the same");
				//}
				if (ModelState.IsValid)
				{
					_unitOfWork.Category.Update(Category);
					_unitOfWork.Save();
				}
				TempData["success"] = "Category saved successfully";
				return RedirectToPage("Index");
			}
			return Page();
		}
	}
}


