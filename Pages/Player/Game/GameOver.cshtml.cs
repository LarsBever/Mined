using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mined.DataAccess.Repository.IRepository;
using Mined.Utility;

namespace Mined.Pages.Player.Game
{
    public class GameOverModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;

        public GameOverModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        public void OnGet()
        {
		}

		public async Task<IActionResult> OnPost()
		{
            //Set all session values back to 0 and restart the game with the same Nickname.
			HttpContext.Session.SetInt32(SD.SessionPlayerScore, 0);
			HttpContext.Session.SetInt32(SD.SessionQuestionNr, 0);
			HttpContext.Session.SetInt32(SD.SessionnumberOfMistakes, 0);
			return RedirectToPage("./Game");
		}
    }
}
