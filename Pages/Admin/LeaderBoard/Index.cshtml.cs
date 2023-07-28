using Microsoft.AspNetCore.Mvc.RazorPages;
using Mined.DataAccess.Data;
using Mined.DataAccess.Repository;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;

namespace Mined.Pages.Admin.LeaderBoard
{
    public class IndexModel : PageModel
    {
		private readonly IUnitOfWork _unitOfWork;
		public IEnumerable<Score> Scores { get; set; }
		public IndexModel(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public void OnGet()
        {
            Scores = _unitOfWork.Score.GetAll();

		}
    }
}
