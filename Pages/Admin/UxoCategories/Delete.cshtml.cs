using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mined.DataAccess.Data;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;
using System.ComponentModel;
using System.Linq;

namespace Mined.Pages.Admin.UxoCategories
{
	[BindProperties]
	public class DeleteModel : PageModel
	{
		private readonly IUnitOfWork _unitOfWork;
		public DeleteModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
		}
		public Category? Category { get; set; }
		public IEnumerable<Category>? Categories { get; set; }
		public IEnumerable<SelectListItem>? CategoryList { get; set; }
		public void OnGet(int id)
		{
			Category = _unitOfWork.Category.GetFirstOrDefault(x => x.Category_ID == id);
		}
		public async Task<IActionResult> OnPost()
		{
			Category = _unitOfWork.Category.GetFirstOrDefault(x => x.Category_ID == Category.Category_ID);
			if (Category != null)
			{

				_unitOfWork.Category.Remove(Category);
				_unitOfWork.Save();

				TempData["success"] = "Category deleted successfully";
				return RedirectToPage("Index");
			}
			return Page();
		}
	}
}


