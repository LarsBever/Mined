using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;
using Mined.Utility;

namespace Mined.Pages.Player.Game.GameModel
{
    [BindProperties]
    public class GameModel : PageModel
    {
        public int? playerScore = 0;
        public int? questionNr = 0;
        public int? numberOfMistakes = 0;
        public int nrOfHighScores = 10;
        public int maxNrOfQuestions = 5;
        public Uxo Uxo { get; set; }
        public IEnumerable<Uxo> Uxos { get; set; }
        public Image Image { get; set; }
        public IEnumerable<Image> Images { get; set; }
        public Score Score { get; set; }
        public IEnumerable<Score> Scores { get; set; }
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

        //To make the highscore, the player has to beat at least the lowest score (2th place).
        //this Method calculates the lowest score on the leaderboard.
        public int GetlowestHighScore(IList<Score> Scores)
        {
            foreach (var score in Scores)
            {
                
                if(Scores.Count > (nrOfHighScores-1))
                {
					if (score.PlayerScore < Scores[1].PlayerScore)
					{
						Scores.RemoveAt(1);
						return GetlowestHighScore(Scores);
					}
					if (score.PlayerScore >= Scores[1].PlayerScore)
					{
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
		}

        public async Task<IActionResult> OnPostAsync()
        {
            //Retrieve Score from the database
			Score = _unitOfWork.Score.GetFirstOrDefault(u => u.Score_ID == HttpContext.Session.GetInt32("SessionScoreId"));

            //If present, retrieve player data (Questionnr, Score and nr of Mistakes) from session to continue the game:
            if (HttpContext.Session.GetInt32("SessionQuestionNr") != null)
            {
                questionNr = HttpContext.Session.GetInt32("SessionQuestionNr");
            }
            if (HttpContext.Session.GetInt32("SessionPlayerScore") != null)
            {
                playerScore = HttpContext.Session.GetInt32("SessionPlayerScore");
            }
            if (HttpContext.Session.GetInt32("SessionnumberOfMistakes") != null)
            {
                numberOfMistakes = HttpContext.Session.GetInt32("SessionnumberOfMistakes");
            }

            var res = new Score()
            {
                correct_answer = Score.correct_answer,
                selected_answer = Score.selected_answer,
            };

            //as long as the questionnr is equal to the MaxNrOfQuestions lower, the player can keep playing.
            if (questionNr <= maxNrOfQuestions)
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
            //When the max number of Questions has been reached,
            //the player's score can be compared to the highscores to see if the player made it to the Leaderboard.
            else
            {
                //List of all scores, to compare current score with.
                var allScores = _unitOfWork.Score.GetAll().ToList();

                //Only the top scores will be saved on the Leader Board (see nrOfHighScores).This nr can be changed
				if (allScores.Count > (nrOfHighScores-1))
                {
                    //Check if the player's score is higher than the lowest score on the leaderboard.
                    if(playerScore > GetlowestHighScore(allScores))
                    {
                        //find and current lowest highscore
                        var currentLowestHighscore = GetlowestHighScore(allScores);
                        var playerLowestHighScore = _unitOfWork.Score.GetFirstOrDefault(x => x.PlayerScore == currentLowestHighscore);
                        allScores.Remove(playerLowestHighScore);

                        //Save the new data when the player's score is high enough.
						Score.NumberOfMistakes = (int)numberOfMistakes;
						Score.PlayerScore = (int)playerScore;
						_unitOfWork.Score.Update(Score);
						_unitOfWork.Save();
					}
                    else
                    {
                        //If the player's score is lower than the lowest score on the leaderboard,
                        //the score will not be added and the player is lead to the Game Over page.
                        //Reset all session values to 0.
                        HttpContext.Session.SetInt32(SD.SessionPlayerScore, 0);
                        HttpContext.Session.SetInt32(SD.SessionQuestionNr, 0);
                        HttpContext.Session.SetInt32(SD.SessionnumberOfMistakes, 0);
                        return RedirectToPage("/Player/Game/Gameover");
					}
                }
                //If there's still room on the Leaderboard,
                //the player's score does not have to be compared and can be simply added.
                else
                {
					Score.NumberOfMistakes = (int)numberOfMistakes;
					Score.PlayerScore = (int)playerScore;
					_unitOfWork.Score.Update(Score);
					_unitOfWork.Save();
				}
            }
            questionNr++;
            HttpContext.Session.SetInt32(SD.SessionQuestionNr, (int)questionNr);
            return RedirectToPage("./Game");
        }
    }
}