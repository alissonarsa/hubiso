using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using hubiso.Data;
using hubiso.Models;

namespace hubiso.Controllers
{
    public class SolicitantesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SolicitantesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Solicitantes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Solicitantes.Include(s => s.Cliente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Solicitantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitante = await _context.Solicitantes
                .Include(s => s.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (solicitante == null)
            {
                return NotFound();
            }

            return View(solicitante);
        }

        // GET: Solicitantes/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Cnpj");
            return View();
        }

        // POST: Solicitantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Email,Telefone,Departamento,Ativo,ClienteId")] Solicitante solicitante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(solicitante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Cnpj", solicitante.ClienteId);
            return View(solicitante);
        }

        // GET: Solicitantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitante = await _context.Solicitantes.FindAsync(id);
            if (solicitante == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Cnpj", solicitante.ClienteId);
            return View(solicitante);
        }

        // POST: Solicitantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Telefone,Departamento,Ativo,ClienteId")] Solicitante solicitante)
        {
            if (id != solicitante.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(solicitante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SolicitanteExists(solicitante.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Cnpj", solicitante.ClienteId);
            return View(solicitante);
        }

        // GET: Solicitantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitante = await _context.Solicitantes
                .Include(s => s.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (solicitante == null)
            {
                return NotFound();
            }

            return View(solicitante);
        }

        // POST: Solicitantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var solicitante = await _context.Solicitantes.FindAsync(id);
            if (solicitante != null)
            {
                _context.Solicitantes.Remove(solicitante);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SolicitanteExists(int id)
        {
            return _context.Solicitantes.Any(e => e.Id == id);
        }
    }
}
