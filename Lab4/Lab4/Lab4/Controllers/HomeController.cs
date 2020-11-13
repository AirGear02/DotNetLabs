using System.Dynamic;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Lab4.Models;

namespace Lab4.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PhotoStudioContext _context;

        public HomeController(ILogger<HomeController> logger, PhotoStudioContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Route("/")]
        public IActionResult Index()
        {
            Dictionary<ClientAndOrderKey, IEnumerable<ClientAndOrder>> query = _context.Orders
           .Join(_context.Clients, orders => orders.ClientId, clients => clients.Id,
           (o, c) => new
           {
               OptionId = o.OptionId,
               Name = c.Name,
               Surname = c.Surname,
               Quantity = o.Quantity,
               DateStart = o.DateStart,
               DateFinish = o.DateFinish,
               Id = c.Id
           })

           .Join(_context.Options, c => c.OptionId, o => o.Id, (c, o) => new
           {
               Option = o.Title,
               Name = c.Name,
               Surname = c.Surname,
               Quantity = c.Quantity,
               DateStart = c.DateStart,
               DateFinish = c.DateFinish,
               Price = c.Quantity * o.Price,
               Id = c.Id
           })
           .ToList()
           .GroupBy(e => new { e.Id, e.Name, e.Surname })
           .Select(e => new
           {
               Name = e.Key.Name,
               Surname = e.Key.Surname,
               Count = e.Count(),
               Id = e.Key.Id,
               Options = e.Select(e => new ClientAndOrder { DateStart = e.DateStart, DateFinish = e.DateFinish, Option = e.Option, 
                   Quantity = e.Quantity, Price = e.Price })
           })
           .ToDictionary(e => new ClientAndOrderKey { Name = e.Name, Surname = e.Surname, Id = e.Id, Count = e.Count }, e => e.Options);
          
           
            foreach(var client in query)
            {
                _logger.LogInformation($"{client.Value.Count()} {client.Key.Name}");
            }
           
           


            return View(query);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
