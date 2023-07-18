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
    public class CreateModel : PageModel
    {
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _hostEnvironment;
		public CreateModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_hostEnvironment = hostEnvironment;
		}
		public Uxo Uxo { get; set; }
		public Image Image { get; set; }
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
			string webRootPath = _hostEnvironment.WebRootPath;
			var files = HttpContext.Request.Form.Files;

			if(Uxo.Uxo_ID ==  0)
			{
				Image.NameNato = Uxo.NameNato;
				Uxo.CategoryId = Category.Category_ID;

				//this is a create req.
				string fileName_new = Guid.NewGuid().ToString();	
				var uploads = Path.Combine(webRootPath, @"images\UxoImages");
				var extension = Path.GetExtension(files[0].FileName);

				using (var fileStream = new FileStream(Path.Combine(uploads, fileName_new + extension), FileMode.Create))
				{
					files[0].CopyTo(fileStream);
				}
				Image.UxoImage = @"\images\UxoImages\" + fileName_new + extension;

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

					//Image.UxoId =

				 //   Uxo_ID = _unitOfWork.Uxo.GetFirstOrDefault(u => u.Uxo_ID == Uxo.Uxo_ID);
					_unitOfWork.Image.Add(Image);
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


