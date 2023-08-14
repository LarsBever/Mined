using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;
using MySqlX.XDevAPI.Common;

namespace Mined.Pages.Admin.TrainModel
{
    [BindProperties]
    public class TrainModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public TrainModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            Uxo = new();
            Image = new();
            Score = new();
        }

        public Uxo Uxo { get; set; }
        public IEnumerable<Uxo> Uxos { get; set; }
        public Image Image { get; set; }
        public IEnumerable<Image> Images { get; set; }
        public Score Score { get; set; }
        //public IList<Result> Results { get; set; } = default!;
        public IEnumerable<Score> Scores { get; set; }
        //public int scoreID { get; set; }


        //Get Uxo for the next question and qet the scores
        public async Task OnGetAsync(int id)
        {
            if (_unitOfWork.Uxo != null)
            {
                Uxos = _unitOfWork.Uxo.GetAll();
            }
            if (_unitOfWork.Image != null)
            {
                Images = _unitOfWork.Image.GetAll();
            }
            if (_unitOfWork.Score != null)
            {
                Scores = _unitOfWork.Score.GetAll();
            }
        }

        public async Task<IActionResult> OnPostAsync(string Nickname, int id)
        {
            //If the player starts the game, a nickname in object score has to be made
            if (Score.Nickname == null)
            {
                //Nickname cannot be null
                if (Score.Nickname == null)
                {
                    return Page();
                }

                //Hier nog validatie toevoegen???
                _unitOfWork.Score.Add(Score);
                _unitOfWork.Save();
                //Score = _unitOfWork.Score.GetFirstOrDefault(c => c.Score_ID == Score.Score_ID);
                //scoreID = Score.Score_ID;
            }
            //if the nickname has been made, the game can be played
            else
            {
                //if (!ModelState.IsValid || _unitOfWork.Score == null || Score == null)
                //{
                //    return Page();
                //}
                //var playerResults = Model.Scores.Where(c => c.Score_ID == unitOfWork.Score.Score_ID).ToList();
                //_unitOfWork.Score.Update(Score);
                //_unitOfWork.Save();
                //_unitOfWork.Score.Update(Score);


                var result = new Score()
                {
                    Nickname = Score.Nickname,
                    correct_answer = Score.correct_answer,
                    selected_answer = Score.selected_answer,
                };

                _unitOfWork.Score.Add(result);
                _unitOfWork.Save();
                //_unitOfWork.Score.Update(Score);
                //_unitOfWork.Save();

                //if (_unitOfWork.Score.Where(c => c.Score_ID == scoreID).ToList().Count() >= 5)
                //{
                //    return RedirectToPage("./LeaderBoard");
                //}

                if (_unitOfWork.Score.GetAll().Count() >= 5)
                {
                    return RedirectToPage("./LeaderBoard");
                }


            }
            return RedirectToPage("./Train");
        }
    }
}


