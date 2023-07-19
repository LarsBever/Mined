using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mined.DataAccess.Data;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;

namespace Mined.Pages.Admin.Uxos
{
    [BindProperties]
    public class UpsertModel : PageModel
    {
		private readonly IUnitOfWork _unitOfWork;
		public UpsertModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
			Uxo = new Uxo();
		}
		public Uxo Uxo { get; set; }
		public Category Category { get; set; }
		public IEnumerable<Category> Categories { get; set; } 
		public IEnumerable<SelectListItem> CategoryList { get; set; }	
		public void OnGet()
        {
			CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem()
			{
				Text = i.MainCategoryNato +"; "+ i.SubCategoryNato,
				Value =i.Category_ID.ToString(),
			});

        }
		public async Task<IActionResult> OnPost()
		{
			var files = HttpContext.Request.Form.Files;

			if(Uxo.Uxo_ID ==  0)
			{
				Uxo.CategoryId = Category.Category_ID;

				if (Uxo.NameNato == Category.MainCategoryNato)
				{
					ModelState.AddModelError(string.Empty, "The Main Category and U.X.O. Name cannot be the same");
				}
				if (Uxo.NameNato == Category.SubCategoryNato)
				{
					ModelState.AddModelError(string.Empty, "The Subcategory and U.X.O. Name cannot be the same");
				}
				if (ModelState.IsValid)
				{
					_unitOfWork.Uxo.Add(Uxo);
					_unitOfWork.Save();
					TempData["success"] = "U.X.O. created successfully";
					return RedirectToPage("Index");
				}

			}
			else
			{
				//this is an edit req.
			}
			//is onderstaande nog nodig?
			//Image.UxoId = Uxo.Uxo_ID;
			//Uxo.CategoryId = Category.Category_ID;
			OnGet();
			return Page();
		}
	}
}


