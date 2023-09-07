using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;

namespace Mined.Pages.Admin.LeaderBoard
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
		public IEnumerable<Score> Scores { get; set; }
		public void OnGet(int id)
		{
			if (id != 0)
			{
				//Update score
				Score = _unitOfWork.Score.GetFirstOrDefault(x => x.Score_ID == id);
			}
		}
		public async Task<IActionResult> OnPost()
		{
			//Add new score
			if (Score.Score_ID == 0)
			{
				Scores = _unitOfWork.Score.GetAll();
				//Check if nickname does not already exist.
				if (Scores.Any(c => c.Nickname == Score.Nickname))
				{
					ModelState.AddModelError(string.Empty, "Too bad! this Nickname has already been picked!");
				}
				//Validate Nr of mistakes (cannot be negative)
				if (Score.NumberOfMistakes < 0)
				{
					ModelState.AddModelError(string.Empty, "Nope, it's practically impossible to get a negative nr of mistakes in this game... " +
															" The nr of mistakes should be 0 or higher");
				}
				//Validate playerscore (cannot be negative)
				if (Score.PlayerScore < 0)
				{
					ModelState.AddModelError(string.Empty, "Nope, it's practically impossible to get a negative score in this game... " +
															" The score should be 0 or higher");
				}
				if (ModelState.IsValid)
				{
					_unitOfWork.Score.Add(Score);
					_unitOfWork.Save();
					TempData["success"] = "Score added successfully";
					return RedirectToPage("Index");
				}
			}
			//Update existing score.
			else
			{
				if (ModelState.IsValid)
				{
					_unitOfWork.Score.Update(Score);
					_unitOfWork.Save();
				}
				TempData["success"] = "Score updated successfully";
				return RedirectToPage("Index");
			}
			TempData["error"] = "Something went wrong...";
			return Page();
		}
	}
}


