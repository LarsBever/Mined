using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mined.DataAccess.Data;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;

namespace Mined.Pages.Admin.Images
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
			//Category = new Category();
		}
		public Uxo Uxo { get; set; }
		public Image Image { get; set; }

		//Voor het toevoegen van een image aan de uxo, moet er een Uxo_ID meegegeven worden.
		//Dit kan doorgegeven worden mbv de zojuist toegevoegde uxo. Deze gegevens moeten kunnen worden opgehaald
		/// /(dit gebeurt ook als je bij uxo van index naar uxo upsert gaat.
		//Voor het updaten van een image aan de uxo, moet ook een Uxo_ID meegegeven worden
		//optie: read table met zoekfunctie (datatables) --> knop in uxo index tabel????


		public IEnumerable<Category> Categories { get; set; } 
		public IEnumerable<SelectListItem> CategoryList { get; set; }	
		public void OnGet()
        {


        }
		public async Task<IActionResult> OnPost()
		{
			string webRootPath = _hostEnvironment.WebRootPath;
			var files = HttpContext.Request.Form.Files;

			if(Uxo.Uxo_ID ==  0)
			{
			
				Image.Uxo_ID = Uxo.Uxo_ID;

				//this is a create req.
				string fileName_new = Guid.NewGuid().ToString();	
				var uploads = Path.Combine(webRootPath, @"images\UxoImages");
				var extension = Path.GetExtension(files[0].FileName);

				using (var fileStream = new FileStream(Path.Combine(uploads, fileName_new + extension), FileMode.Create))
				{
					files[0].CopyTo(fileStream);
				}
				Image.UxoImage = @"\images\UxoImages\" + fileName_new + extension;

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


