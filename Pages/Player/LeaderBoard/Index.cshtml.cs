using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Construction;
using Mined.DataAccess.Data;
using Mined.DataAccess.Repository;
using Mined.DataAccess.Repository.IRepository;
using Mined.Models;
using Mined.Utility;
using static System.Formats.Asn1.AsnWriter;

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
		}

		public async Task<IActionResult> OnPost()
		{
			HttpContext.Session.SetInt32(SD.SessionPlayerScore, 0);
			HttpContext.Session.SetInt32(SD.SessionQuestionNr, 0);
			HttpContext.Session.SetInt32(SD.SessionnumberOfMistakes, 0);
			return RedirectToPage("/Player/Game/Game");
		}
	}
}
