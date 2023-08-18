using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Construction;
using Microsoft.EntityFrameworkCore;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;
using System.Linq;
using Mined.Utility;
using MySqlX.XDevAPI.Common;
using System.Collections.Generic;
using System.Collections;
using Result = Mined.Models.Result;

namespace Mined.Pages.Admin.TrainModel
{
    [BindProperties]
    public class TrainModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public int Selected_answer { get; set; }
        public int Correct_answer { get; set; }

        public TrainModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            Uxo = new();
            Image = new();
            Result = new Result();
        }

        public Uxo Uxo { get; set; }
        public IEnumerable<Uxo> Uxos { get; set; }
        public IList<Uxo> ChosenUxos { get; set; }
        public IList<Uxo> ChosenImages { get; set; }
        public IList<int> ChosenUxoIds { get; set; }
        public Image Image { get; set; }
        public IEnumerable<Image> Images { get; set; }
        public Result Result { get; set; }
        public IList<Result> Results { get; set; }
        public int? ChosenNumberOfQuestions { get; set; }
        public int? ChosenCategory { get; set; } 

        public async Task OnGetAsync(int id)
        {
            //Retrieve the category chosen by the player
            ChosenCategory = HttpContext.Session.GetInt32("SessionChosenCategory");
            IList<Uxo> ChosenUxos = new List<Uxo>();
            IList<int> ChosenUxoIds = new List<int>();
            IList<Image> ChosenImages = new List<Image>();

            if (_unitOfWork.Uxo != null)
            {
                Uxos = _unitOfWork.Uxo.GetAll();
                //If the player wants to practice a specified category of Uxos, the chosenCategory != 0.
                if (ChosenCategory != 0)
                {
                    foreach (var uxo in Uxos)
                    {
                        //If the UXO matches the chosen category, add Uxo to the list for training.
                        if (uxo.Category_ID == ChosenCategory)
                        {
                            ChosenUxos.Add(uxo);
                            ChosenUxoIds.Add(uxo.Uxo_ID);
                        }
                        Uxos = ChosenUxos;
                    }
                }
            }

            if (_unitOfWork.Image != null)
            {
                Images = _unitOfWork.Image.GetAll();
                if (ChosenCategory != 0)
                {
                    foreach (var image in Images)
                    {
                        if(ChosenUxoIds.Count != 0 && image.Uxo_ID == ChosenUxoIds[0])
                        {
                            ChosenImages.Add(Image);
                            ChosenUxoIds.RemoveAt(0);
                        }
                        Images = ChosenImages;
                    }
                }

            }

            if (_unitOfWork.Result != null)
            {
                Results = _unitOfWork.Result.GetAll().ToList();
				if (Results.Count >= ChosenNumberOfQuestions)
                {
					foreach (var answer in Results)
					{
						_unitOfWork.Result.Remove(answer);
                        _unitOfWork.Save();
					}
				};
			}
           
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            

            var result = new Result();
            result.Correct_answer = Correct_answer;
            result.Selected_answer = Selected_answer;

            ////scores moet goed worden doorgegeven in de sessie en goed worden opgeslagen
            //HttpContext.Session.Set<IList>(SD.SessionScores, PlayerResults);
            _unitOfWork.Result.Add(result);
            _unitOfWork.Save();
			//HttpContext.Session.SetInt32(SD.SessionTrainingId, _unitOfWork.Result.GetFirstOrDefault(
			//                                                    u => u.TrainingId == Result.TrainingId).TrainingId);

			//as long as the questionnr is 5 or lower, the player can keep playing.

			//To add the new answer, first all previous answers/ results need to be retrieved.
			var ResultList = _unitOfWork.Result.GetAll().ToList();

            ChosenNumberOfQuestions = HttpContext.Session.GetInt32("SessionChosenNumberOfQuestions");
            if (ResultList.Count >= ChosenNumberOfQuestions)
            {
                return RedirectToPage("/Player/Results/Index");
            }
            else
            {
                return RedirectToPage("./Train");
            }
            //session score en questionnr lijken goed te werken (ook bij max aantal vragen??? nog checken!!)
            //nu nog fixen dat de score goed wordt opgeslagen (ook tabel aanpassen)
            //code wat netter verwerken (evt. inkorten?)
            //ipv naar leaderboardnaar results gaan!
            //Score mag niet bewerkt kunnen worden door players = oplossen!!!

            ///Score mag niet in de min gaan!!
        }
    }
}