using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mined.Pages.Admin.GameModel
{
    [BindProperties]
    public class GameModel : PageModel
    {
		private readonly UserManager<IdentityUser> userManager;
		private readonly SignInManager<IdentityUser> signInManager;
		private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public GameModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
			this.userManager = userManager;
			this.signInManager = signInManager;
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
            if (!ModelState.IsValid || _unitOfWork.Score == null || Score == null)
            {
                return Page();
            }

            var score = new Score()
            {
                Score_ID = Score.Score_ID,
                correct_answer = Score.correct_answer,
                selected_answer = Score.selected_answer,
                Nickname = Score.Nickname,
                NumberOfMistakes = Score.NumberOfMistakes,
                UxoMistakes = Score.UxoMistakes,
                PlayerScore = Score.PlayerScore,

				//Nickname moet worden gebruikt ipv user_id
				//user_id is alleen voor het inloggen van admins
				//Er moet wel gecontroleerd worden of de nickname niet al bestaat.
			};

            _unitOfWork.Score.Add(score);
            _unitOfWork.Save();

            if (_unitOfWork.Score.GetAll().Count() >= 5)
            {
                return RedirectToPage("./Score");
            }

            return RedirectToPage("./Game");
        }

    }
}
