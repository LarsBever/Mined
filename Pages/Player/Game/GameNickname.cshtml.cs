using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;
using Mined.Utility;

namespace Mined.Pages.Player.Game.GameNicknameModel
{
    [BindProperties]
    public class GameNicknameModel : PageModel
    {
        public Score Score { get; set; }
        public IEnumerable<Score> Scores { get; set; }
        public IEnumerable<Uxo> Uxos { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public GameNicknameModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            Score = new();
        }
        public async Task OnGetAsync(int id)
        {
            if (_unitOfWork.Score != null)
            {
                Scores = _unitOfWork.Score.GetAll();
            }
            if (_unitOfWork.Uxo != null)
            {
                Uxos = _unitOfWork.Uxo.GetAll();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //If the player starts the game, a nickname in object score has to be made
            //Nickname cannot be null
            Scores = _unitOfWork.Score.GetAll();
            
            if (Score.Nickname == null)
            {
				ModelState.AddModelError(string.Empty, "Oops, looks like you forgot to fill in a Nickname...");
            }
            else
            {
				if (Scores.Any(c => c.Nickname == Score.Nickname))
				{
					ModelState.AddModelError(string.Empty, "Too bad! looks like someone has already picked this Nickname");
				}
				else
				{
					_unitOfWork.Score.Add(Score);
					_unitOfWork.Save();
					HttpContext.Session.SetInt32(SD.SessionScoreId, (_unitOfWork.Score.GetFirstOrDefault(
													u => u.Score_ID == Score.Score_ID)).Score_ID);
					return RedirectToPage("./Game");
				}
			}
            return Page();
		}
    }
}


