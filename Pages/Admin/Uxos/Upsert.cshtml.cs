using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Mined.DataAccess.Data;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;
using System.ComponentModel;
using System.Linq;
using static System.Formats.Asn1.AsnWriter;

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
			Uxo = new();
			Image = new();
			Category = new();
		}
		public int Id { get; set; }
		public Uxo? Uxo { get; set; }
		public IEnumerable<Uxo> Uxos { get; set; }
		public Image? Image { get; set; }
		public IEnumerable<Image> Images { get; set; }
		public Category? Category { get; set; }
		public IEnumerable<Category> Categories { get; set; }
		public IEnumerable<SelectListItem>? CategoryList { get; set; }

		public void FillDropDownList()
		{
			CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem()
			{
				Text = i.CategoryName,
				Value = i.CategoryName.ToString(),
			});
		}

		public void OnGet(int id)
		{
			//For Update: load all images related to the U.X.O. 
			Images = _unitOfWork.Image.GetAll();

			if (id != 0)
			{
				//Update item
				Uxo = _unitOfWork.Uxo.GetFirstOrDefault(x => x.Uxo_ID == id);
				Uxo.Category = _unitOfWork.Category.GetFirstOrDefault(x => x.Category_ID == Uxo.Category_ID);
				Category = Uxo.Category;
				Image = _unitOfWork.Image.GetFirstOrDefault(x => x.Uxo_ID == Uxo.Uxo_ID);

			}			
			FillDropDownList();
		}

		public async Task<IActionResult> OnPost()
		{
			//Check category to find categoryname, which will be used as foldername in the imagepath
			Category = _unitOfWork.Category.GetFirstOrDefault(x => x.CategoryName == Category.CategoryName);

			//Category chosenCategory = Uxo.Category;
			string nameCategoryFolder = Category.CategoryName;

			//string nameCategoryFolder = chosenCategory.CategoryName;
			string webRootPath = _hostEnvironment.WebRootPath;
			var files = HttpContext.Request.Form.Files;

			if (Uxo.Uxo_ID == 0)
			{
				//Add
				//Check if UXO does not already exist:
				Uxos = _unitOfWork.Uxo.GetAll();
				if (Uxos.Any(c => c.NameNato == Uxo.NameNato))
				{
					ModelState.AddModelError(string.Empty, "It appears this UXO already exists in the database");
					FillDropDownList();
				}
				else
				{
					//string fileName_new = Guid.NewGuid().ToString();

					var uploads = Path.Combine(webRootPath, @"images\UxoImages\" + nameCategoryFolder);
					var extension = Path.GetExtension(files[0].FileName);

					//Add new image
					using (var fileStream = new FileStream(Path.Combine(uploads, files[0].FileName + extension), FileMode.Create))
					{
						files[0].CopyTo(fileStream);
					}
					Image.UxoImage = @"\images\UxoImages\" + nameCategoryFolder + @"\" + files[0].FileName + extension;

					if (Uxo.NameNato == Category.CategoryName)
					{
						ModelState.AddModelError(string.Empty, "Category and U.X.O. Name cannot be the same");
					}

					if (ModelState.IsValid)
					{
						//Uxo needs a Category object for the insert into the Database
						Uxo.Category = _unitOfWork.Category.GetFirstOrDefault(x => x.Category_ID == Category.Category_ID);

						//To add an Image, the correct Uxo object, including Uxo_ID is needed. For this, we first need to insert the Uxo into the database.
						//The Uxo_ID is autoincremented by the Database.
						//We store the NameNato value in 'overdracht', so that we can retrieve the right Uxo object from the database after it is inserted.
						string transferstring = Uxo.NameNato.ToString();
						_unitOfWork.Uxo.Add(Uxo);
						_unitOfWork.Save();

						//Now that the Uxo is inserted and the Uxo_ID is created, we can add the Images using the U.X.O. Name.
						//Add Image
						Uxo = _unitOfWork.Uxo.GetFirstOrDefault(x => x.NameNato == transferstring);
						Image.Uxo = _unitOfWork.Uxo.GetFirstOrDefault(x => x.NameNato == transferstring);
						Uxo.NameNato = transferstring;
						_unitOfWork.Image.Add(Image);
						_unitOfWork.Save();
						TempData["success"] = "U.X.O. added successfully";
						return RedirectToPage("Index");
					}
				}				
			}
			else
			{
				//Update
				
				//For when anything except the image gets updated
				Image = _unitOfWork.Image.GetFirstOrDefault(u => u.Uxo_ID == Uxo.Uxo_ID);			

				if (files.Count > 0)
				{
					//For when the image gets updated
					var uploads = Path.Combine(webRootPath, @"images\UxoImages" + nameCategoryFolder);
					var extension = Path.GetExtension(files[0].FileName);

					//Delete the old image
					var oldImagePath = Path.Combine(webRootPath, Image.UxoImage.TrimStart('\\'));
					if (System.IO.File.Exists(oldImagePath))
					{
						System.IO.File.Delete(oldImagePath);
					}

					//New image upload
					using (var fileStream = new FileStream(Path.Combine(uploads, files[0].FileName + extension), FileMode.Create))
					{
						files[0].CopyTo(fileStream);
					}
					Image.UxoImage = @"\images\UxoImages\" + nameCategoryFolder + @"\" + files[0].FileName + extension;
				}
				if (Uxo.NameNato == Category.CategoryName)
				{
					ModelState.AddModelError(string.Empty, "The Category and U.X.O. Name cannot be the same");
				}
				if (ModelState.IsValid)
				{
					//Uxo needs a Category for the Insert into the Database
					Uxo.Category = _unitOfWork.Category.GetFirstOrDefault(x => x.Category_ID == Category.Category_ID);
					Uxo.Category_ID = Category.Category_ID;

					//To add an Image, the correct Uxo object, including Uxo_ID is needed. For this, we first need to insert the Uxo into the database.
					//The Uxo_ID is autoincremented by the Database.
					//We store the NameNato value in 'overdracht', so that we can retrieve the right Uxo object from the database after it is inserted.
					string transferstring = Uxo.NameNato.ToString();
					_unitOfWork.Uxo.Update(Uxo);
					_unitOfWork.Save();

					//Now that the Uxo is inserted and the Uxo_ID is created, we can update the Image.
					//update Image
					Uxo = _unitOfWork.Uxo.GetFirstOrDefault(x => x.NameNato == transferstring);
					Image.Uxo = _unitOfWork.Uxo.GetFirstOrDefault(x => x.NameNato == transferstring);
					Uxo.NameNato = transferstring;
					_unitOfWork.Image.Update(Image);				
					_unitOfWork.Save();
				}
				TempData["success"] = "U.X.O. saved successfully";
				return RedirectToPage("Index");
			}
            TempData["error"] = "Something went wrong...";
            return Page();
		}
	}
}


