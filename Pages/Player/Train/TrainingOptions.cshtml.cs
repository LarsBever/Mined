using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;
using Mined.Utility;

namespace Mined.Pages.Player.Train.TrainingOptionsModel
{
    [BindProperties]
    public class TrainingOptionsModel : PageModel
    {
        public Category? Category { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
        public Uxo? Uxo { get; set; }
        public IEnumerable<Uxo> Uxos { get; set; }
        public int ChosenNumberOfQuestions { get; set; }
        public int ChosenCategory { get; set; }
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        public TrainingOptionsModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }
        //Method to fill the Category Drop Down List
        public void FillDropDownList()
        {
            CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem()
            {
                Text = i.CategoryName,
                Value = i.CategoryName.ToString(),
            });
        }
        public void OnGet()
        {
            if (_unitOfWork.Uxo != null)
            {
                Uxos = _unitOfWork.Uxo.GetAll();
            }

            //Fill Category Dropdownlist
            FillDropDownList();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            HttpContext.Session.SetInt32(SD.SessionChosenNumberOfQuestions, ChosenNumberOfQuestions);
            HttpContext.Session.SetInt32(SD.SessionChosenCategory, ChosenCategory);
            return RedirectToPage("./Train");
        }
    }
}
