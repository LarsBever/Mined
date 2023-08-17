using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Construction;
using Microsoft.EntityFrameworkCore;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;
using Mined.Utility;
using MySqlX.XDevAPI.Common;

namespace Mined.Pages.Player.Game.GameModel
{
    [BindProperties]
    public class GameModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public int selected_answer { get; set; }
        public int correct_answer { get; set; }

        public GameModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
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
        ////public IList<Result> Results { get; set; } = default!;
        public IEnumerable<Score> Scores { get; set; }
        //public int scoreID { get; set; }

        public int? playerScore = 0;
        public int? questionNr = 0;
        public int? numberOfMistakes = 0; 

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

        public async Task<IActionResult> OnPostAsync()
        {
            Score = _unitOfWork.Score.GetFirstOrDefault(u => u.Score_ID == HttpContext.Session.GetInt32("SessionScoreId"));

            if ((HttpContext.Session.GetInt32("SessionQuestionNr")) != null)
            {
            //    questionNr = 0;
            //}
            //else
            //{
                questionNr = HttpContext.Session.GetInt32("SessionQuestionNr");
            }

            if ((HttpContext.Session.GetInt32("SessionPlayerScore")) != null)
            {
            //    playerScore = 0;
            //}
            //else
            //{
                playerScore = HttpContext.Session.GetInt32("SessionPlayerScore");
            }

            if ((HttpContext.Session.GetInt32("SessionnumberOfMistakes")) != null)
            {
            //    numberOfMistakes = 0;
            //}
            //else
            //{
                numberOfMistakes = HttpContext.Session.GetInt32("SessionnumberOfMistakes");
            }

            var res = new Score()
            {
                correct_answer = Score.correct_answer,
                selected_answer = Score.selected_answer,
            };

            //as long as the questionnr is 5 or lower, the player can keep playing.
            if (questionNr <= 5)
            {

                if (selected_answer == correct_answer)
                {
                    playerScore++;
                    HttpContext.Session.SetInt32(SD.SessionPlayerScore, (int)playerScore);

                }
                else
                {
                    if(playerScore != 0)
                    {
                        playerScore--;
                        HttpContext.Session.SetInt32(SD.SessionPlayerScore, (int)playerScore);
                    }
                    
                    numberOfMistakes++;
                    HttpContext.Session.SetInt32(SD.SessionnumberOfMistakes, (int)numberOfMistakes);
                }
            }
            else
            {
                Score.NumberOfMistakes = (int)numberOfMistakes;
                Score.PlayerScore = (int)playerScore;
                _unitOfWork.Score.Update(Score);
                _unitOfWork.Save();

                HttpContext.Session.SetInt32(SD.SessionPlayerScore, 0);
                HttpContext.Session.SetInt32(SD.SessionQuestionNr, 0);
                HttpContext.Session.SetInt32(SD.SessionnumberOfMistakes, 0);
                return RedirectToPage("/Player/LeaderBoard/Index");
            }
            questionNr++;

            //session score en questionnr lijken goed te werken (ook bij max aantal vragen??? nog checken!!)
            //nu nog fixen dat de score goed wordt opgeslagen (ook tabel aanpassen)
            //code wat netter verwerken (evt. inkorten?)
            //ipv naar leaderboardnaar results gaan!
            //Score mag niet bewerkt kunnen worden door players = oplossen!!!
            HttpContext.Session.SetInt32(SD.SessionQuestionNr, (int)questionNr);
            return RedirectToPage("./Game");
        }
    }
}