using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;

namespace Mined.Pages.Admin.LeaderBoard
{
	[BindProperties]
	public class DeleteModel : PageModel
	{
        public Score Score { get; set; }
        private readonly IUnitOfWork _unitOfWork;
		public DeleteModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
		}
		public void OnGet(int id)
		{
			Score = _unitOfWork.Score.GetFirstOrDefault(x => x.Score_ID == id);
		}
		public async Task<IActionResult> OnPost()
		{
			if (Score != null)
			{
				_unitOfWork.Score.Remove(Score);
				_unitOfWork.Save();
				TempData["success"] = "Score deleted successfully";
				return RedirectToPage("Index");
			}
            TempData["error"] = "Something went wrong...";
            return Page();
		}
	}
}


