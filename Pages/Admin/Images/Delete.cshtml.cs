using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Mined.DataAccess.Data;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;


namespace Mined.Pages.Admin.Images
{
    [BindProperties]
    public class DeleteModel : PageModel
    {
		private readonly IUnitOfWork _unitOfWork;
		public DeleteModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public Uxo Uxo { get; set; }
		public Image Image { get; set; }
		public Category Category { get; set; }
		public IEnumerable<Category> Categories { get; set; } 

		public void OnGet(int Uxo_ID)
        {
			Uxo = _unitOfWork.Uxo.GetFirstOrDefault(u=>u.Uxo_ID==Uxo.Uxo_ID);
        }
		public async Task<IActionResult> OnPost()
        {
			var uxoFromDb = _unitOfWork.Uxo.GetFirstOrDefault(u => u.Uxo_ID == Uxo.Uxo_ID);
			if (uxoFromDb != null) 
				{
					_unitOfWork.Uxo.Remove(Uxo);
					_unitOfWork.Image.Remove(Image);
					_unitOfWork.Save();
					TempData["success"] = "U.X.O. deleted successfully";
					return RedirectToPage("Index");
				}
            return Page();
        }
    }
}


