using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Construction;
using Mined.DataAccess.Data;
using Mined.DataAccess.Repository;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;

namespace Mined.Pages.Player.LeaderBoard
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
			
			foreach (Score score in Scores)
			{
				//eerst sorteren van hoog naar laag?

				////recursieve functie?
				//if(score is van hoogste 10)
				//{
				//	toevoegen aan lijst
				//}
			}

		}
    }
}
