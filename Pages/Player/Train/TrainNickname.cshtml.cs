using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;
using Mined.Utility;
using MySqlX.XDevAPI.Common;
using System.Linq;

namespace Mined.Pages.Admin.TrainNicknameModel
{
    [BindProperties]
    public class TrainNicknameModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public TrainNicknameModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            Score = new();
            //Uxo = new();
        }
        public Score Score { get; set; }
        //public IList<Result> Results { get; set; } = default!;
        public IEnumerable<Score> Scores { get; set; }
        //public int scoreID { get; set; }

        //public Uxo Uxo { get; set; }
        //public IList<Result> Results { get; set; } = default!;
        public IEnumerable<Uxo> Uxos { get; set; }
        //public int scoreID { get; set; }


        //Get Uxo for the next question and qet the scores
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
            if (Uxos.Count() == 0)
            {
                ModelState.AddModelError(string.Empty, "Oh dear, looks like someone forgot to add the U.X.Os. please add" +
                                                       " at least 4 U.X.O.s in the admin menu (Admin>UXO>Add UXO");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //If the player starts the game, a nickname in object score has to be made
            //Nickname cannot be null
            Scores = _unitOfWork.Score.GetAll();
            
            if (Score.Nickname == null )
            {
                return Page();
            }
            if (Scores.Any(c => c.Nickname == Score.Nickname))
            {
                ModelState.AddModelError(string.Empty, "Too bad! looks like someone has already picked this Nickname");
                return Page();
            }
            else
            {
                _unitOfWork.Score.Add(Score);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionScoreId, (_unitOfWork.Score.GetFirstOrDefault(
                                                u => u.Score_ID == Score.Score_ID)).Score_ID);
                return RedirectToPage("./Train");
            }
        }
    }
}


