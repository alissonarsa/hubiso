using hubiso.Data;
using hubiso.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace hubiso.Controllers
{
    [Route("clientes")]
    public class ClientesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public ClientesController(ApplicationDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // GET: clientes
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.ToListAsync());
        }

        // GET: clientes/details/5
        [HttpGet("details/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _context.Clientes.FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        // GET: clientes/create
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: clientes/create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Cnpj,RazaoSocial,NomeFantasia,InscricaoEstadual,Ativo")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: clientes/edit/5
        [HttpGet("edit/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        // POST: clientes/edit/5
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cnpj,RazaoSocial,NomeFantasia,InscricaoEstadual,Ativo")] Cliente cliente)
        {
            if (id != cliente.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }

        // GET: clientes/delete/5
        [HttpGet("delete/{id?}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _context.Clientes.FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        // POST: clientes/delete/5
        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null) _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.Id == id);
        }

        [HttpGet("consultar-cnpj/{cnpj}")]
        public async Task<IActionResult> ConsultarCnpj(string cnpj)
        {
            var apenasNumeros = new string(cnpj.Where(char.IsDigit).ToArray());
            if (string.IsNullOrWhiteSpace(apenasNumeros) || apenasNumeros.Length != 14)
            {
                return BadRequest(new { message = "CNPJ deve conter 14 dígitos." });
            }

            var client = _httpClientFactory.CreateClient("BrasilApi");

            var response = await client.GetAsync($"api/cnpj/v2/{apenasNumeros}");

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                response = await client.GetAsync($"api/cnpj/v1/{apenasNumeros}");
            }

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<CnpjResponse>();
                // Usar Ok() garante que as configurações globais de JSON (camelCase) sejam aplicadas
                return Ok(data);
            }

            return StatusCode((int)response.StatusCode, new { message = "Erro ao consultar CNPJ na API externa." });
        }
    }
}