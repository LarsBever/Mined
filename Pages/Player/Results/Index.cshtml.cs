using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;

namespace Mined.Pages.Player.ResultsModel
{
    //[Authorize(Roles = "RegularUser, Administrator")]
    public class ResultsModel : PageModel
    {
		//private readonly Mined.Data.QuizAppContext _context;
		//private readonly UserManager<IdentityUser> userManager;

		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _hostEnvironment;

		//public ResultsModel(QuizApp.Data.QuizAppContext context, UserManager<IdentityUser> userManager)
  //      {
  //          _context = context;
  //          this.userManager = userManager;
  //      }

		public ResultsModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_hostEnvironment = hostEnvironment;
			Uxo = new();
			Image = new();
			Score = new();
		}
		public Uxo? Uxo { get; set; }
		public IEnumerable<Uxo> Uxos { get; set; }
		public Image? Image { get; set; }
		public IEnumerable<Image> Images { get; set; }
		public Score? Score { get; set; }
		public IEnumerable<Score> Scores { get; set; }

		public async Task OnGetAsync()
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

        //Try Again button to redirect to Delete score and redirect to game
		public async Task<IActionResult> OnPostAsync()
        {
            
            //Hier moet de computer weten bij welke nickname de score/het antwoord hoort.
            // dus iets in de trend van var nickname = nickname...
            // daarnaast moet de computer weten bij welke score de antwoorden toegevoegd moeten worden
            var uScoreID = Score.Score_ID;
            var uScore = _unitOfWork.Score.GetFirstOrDefault(c => c.Score_ID == Score.Score_ID);

            if (uScore != null)
            {
                // Here the previous answers will be deleted and all data except Score_ID and nickname are set to 0 or null.


                //foreach (var answer in uScore)
                //{
                //_unitOfWork.Score.Remove(answer);
                //}
                Score.NumberOfMistakes = 0;
                Score.PlayerScore = 0;

                _unitOfWork.Score.Update(Score);
            }

            _unitOfWork.Save();

            return RedirectToPage("/Player/Train/Train");
        }

    }
}
