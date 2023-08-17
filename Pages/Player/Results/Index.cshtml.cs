using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
            Result = new();
        }
        public Uxo? Uxo { get; set; }
        public IEnumerable<Uxo> Uxos { get; set; }
        public Image? Image { get; set; }
        public IEnumerable<Image> Images { get; set; }
        public Result? Result { get; set; }
        public IList<Result> Results { get; set; }

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
                Results = _unitOfWork.Result.GetAll().ToList();
            }
        }

        //Try Again button to redirect to Delete score and redirect to game
        public async Task<IActionResult> OnPostAsync()
        {

            //Hier moet de computer weten bij welke nickname de score/het antwoord hoort.
            // dus iets in de trend van var nickname = nickname...
            // daarnaast moet de computer weten bij welke score de antwoorden toegevoegd moeten worden
            //var uScoreID = Score.Score_ID;
            //var uScore = _unitOfWork.Score.GetFirstOrDefault(c => c.Score_ID == Score.Score_ID);

            // Here the previous answers will be deleted and all data except Score_ID and nickname are set to 0 or null.

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
