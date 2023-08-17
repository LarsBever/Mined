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
        public Image Image { get; set; }
        public IEnumerable<Image> Images { get; set; }
        public Result Result { get; set; }
        public IList<Result> Results { get; set; }
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
                Results = _unitOfWork.Result.GetAll().ToList();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            //playerScores = _unitOfWork.Score.GetFirstOrDefault(u => u.Score_ID == HttpContext.Session.GetInt32("SessionScoreId"));

            // eerst alle scores ophalen, dan de specifieke ophalen met goede ID.
            // //Daarna toevoegen aan 2e lijst die je gebruikt voor de rest??

            var ResultList = _unitOfWork.Result.GetAll().ToList();


            foreach (var results in ResultList)
            {
               Results.Add(results);
            }

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

            if (Results.Count >= 5)
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