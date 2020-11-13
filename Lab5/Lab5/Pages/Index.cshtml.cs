using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Lab5.Models;

namespace Lab5.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public PhotoStudioContext Context { get; }

        public IndexModel(ILogger<IndexModel> logger, PhotoStudioContext context)
        {
            _logger = logger;
            Context = context;
        }

        public void OnGet()
        {

        }
    }
}
