using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Mined.DataAccess.Data;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;
using System.ComponentModel;
using System.Linq;


namespace Mined.Pages.Admin.Scores
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
			Score = new();
		}
		public int Id { get; set; }
		public Score? Score { get; set; }
		public void OnGet(int id)
		{
			if (id != 0)
			{
				//Update item
				Score = _unitOfWork.Score.GetFirstOrDefault(x => x.Score_ID == id);
			}
		}
		public async Task<IActionResult> OnPost()
		{
			if (Score.Score_ID == 0)
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
					_unitOfWork.Score.Add(Score);
					_unitOfWork.Save();
					TempData["success"] = "Score added successfully";
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
					_unitOfWork.Score.Update(Score);
					_unitOfWork.Save();
				}
				TempData["success"] = "Score saved successfully";
				return RedirectToPage("Index");
			}
			return Page();
		}
	}
}


