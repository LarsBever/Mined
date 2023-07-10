using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Mined.DataAccess.Data;
using Mined.Models;

namespace Mined.Pages.Admin.Uxos
{
    public class IndexModel : PageModel
    {
        private readonly MinedDbContext _db;
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Image> Images { get; set; }
        public IEnumerable<Payload> Payloads { get; set; }
        public IEnumerable<UxoPayload> UxoPayloads { get; set; }
        public IEnumerable<Uxo> Uxos { get; set; }
        public IndexModel(MinedDbContext db)
        {
            _db = db;
        }
        public void OnGet()
        {
            Categories = _db.Categories;
            Images = _db.Images;
            Payloads = _db.Payloads;
            UxoPayloads = _db.UxoPayloads;
            Uxos = _db.Uxos;
        }
    }
}
