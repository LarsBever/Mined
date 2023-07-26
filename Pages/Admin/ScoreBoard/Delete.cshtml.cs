using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mined.DataAccess.Data;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;
using System.ComponentModel;
using System.Linq;

namespace Mined.Pages.Admin.Scores
{
	[BindProperties]
	public class DeleteModel : PageModel
	{
		private readonly IUnitOfWork _unitOfWork;
		public DeleteModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
		}
		public Score Score { get; set; }
		public MinedUser? MinedUser { get; set; }
		public void OnGet(int id)
		{
			Score = _unitOfWork.Score.GetFirstOrDefault(x => x.Score_ID == id);
		}
		public async Task<IActionResult> OnPost()
		{
			//Score = _unitOfWork.Category.GetFirstOrDefault(x => x.Score_ID == Score.Score_ID);
			if (Score != null)
			{

				_unitOfWork.Score.Remove(Score);
				_unitOfWork.Save();

				TempData["success"] = "`Score deleted successfully";
				return RedirectToPage("Index");
			}
			return Page();
		}
	}
}


