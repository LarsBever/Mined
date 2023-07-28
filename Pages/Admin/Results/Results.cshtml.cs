using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Mined.Data;
using Mined.Models;

namespace QuizApp.Pages
{
    [Authorize(Roles = "RegularUser, Administrator")]
    public class ResultsModel : PageModel
    {
        private readonly QuizApp.Data.QuizAppContext _context;
        private readonly UserManager<IdentityUser> userManager;

        public ResultsModel(QuizApp.Data.QuizAppContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }
        public IList<Uxo> Uxos { get; set; } = default!;
        public IList<Result> Results { get; set; } = default!;


        public async Task OnGetAsync()
        {
            if (_context.Country != null)
            {
                Country = await _context.Country.ToListAsync();
            }

            if (_context.Result != null)
            {
                Results = await _context.Result.ToListAsync();
            }
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var uId = userManager.GetUserId(User);
            var userResults = _context.Result.Where(c => c.user_id == uId).ToList();
            if (userResults != null)
            {
                foreach (var answer in userResults)
                {
                    _context.Result.Remove(answer);
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("./Quiz");
        }

    }
}
