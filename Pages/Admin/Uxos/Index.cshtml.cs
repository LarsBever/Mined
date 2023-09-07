using Microsoft.AspNetCore.Mvc.RazorPages;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;

namespace Mined.Pages.Admin.Uxos
{
	public class IndexModel : PageModel
	{
		private readonly IUnitOfWork _unitOfWork;
		public IEnumerable<Image> Images { get; set; }
		public IEnumerable<Uxo> Uxos { get; set; }
		public IndexModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public void OnGet()
		{
			Images = _unitOfWork.Image.GetAll();
			Uxos = _unitOfWork.Uxo.GetAll();
		}
	}
}
