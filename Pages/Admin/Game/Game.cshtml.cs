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

namespace MinedApp.Pages.Admin.GameModel
{
    [BindProperties]
    public class GameModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public GameModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
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
     

        public async Task OnGetAsync(int id)
        {
            Uxos = _unitOfWork.Uxo.GetAll();
            Images = _unitOfWork.Image.GetAll();
            Scores = _unitOfWork.Score.GetAll();
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
                UxoMistakes =   Score.UxoMistakes,
                PlayerScore =   Score.PlayerScore
            };

            _unitOfWork.Score.Add(score);
            _unitOfWork.Save();

            if (_unitOfWork.Score.GetFirstOrDefault(c => c.Nickname == Score.Nickname).ToList().Count() >= 10)
            {
                return RedirectToPage("./Score");
            }

            return RedirectToPage("./Game");
        }

    }
}
