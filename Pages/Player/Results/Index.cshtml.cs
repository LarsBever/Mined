using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;

namespace Mined.Pages.Player.ResultsModel
{
    public class ResultsModel : PageModel
    {
        public Uxo? Uxo { get; set; }
        public IEnumerable<Uxo> Uxos { get; set; }
        public Image? Image { get; set; }
        public IEnumerable<Image> Images { get; set; }
        public Result? Result { get; set; }
        public IList<Result> Results { get; set; }
        public int? ChosenNumberOfQuestions { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public ResultsModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            Uxo = new();
            Image = new();
            Result = new();
        }

        public async Task OnGetAsync()
        {
            ChosenNumberOfQuestions = HttpContext.Session.GetInt32("SessionChosenNumberOfQuestions");
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

        //Try Again button to redirect to Delete score and redirect to game
        public async Task<IActionResult> OnPostAsync()
        {
            //The players results will be deleted before the training can be done again:
            var userResults = _unitOfWork.Result.GetAll().ToList();
            if (userResults != null)
            {
                foreach (var answer in userResults)
                {
                    _unitOfWork.Result.Remove(answer);
                }
            }
            _unitOfWork.Save();
            return RedirectToPage("/Player/Train/Train");
        }

    }
}
