using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;

namespace Mined.Pages.Admin.Uxos
{
	[BindProperties]
	public class DeleteModel : PageModel
	{
		private readonly IUnitOfWork _unitOfWork;
		public DeleteModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
		}

		public Uxo? Uxo { get; set; }
		public Image? Image { get; set; }
		public IEnumerable<Image> Images { get; set; }
		public Category? Category { get; set; }
		public IEnumerable<Category>? Categories { get; set; }
		public IEnumerable<SelectListItem>? CategoryList { get; set; }
		public void OnGet(int id)
		{
			Uxo = _unitOfWork.Uxo.GetFirstOrDefault(x => x.Uxo_ID == id);
			Uxo.Category = _unitOfWork.Category.GetFirstOrDefault(x=>x.Category_ID == Uxo.Category_ID);
			Category = Uxo.Category;
			Images = _unitOfWork.Image.GetAll();
		}
		public async Task<IActionResult> OnPost()
		{
			var uxoFromDb = _unitOfWork.Uxo.GetFirstOrDefault(x => x.Uxo_ID == Uxo.Uxo_ID);
			if (uxoFromDb != null)
			{
				//If Uxo exists, remove UXO
				_unitOfWork.Uxo.Remove(uxoFromDb);
				_unitOfWork.Save();
				TempData["success"] = "U.X.O. deleted successfully";
				return RedirectToPage("Index");
			}
            TempData["error"] = "Something went wrong...";
            return Page();
		}
	}
}


