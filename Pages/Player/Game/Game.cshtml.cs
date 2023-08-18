using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Construction;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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

        public int GetlowestHighScore(IList<Score> Scores)
        {
            foreach (var score in Scores)
            {
                if(Scores.Count > 2){
					if (score.PlayerScore < Scores[1].PlayerScore)
					{
						Scores.RemoveAt(1);
						return GetlowestHighScore(Scores);
						// de scores moet 1 minder worden en als de lijst leeg is, de waarde teruggeven
					}
					if (score.PlayerScore >= Scores[1].PlayerScore)
					{
						//if the first score is equal or higher than the other value, it is not the lowest score in the highscore. It should be deleted
						//the search for the lowest value continues.

						Scores.RemoveAt(0);
						return GetlowestHighScore(Scores);
					}
				}
                if(score.PlayerScore < Scores[1].PlayerScore)
                {
                    return Scores[0].PlayerScore;
                }
            }
			//if the list has been searched for other higher values and the list is empty, the lowest highscore can be returned.
			return Scores[1].PlayerScore;
		}

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
			//var test123 = HttpContext.Session.GetInt32("SessionScoreId");
		}

        public async Task<IActionResult> OnPostAsync()
        {
			//var test123 = HttpContext.Session.GetInt32("SessionScoreId");
			Score = _unitOfWork.Score.GetFirstOrDefault(u => u.Score_ID == HttpContext.Session.GetInt32("SessionScoreId"));

            if (HttpContext.Session.GetInt32("SessionQuestionNr") != null)
            {
            //    questionNr = 0;
            //}
            //else
            //{
                questionNr = HttpContext.Session.GetInt32("SessionQuestionNr");
            }

            if (HttpContext.Session.GetInt32("SessionPlayerScore") != null)
            {
            //    playerScore = 0;
            //}
            //else
            //{
                playerScore = HttpContext.Session.GetInt32("SessionPlayerScore");
            }

            if (HttpContext.Session.GetInt32("SessionnumberOfMistakes") != null)
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
            if (questionNr <= 2)
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
                //List of all scores, to compare current score with. This comparison is meant to Add the new score if it is high enough.
                
                var allScores = _unitOfWork.Score.GetAll().ToList();

                //Only the top 100 scores will be saved on the Leader Board
				if (allScores.Count > 100)
                {
                    //Minimale waarde van de lijst vinden en deze vergelijken
                    if(playerScore > GetlowestHighScore(allScores))
                    {
                        //find and current lowest highscore
                        var currentLowestHighscore = GetlowestHighScore(allScores);
                        var playerLowestHighScore = _unitOfWork.Score.GetFirstOrDefault(x => x.PlayerScore == currentLowestHighscore);
                        allScores.Remove(playerLowestHighScore);

                        //Save the new data
						Score.NumberOfMistakes = (int)numberOfMistakes;
						Score.PlayerScore = (int)playerScore;
						_unitOfWork.Score.Update(Score);
						_unitOfWork.Save();
					}
                    else
                    {
						//If the player's score is lower than the lowest score on the leaderboard, the score will not be added and the player is lead to the Game Over page
						HttpContext.Session.SetInt32(SD.SessionPlayerScore, 0);
                        HttpContext.Session.SetInt32(SD.SessionQuestionNr, 0);
                        HttpContext.Session.SetInt32(SD.SessionnumberOfMistakes, 0);
                        return RedirectToPage("/Player/Game/Gameover");
					}
                }
                else
                {
					Score.NumberOfMistakes = (int)numberOfMistakes;
					Score.PlayerScore = (int)playerScore;
					_unitOfWork.Score.Update(Score);
					_unitOfWork.Save();
				}

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