using Microsoft.AspNetCore.Mvc.RazorPages;
using Mined.DataAccess.Data;
using Mined.DataAccess.Repository;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;

namespace Mined.Pages.Admin.Uxos
{
    public class IndexModel : PageModel
    {
		private readonly IUnitOfWork _unitOfWork;
		public IEnumerable<Image> Images { get; set; }
		public IEnumerable<Uxo> Uxos { get; set; }
		public IEnumerable<Category> Categories { get; set; }
		public IndexModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

        public void OnGet()
        {
            Categories = _unitOfWork.Category.GetAll();
            Images = _unitOfWork.Image.GetAll();
			Uxos = _unitOfWork.Uxo.GetAll();
        }
    }
}
