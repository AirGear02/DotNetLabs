using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab4.Models;

namespace Lab4.Controllers
{
    public class OptionsController : Controller
    {
        private readonly PhotoStudioContext _context;

        public OptionsController(PhotoStudioContext context)
        {
            _context = context;
        }

        // GET: Options
        [Route("/options")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Options.ToListAsync());
        }

        // GET: Options/Details/5
        [Route("/options/deatils/{id?}")]
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var option = await _context.Options
                .FirstOrDefaultAsync(m => m.Id == id);
            if (option == null)
            {
                return NotFound();
            }

            return View(option);
        }

        // GET: Options/Create
        [Route("/options/create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Options/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/options/create")]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Price")] Option option)
        {
            if (ModelState.IsValid)
            {
                option.Id = Guid.NewGuid();
                _context.Add(option);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(option);
        }

        // GET: Options/Edit/5
        [Route("options/edit/{id?}")]
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var option = await _context.Options.FindAsync(id);
            if (option == null)
            {
                return NotFound();
            }
            return View(option);
        }

        // POST: Options/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("options/edit/{id?}")]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Title,Description,Price")] Option option)
        {
            if (id != option.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(option);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OptionExists(option.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(option);
        }

        // GET: Options/Delete/5
        [Route("/options/delete/{id?}")]
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var option = await _context.Options
                .FirstOrDefaultAsync(m => m.Id == id);
            if (option == null)
            {
                return NotFound();
            }

            return View(option);
        }

        // POST: Options/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("/options/delete/{id?}")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var option = await _context.Options.FindAsync(id);
            _context.Options.Remove(option);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OptionExists(Guid id)
        {
            return _context.Options.Any(e => e.Id == id);
        }
    }
}
