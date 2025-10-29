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
    public class EnderecoObrasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnderecoObrasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EnderecoObras
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.EnderecosObra.Include(e => e.Cliente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: EnderecoObras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enderecoObra = await _context.EnderecosObra
                .Include(e => e.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enderecoObra == null)
            {
                return NotFound();
            }

            return View(enderecoObra);
        }

        // GET: EnderecoObras/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Cnpj");
            return View();
        }

        // POST: EnderecoObras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DescricaoLocal,Cep,Logradouro,Numero,Complemento,Bairro,Cidade,Uf,Ativo,ClienteId")] EnderecoObra enderecoObra)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enderecoObra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Cnpj", enderecoObra.ClienteId);
            return View(enderecoObra);
        }

        // GET: EnderecoObras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enderecoObra = await _context.EnderecosObra.FindAsync(id);
            if (enderecoObra == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Cnpj", enderecoObra.ClienteId);
            return View(enderecoObra);
        }

        // POST: EnderecoObras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DescricaoLocal,Cep,Logradouro,Numero,Complemento,Bairro,Cidade,Uf,Ativo,ClienteId")] EnderecoObra enderecoObra)
        {
            if (id != enderecoObra.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enderecoObra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnderecoObraExists(enderecoObra.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "Cnpj", enderecoObra.ClienteId);
            return View(enderecoObra);
        }

        // GET: EnderecoObras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enderecoObra = await _context.EnderecosObra
                .Include(e => e.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enderecoObra == null)
            {
                return NotFound();
            }

            return View(enderecoObra);
        }

        // POST: EnderecoObras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enderecoObra = await _context.EnderecosObra.FindAsync(id);
            if (enderecoObra != null)
            {
                _context.EnderecosObra.Remove(enderecoObra);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EnderecoObraExists(int id)
        {
            return _context.EnderecosObra.Any(e => e.Id == id);
        }
    }
}
