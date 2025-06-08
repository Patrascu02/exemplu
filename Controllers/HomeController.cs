using exemplu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace exemplu.Controllers
{
    public class HomeController : Controller
    {
        

        private readonly Context _context;

        public HomeController(Context context)
        {
            _context = context;
        }




        public async Task<IActionResult> Index()
        {
            var context = _context.CONCURENTI.Include(c => c.CONCURS);
            return View(await context.ToListAsync());
        }

        
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cONCURENT = await _context.CONCURENTI
                .Include(c => c.CONCURS)
                .FirstOrDefaultAsync(m => m.Id == id);


            if (cONCURENT == null)
            {
                return NotFound();
            }

            return View(cONCURENT);
        }

        
        public IActionResult Create()
        {
            ViewData["CONCURSId"] = new SelectList(_context.CONCURSURI, "Id", "Categorie");
            return View();


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nume,Prenume,Tara,Varsta,CONCURSId")] CONCURENT concurent)
        {
           
            concurent.DataNasterii = DateTime.Now;

            if (ModelState.IsValid)
            {
                // Obtine concursul asociat
                var concurs = await _context.CONCURSURI
                    .Include(c => c.CONCURENTI)
                    .FirstOrDefaultAsync(c => c.Id == concurent.CONCURSId);

                if (concurs == null)
                {
                    ModelState.AddModelError("", "Concursul selectat nu exista.");
                    ViewData["CONCURSId"] = new SelectList(_context.CONCURSURI, "Id", "Categorie", concurent.CONCURSId);
                    return View(concurent);
                }

                
                if (concurs.CONCURENTI.Count() >= concurs.nr_max_participanti)
                {
                    ModelState.AddModelError("", "Nr maxim de participanti a fost atins pentru acest concurs.");
                    ViewData["CONCURSId"] = new SelectList(_context.CONCURSURI, "Id", "Categorie", concurent.CONCURSId);
                    return View(concurent);
                }

                
                if (concurs.restrictie_varsta == true && concurent.Varsta < 18)
                {
                    ModelState.AddModelError("", "Concurentii minori nu pot participa la acest concurs.");
                    ViewData["CONCURSId"] = new SelectList(_context.CONCURSURI, "Id", "Categorie", concurent.CONCURSId);
                    return View(concurent);
                }

                // Salveaz concurentul
                _context.Add(concurent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CONCURSId"] = new SelectList(_context.CONCURSURI, "Id", "Categorie", concurent.CONCURSId);
            return View(concurent);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cONCURENT = await _context.CONCURENTI.FindAsync(id);
            if (cONCURENT == null)
            {
                return NotFound();
            }
            ViewData["CONCURSId"] = new SelectList(_context.CONCURSURI, "Id", "Categorie", cONCURENT.CONCURSId);
            return View(cONCURENT);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nume,Prenume,DataNasterii,Tara,Varsta,CONCURSId")] CONCURENT cONCURENT)
        {
            if (id != cONCURENT.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cONCURENT);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CONCURENTExists(cONCURENT.Id))
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
            ViewData["CONCURSId"] = new SelectList(_context.CONCURSURI, "Id", "Categorie", cONCURENT.CONCURSId);
            return View(cONCURENT);
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cONCURENT = await _context.CONCURENTI
                .Include(c => c.CONCURS)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cONCURENT == null)
            {
                return NotFound();
            }

            return View(cONCURENT);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cONCURENT = await _context.CONCURENTI.FindAsync(id);
            if (cONCURENT != null)
            {
                _context.CONCURENTI.Remove(cONCURENT);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CONCURENTExists(int id)
        {
            return _context.CONCURENTI.Any(e => e.Id == id);
        }


        public IActionResult Search(string nume, DateTime? data, string categorie)
        {
            var concursuri = _context.CONCURSURI.AsQueryable();

            if (!string.IsNullOrEmpty(nume))
            {
                concursuri = concursuri.Where(c => c.Nume.Contains(nume));
            }

            if (data.HasValue)
            {
                concursuri = concursuri.Where(c => c.Data == data.Value.Date);
            }

            if (!string.IsNullOrEmpty(categorie))
            {
                concursuri = concursuri.Where(c => c.Categorie.Contains(categorie));
            }

            return View(concursuri.ToList());
        }
    }
}
