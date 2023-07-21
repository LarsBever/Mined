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
	public class UpsertModel : PageModel
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _hostEnvironment;
		private readonly MinedDbContext _db;
		public UpsertModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_hostEnvironment = hostEnvironment;
			Uxo = new Uxo();
			Image = new Image();
			Category = new Category();
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
				Text = i.MainCategoryNato + "; " + i.SubCategoryNato,
				Value = i.Category_ID.ToString(),
			});

		}
		public async Task<IActionResult> OnPost()
		{
			string webRootPath = _hostEnvironment.WebRootPath;
			var files = HttpContext.Request.Form.Files;

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
				//Uxo needs a Category for the Insert into the Database
				Uxo.Category = _unitOfWork.Category.GetFirstOrDefault(x => x.Category_ID == Category.Category_ID);

				//To add an Image, the right Uxo, including Uxo_ID is needed. For this, we first need to insert the Uxo into the database.
				//The Uxo_ID is autoincremented by the Database.
				//We store the NameNato value in 'overdracht', so that we can retrieve the right Uxo object from the database after it is inserted.
				string overdracht = Uxo.NameNato.ToString();
				_unitOfWork.Uxo.Add(Uxo);
				_unitOfWork.Save();

				//Now that the Uxo is inserted and the Uxo_ID is created, we can add the Images.
				//Add Image
				//Uxo = _unitOfWork.Uxo.GetFirstOrDefault(x => x.NameNato == overdracht);
				//Image.Uxo = _unitOfWork.Uxo.GetFirstOrDefault(x => x.NameNato == overdracht);
				Uxo.NameNato = overdracht;
				_unitOfWork.Image.Add(Image);
				_unitOfWork.Save();
				//Image.Image_ID = 0;

				TempData["success"] = "U.X.O. created successfully";
				return RedirectToPage("Index");
			}
			OnGet();
			return Page();
		}
	}
}


