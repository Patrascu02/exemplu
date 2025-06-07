using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using exemplu.Models;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using System.Diagnostics;

namespace exemplu.Controllers
{
    public class CONCURENTsController : Controller
    {
        private readonly Context _context;

        public CONCURENTsController(Context context)
        {
            _context = context;
        }

        // GET: CONCURENTs
        public async Task<IActionResult> Index()
        {
            var context = _context.CONCURENTI.Include(c => c.CONCURS);
            return View(await context.ToListAsync());
        }

        // GET: CONCURENTs/Details/5
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

        // GET: CONCURENTs/Create
        public IActionResult Create()
        {
            ViewData["CONCURSId"] = new SelectList(_context.CONCURSURI, "Id", "Categorie");
            return View();


        }

        // POST: CONCURENTs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nume,Prenume,DataNasterii,Tara,Varsta,CONCURSId")] CONCURENT concurent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(concurent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }



            // 1. Obține concursul asociat
            var concurs = await _context.CONCURSURI
                .Include(c => c.CONCURENTI)
                .FirstOrDefaultAsync(c => c.Id == concurent.CONCURSId);

            if (concurs == null)
            {
                ModelState.AddModelError("", "Concursul selectat nu există.");
                return View(concurent);
            }


            

            System.Diagnostics.Debug.WriteLine("=============================================================================");
            // 2. Verifică dacă s-a atins numărul maxim de participanți
            if (concurs.CONCURENTI.Count() >= concurs.nr_max_participanti)
            {
                ModelState.AddModelError("", "Numărul maxim de participanți a fost atins pentru acest concurs.");
                return View(concurent);
            }


            Debug.WriteLine($"Varsta primita: {concurent.Varsta}");


            // 3. Verifică dacă există restricție de vârstă și concurentul este minor (< 18)
            if (concurs.restrictie_varsta == true && concurent.Varsta < 18)
            {
                ModelState.AddModelError("", "Concurenții minori nu pot participa la acest concurs.");
                return View(concurent);
            }

            // 4. Salvează concurentul
            _context.Add(concurent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));


        }

        // GET: CONCURENTs/Edit/5
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

        // POST: CONCURENTs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: CONCURENTs/Delete/5
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

        // POST: CONCURENTs/Delete/5
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
    }
}
