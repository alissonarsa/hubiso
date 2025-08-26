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
    [Route("materiais")] // A rota base para todo o controller
    public class MaterialsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MaterialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: materiais
        [HttpGet] // Responde a um pedido GET para a rota base: /materiais
        public async Task<IActionResult> Index()
        {
            return View(await _context.Materiais.ToListAsync());
        }

        // GET: materiais/details/5
        [HttpGet("details/{id?}")] // Responde a /materiais/details/5 (o ? torna o id opcional)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materiais
                .FirstOrDefaultAsync(m => m.Id == id);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        // GET: materiais/create
        [HttpGet("create")] // Responde a /materiais/create
        public IActionResult Create()
        {
            return View();
        }

        // POST: materiais/create
        [HttpPost("create")] // Responde a um pedido POST para /materiais/create
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Descricao,UnidadeMedida,PrecoVenda,PesoEmbalagem,Ativo")] Material material)
        {
            if (ModelState.IsValid)
            {
                _context.Add(material);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }

        // GET: materiais/edit/5
        [HttpGet("edit/{id?}")] // Responde a /materiais/edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materiais.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }
            return View(material);
        }

        // POST: materiais/edit/5
        [HttpPost("edit/{id}")] // Responde a um pedido POST para /materiais/edit/5
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Descricao,UnidadeMedida,PrecoVenda,PesoEmbalagem,Ativo")] Material material)
        {
            if (id != material.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(material);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialExists(material.Id))
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
            return View(material);
        }

        // GET: materiais/delete/5
        [HttpGet("delete/{id?}")] // Responde a /materiais/delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materiais
                .FirstOrDefaultAsync(m => m.Id == id);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        // POST: materiais/delete/5
        [HttpPost("delete/{id}")] // Responde a um pedido POST para /materiais/delete/5
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var material = await _context.Materiais.FindAsync(id);
            if (material != null)
            {
                _context.Materiais.Remove(material);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialExists(int id)
        {
            return _context.Materiais.Any(e => e.Id == id);
        }
    }
}