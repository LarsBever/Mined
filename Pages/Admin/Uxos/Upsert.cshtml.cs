using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;
using System.ComponentModel;

namespace Mined.Pages.Admin.Uxos
{
	[BindProperties]
	public class UpsertModel : PageModel
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _hostEnvironment;
		public UpsertModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_hostEnvironment = hostEnvironment;
			Uxo = new Uxo();
			Image = new Image();
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
				////Uxo heeft een CategoryId nodig om de UXO toe te voegen
				//Uxo.Category = _unitOfWork.Category.GetFirstOrDefault(x => x.Category_ID == Category.Category_ID);
				_unitOfWork.Uxo.Add(Uxo);
				Category = null;
				//_unitOfWork.Save();

				//Uxo = _unitOfWork.Uxo.GetFirstOrDefault(x => x.NameNato == Uxo.NameNato);
				//Image.UxoId = Uxo.Uxo_ID;

				////Uxo heeft een CategoryId nodig om de UXO toe te voegen
				//Uxo.Category = _unitOfWork.Category.GetFirstOrDefault(x => x.Category_ID == Category.Category_ID);
				//Uxo = _unitOfWork.Uxo.GetFirstOrDefault(x => x.NameNato == Uxo.NameNato);
				//Image.UxoId = Uxo.Uxo_ID;
				//Image heeft een imageID nodig om de image toe te voegen

				 _unitOfWork.Image.Add(Image);
				_unitOfWork.Save();
				TempData["success"] = "U.X.O. created successfully";
				return RedirectToPage("Index");
			}
			OnGet();
			return Page();
		}
	}
}


