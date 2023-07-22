using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mined.DataAccess.Data;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;
using System.ComponentModel;
using System.Linq;

namespace Mined.Pages.Admin.Uxos
{
	[BindProperties]
	public class DeleteModel : PageModel
	{
		private readonly IUnitOfWork _unitOfWork;
		public DeleteModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
		}

		public Uxo? Uxo { get; set; }
		public Image? Image { get; set; }
		public Category? Category { get; set; }
		public IEnumerable<Category>? Categories { get; set; }
		public IEnumerable<SelectListItem>? CategoryList { get; set; }
		public void OnGet(int id)
		{
			Uxo = _unitOfWork.Uxo.GetFirstOrDefault(x => x.Uxo_ID == id);
			Uxo.Category = _unitOfWork.Category.GetFirstOrDefault(x=>x.Category_ID == Uxo.Category_ID);
			Category = Uxo.Category;
		}
		public async Task<IActionResult> OnPost()
		{
			var uxoFromDb = _unitOfWork.Uxo.GetFirstOrDefault(x => x.Uxo_ID == Uxo.Uxo_ID);
			if (uxoFromDb != null)
			{
				
				//Uxo needs a Category for the Insert into the Database
				//Uxo.Category = _unitOfWork.Category.GetFirstOrDefault(x => x.Category_ID == Category.Category_ID);

				//To add an Image, the correct Uxo object, including Uxo_ID is needed. For this, we first need to insert the Uxo into the database.
				//The Uxo_ID is autoincremented by the Database.
				//We store the NameNato value in 'overdracht', so that we can retrieve the right Uxo object from the database after it is inserted.
				//string overdracht = Uxo.NameNato.ToString();
				_unitOfWork.Uxo.Remove(uxoFromDb);
				_unitOfWork.Save();

				//Now that the Uxo is inserted and the Uxo_ID is created, we can add the Images.
				////Add Image
				//Uxo = _unitOfWork.Uxo.GetFirstOrDefault(x => x.NameNato == overdracht);
				//Image.Uxo = _unitOfWork.Uxo.GetFirstOrDefault(x => x.NameNato == overdracht);
				//Uxo.NameNato = overdracht;
				//_unitOfWork.Image.Add(Image);
				TempData["success"] = "U.X.O. deleted successfully";
				return RedirectToPage("Index");
			}
			return Page();
		}
	}
}


