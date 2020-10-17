using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab3.Models;

namespace Lab3.Pages
{
    public class OptionsModel : PageModel
    {
        private readonly Lab3.PhotoStudioContext _context;

        public OptionsModel(PhotoStudioContext context)
        {
            _context = context;
        }

        public IList<Option> Option { get;set; }

        public async Task OnGetAsync()
        {
            Option = await _context.Options.ToListAsync();
        }
    }
}
