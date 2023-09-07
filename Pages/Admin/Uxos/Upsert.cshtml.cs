using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;

namespace Mined.Pages.Admin.Uxos
{
	[BindProperties]
	public class UpsertModel : PageModel
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IWebHostEnvironment _hostEnvironment;
        public int Id { get; set; }
        public Uxo? Uxo { get; set; }
        public IEnumerable<Uxo> Uxos { get; set; }
        public Image? Image { get; set; }
        public IEnumerable<Image> Images { get; set; }
        public Category? Category { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<SelectListItem>? CategoryList { get; set; }
        public UpsertModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
		{
			_unitOfWork = unitOfWork;
			_hostEnvironment = hostEnvironment;
			Uxo = new();
			Image = new();
			Category = new();
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
			//Check category to find the Category Folder name, which will be used as foldername in the imagepath later.
			Category = _unitOfWork.Category.GetFirstOrDefault(x => x.CategoryName == Category.CategoryName);
			string nameCategoryFolder = Category.CategoryName;

			string webRootPath = _hostEnvironment.WebRootPath;
			var files = HttpContext.Request.Form.Files;

			if (Uxo.Uxo_ID == 0)
			{
				//Add new UXO

				//Check if given UXO name does not already exist:
				Uxos = _unitOfWork.Uxo.GetAll();
				if (Uxos.Any(c => c.NameNato == Uxo.NameNato))
				{
					ModelState.AddModelError(string.Empty, "It appears this UXO already exists in the database");
					FillDropDownList();
				}
				else
				{
                    //Add new image
                    var uploads = Path.Combine(webRootPath, @"images/UxoImages/" + nameCategoryFolder);
					var extension = Path.GetExtension(files[0].FileName);
					using (var fileStream = new FileStream(Path.Combine(uploads, files[0].FileName), FileMode.Create))
					{
						files[0].CopyTo(fileStream);
					}
					Image.UxoImage = @"~/images/UxoImages/" + nameCategoryFolder + @"/" + files[0].FileName;

					//Check if Uxo name is not the same as Category name.
					if (Uxo.NameNato == Category.CategoryName)
					{
						ModelState.AddModelError(string.Empty, "Category and U.X.O. Name cannot be the same");
					}
					if (ModelState.IsValid)
					{
						//Uxo needs a Category object so that it can be inserted into the database:
						Uxo.Category = _unitOfWork.Category.GetFirstOrDefault(x => x.Category_ID == Category.Category_ID);

						//Because of the Parent-Child relation in the database,
						//the UXO data must be inserted into the database before the image can be inserted.
						//Once inserted, the auto-incremented UXO_Id will be available, which is necessary to add the Image data.
						//However, to add the Image data and link it to the UXO, the UXO must be identifiable.
						//For this, we will use a transferstring, because after the save, the UXO data will not be available.
						string transferstringNameNato = Uxo.NameNato.ToString();
						_unitOfWork.Uxo.Add(Uxo);
						_unitOfWork.Save();

                        //Now that the Uxo is inserted and the Uxo_ID is created, we can add the Images using the U.X.O. Name.
                        
						//Add Image
                        Uxo = _unitOfWork.Uxo.GetFirstOrDefault(x => x.NameNato == transferstringNameNato);
						Image.Uxo = _unitOfWork.Uxo.GetFirstOrDefault(x => x.NameNato == transferstringNameNato);
						Uxo.NameNato = transferstringNameNato;
						_unitOfWork.Image.Add(Image);
						_unitOfWork.Save();
						TempData["success"] = "U.X.O. added successfully";
						return RedirectToPage("Index");
					}
				}				
			}
			else
			{
				//Update existing UXO.
				
				//For when anything except the image gets updated:
				Image = _unitOfWork.Image.GetFirstOrDefault(u => u.Uxo_ID == Uxo.Uxo_ID);			
				
				//If the image does get updated
				if (files.Count > 0)
				{
					//For when the image gets updated
					var uploads = Path.Combine(webRootPath, @"images/UxoImages/" + nameCategoryFolder);
					var extension = Path.GetExtension(files[0].FileName);

					//Delete the old image
					var oldImagePath = Path.Combine(webRootPath, Image.UxoImage.TrimStart('\\'));
					if (System.IO.File.Exists(oldImagePath))
					{
						System.IO.File.Delete(oldImagePath);
					}

					//New image upload
					using (var fileStream = new FileStream(Path.Combine(uploads, files[0].FileName), FileMode.Create))
					{
						files[0].CopyTo(fileStream);
					}
					Image.UxoImage = @"~/images/UxoImages/" + nameCategoryFolder + @"/" + files[0].FileName;
				}
				//Verify if the UXO name is not the same as the Category.
				if (Uxo.NameNato == Category.CategoryName)
				{
					ModelState.AddModelError(string.Empty, "The Category and U.X.O. Name cannot be the same");
				}
				if (ModelState.IsValid)
				{
					//Uxo needs a Category for the Update into the Database
					Uxo.Category = _unitOfWork.Category.GetFirstOrDefault(x => x.Category_ID == Category.Category_ID);
					Uxo.Category_ID = Category.Category_ID;
					_unitOfWork.Uxo.Update(Uxo);
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


