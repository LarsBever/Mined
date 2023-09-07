using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;
using Result = Mined.Models.Result;

namespace Mined.Pages.Admin.TrainModel
{
    [BindProperties]
    public class TrainModel : PageModel
    {
        public Uxo Uxo { get; set; }
        public IEnumerable<Uxo> Uxos { get; set; }
        public IList<Uxo> ChosenUxos { get; set; }
        public IList<int> ChosenUxoIds { get; set; }
        public Image Image { get; set; }
        public IEnumerable<Image> Images { get; set; }
        public IList<Image> ChosenImages { get; set; }
        public Result Result { get; set; }
        public IList<Result> Results { get; set; }
        public int? ChosenNumberOfQuestions { get; set; }
        public int? ChosenCategory { get; set; }
        public int Selected_answer { get; set; }
        public int Correct_answer { get; set; }

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public TrainModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            Uxo = new();
            Image = new();
            Result = new Result();
        }
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
                //If the player wants to practice a specified category of Uxos:
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
                //If present, add all images related to the UXOs from the specified category to a list.
                Images = _unitOfWork.Image.GetAll();
                if (ChosenCategory != 0)
                {
                    foreach (var image in Images)
                    {
                        if(ChosenUxoIds.Count != 0 && image.Uxo_ID == ChosenUxoIds[0])
                        {
                            ChosenImages.Add(image);
                            ChosenUxoIds.RemoveAt(0);
                        }
                        Images = ChosenImages;
                    }
                }

            }

            //Empty the player results after the training.
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

            _unitOfWork.Result.Add(result);
            _unitOfWork.Save();

			var ResultList = _unitOfWork.Result.GetAll().ToList();

            ChosenNumberOfQuestions = HttpContext.Session.GetInt32("SessionChosenNumberOfQuestions");
            if (ResultList.Count >= ChosenNumberOfQuestions)
            {
                //Go to results page if the ChosenNumberOfQuestions has been reached
                return RedirectToPage("/Player/Results/Index");
            }
            else
            {
                //Keep training if the ChosenNumberOfQuestions has not yet been reached
                return RedirectToPage("./Train");
            }
        }
    }
}