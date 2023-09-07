using Microsoft.AspNetCore.Mvc.RazorPages;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;

namespace Mined.Pages.Admin.UxoCategories
{
    public class IndexModel : PageModel
    {
		private readonly IUnitOfWork _unitOfWork;
		public IEnumerable<Category> Categories { get; set; }
		public IndexModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
        public void OnGet()
        {
            Categories = _unitOfWork.Category.GetAll();
        }
    }
}
