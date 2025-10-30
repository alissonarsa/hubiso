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
    [Route("emailfaturamentos")] // <-- Rota base adicionada
    public class EmailFaturamentosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmailFaturamentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: emailfaturamentos
        [HttpGet] // <-- Adicionado
        public async Task<IActionResult> Index()
        {
            // Inclui os dados do Cliente associado para mostrar na lista (opcional, mas útil)
            var applicationDbContext = _context.EmailsFaturamento.Include(e => e.Cliente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: emailfaturamentos/details/5
        [HttpGet("details/{id?}")] // <-- Adicionado
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var emailFaturamento = await _context.EmailsFaturamento
                .Include(e => e.Cliente) // Inclui o Cliente associado
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emailFaturamento == null) return NotFound();
            return View(emailFaturamento);
        }

        // GET: emailfaturamentos/create ou /emailfaturamentos/create?clienteId=123
        [HttpGet("create")] // <-- Adicionado
        public IActionResult Create(int? clienteId) // Recebe o clienteId opcional da URL
        {
            // Cria a lista suspensa de Clientes
            // Se um clienteId foi passado, pré-seleciona esse cliente
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "RazaoSocial", clienteId);
            return View();
        }

        // POST: emailfaturamentos/create
        [HttpPost("create")] // <-- Adicionado
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Ativo,ClienteId")] EmailFaturamento emailFaturamento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(emailFaturamento);
                await _context.SaveChangesAsync();
                // Redireciona de volta para os detalhes do Cliente a que pertence
                return RedirectToAction("Details", "Clientes", new { id = emailFaturamento.ClienteId });
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "RazaoSocial", emailFaturamento.ClienteId);
            return View(emailFaturamento);
        }

        // GET: emailfaturamentos/edit/5
        [HttpGet("edit/{id?}")] // <-- Adicionado
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var emailFaturamento = await _context.EmailsFaturamento.FindAsync(id);
            if (emailFaturamento == null) return NotFound();
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "RazaoSocial", emailFaturamento.ClienteId);
            return View(emailFaturamento);
        }

        // POST: emailfaturamentos/edit/5
        [HttpPost("edit/{id}")] // <-- Adicionado
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Ativo,ClienteId")] EmailFaturamento emailFaturamento)
        {
            if (id != emailFaturamento.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(emailFaturamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmailFaturamentoExists(emailFaturamento.Id)) return NotFound();
                    else throw;
                }
                // Redireciona de volta para os detalhes do Cliente a que pertence
                return RedirectToAction("Details", "Clientes", new { id = emailFaturamento.ClienteId });
            }
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "RazaoSocial", emailFaturamento.ClienteId);
            return View(emailFaturamento);
        }

        // GET: emailfaturamentos/delete/5
        [HttpGet("delete/{id?}")] // <-- Adicionado
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var emailFaturamento = await _context.EmailsFaturamento
                .Include(e => e.Cliente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (emailFaturamento == null) return NotFound();
            return View(emailFaturamento);
        }

        // POST: emailfaturamentos/delete/5
        [HttpPost("delete/{id}")] // <-- Adicionado
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var emailFaturamento = await _context.EmailsFaturamento.FindAsync(id);
            int? clienteId = emailFaturamento?.ClienteId; // Guarda o Id antes de apagar
            if (emailFaturamento != null)
            {
                _context.EmailsFaturamento.Remove(emailFaturamento);
                await _context.SaveChangesAsync();
            }
            // Redireciona de volta para os detalhes do Cliente a que pertencia
            if (clienteId.HasValue)
            {
                return RedirectToAction("Details", "Clientes", new { id = clienteId.Value });
            }
            return RedirectToAction(nameof(Index)); // Fallback
        }

        private bool EmailFaturamentoExists(int id)
        {
            return _context.EmailsFaturamento.Any(e => e.Id == id);
        }
    }
}