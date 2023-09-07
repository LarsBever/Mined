using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;
using Mined.Utility;

namespace Mined.Pages.Player.LeaderBoard
{
    public class IndexModel : PageModel
    {
		private readonly IUnitOfWork _unitOfWork;
		public IEnumerable<Score> Scores { get; set; }
		public IndexModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public void OnGet()
        {
            Scores = _unitOfWork.Score.GetAll();
		}

		public async Task<IActionResult> OnPost()
		{
			//If the player wants to play the game again, set all session values back to 0:
			HttpContext.Session.SetInt32(SD.SessionPlayerScore, 0);
			HttpContext.Session.SetInt32(SD.SessionQuestionNr, 0);
			HttpContext.Session.SetInt32(SD.SessionnumberOfMistakes, 0);
			return RedirectToPage("/Player/Game/Game");
		}
	}
}
