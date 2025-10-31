using hubiso.Data;
using hubiso.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;
using System.Text.Json;

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
        public async Task<IActionResult> Index(string filtroStatus = "ativos") // <== 1. Recebe o filtro
        {
            // Inicia a consulta (sem executar)
            var query = _context.Clientes.AsQueryable();
            // 2. Aplica o filtro conforme o parâmetro
            if (filtroStatus == "ativos")
            {
                query = query.Where(c => c.Ativo == true);
                ViewData["Title"] = "Clientes Ativos"; // Define o título da página
            }
            else if (filtroStatus == "inativos")
            {
                query = query.Where(c => c.Ativo == false);
                ViewData["Title"] = "Clientes Inativos"; // Define o título da página
            }
            else // "todos" ou qualquer outro valor
            {
                // Não aplica filtro de status
                ViewData["Title"] = "Todos os Clientes"; // Define o título da página
            }
            // 3. Envia o filtro atual para a View (para sabermos qual botão destacar)
            ViewData["FiltroAtual"] = filtroStatus;
            // 4. Executa a consulta já filtrada e envia para a View
            return View(await query.ToListAsync());
        }

        // GET: clientes/details/5
        [HttpGet("details/{id?}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var cliente = await _context.Clientes
            .Include(c => c.Solicitantes)
            .Include(c => c.EnderecosObra)
            .Include(c => c.EmailsFaturamento)
            .FirstOrDefaultAsync(m => m.Id == id);
            if (cliente == null) return NotFound();
            return View(cliente);
        }

        // GET: clientes/criar
        [HttpGet("criar")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: clientes/criar
        [HttpPost("criar")]
        [ValidateAntiForgeryToken]
        // CORREÇÃO: Adicionados os novos campos de endereço ao [Bind]
        public async Task<IActionResult> Create([Bind("Id,Cnpj,RazaoSocial,NomeFantasia,InscricaoEstadual,Cep,Logradouro,Numero,Complemento,Bairro,Cidade,Uf,Ativo")] Cliente cliente)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,RazaoSocial,NomeFantasia,InscricaoEstadual,Cep,Logradouro,Numero,Complemento,Bairro,Cidade,Uf,Ativo")] Cliente clienteEditado)
        {
            if (id != clienteEditado.Id) return NotFound();

            // --- INÍCIO DA CORREÇÃO ---
            // Remove a validação do CNPJ ANTES de checar o ModelState.
            // Isso é necessário porque o CNPJ não está no [Bind],
            // o que o faz ser 'null' no 'clienteEditado' e acionar o [Required]
            // no pipeline de validação, antes mesmo de entrarmos neste método.
            ModelState.Remove("Cnpj");
            // --- FIM DA CORREÇÃO ---

            var clienteOriginal = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);

            if (clienteOriginal == null) return NotFound();

            // Agora, este 'if' vai passar (se os outros campos como RazaoSocial estiverem OK)
            if (ModelState.IsValid)
            {
                try
                {
                    clienteOriginal.RazaoSocial = clienteEditado.RazaoSocial;
                    clienteOriginal.NomeFantasia = clienteEditado.NomeFantasia;
                    clienteOriginal.InscricaoEstadual = clienteEditado.InscricaoEstadual;
                    clienteOriginal.Cep = clienteEditado.Cep;
                    clienteOriginal.Logradouro = clienteEditado.Logradouro;
                    clienteOriginal.Numero = clienteEditado.Numero;
                    clienteOriginal.Complemento = clienteEditado.Complemento;
                    clienteOriginal.Bairro = clienteEditado.Bairro;
                    clienteOriginal.Cidade = clienteEditado.Cidade;
                    clienteOriginal.Uf = clienteEditado.Uf;
                    clienteOriginal.Ativo = clienteEditado.Ativo;

                    _context.Update(clienteOriginal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(clienteOriginal.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index)); // SUCESSO!
            }

            // 5. SE O MODELSTATE FOR INVÁLIDO (Ex: Razão Social em branco)

            // Recarregando as listas
            await _context.Entry(clienteOriginal).Collection(c => c.Solicitantes).LoadAsync();
            await _context.Entry(clienteOriginal).Collection(c => c.EnderecosObra).LoadAsync();
            await _context.Entry(clienteOriginal).Collection(c => c.EmailsFaturamento).LoadAsync();

            // Repopulando o 'clienteOriginal' com os dados *inválidos* que o usuário digitou
            clienteOriginal.RazaoSocial = clienteEditado.RazaoSocial;
            clienteOriginal.NomeFantasia = clienteEditado.NomeFantasia;
            // ... (etc.) ...
            clienteOriginal.Ativo = clienteEditado.Ativo;

            // A linha 'ModelState.Remove("Cnpj");' foi removida daqui, 
            // pois agora ela está no topo do método.

            return View(clienteOriginal);
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
            if (cliente != null)
            {
                cliente.Ativo = false;
                _context.Clientes.Update(cliente);
            }
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
                return Ok(data);
            }

            return StatusCode((int)response.StatusCode, new { message = "Erro ao consultar CNPJ na API externa." });
        }

        // --- NOVO MÉTODO PARA CONSULTAR O CEP ---
        [HttpGet("consultar-cep/{cep}")]
        public async Task<IActionResult> ConsultarCep(string cep)
        {
            var apenasNumeros = new string(cep.Where(char.IsDigit).ToArray());
            if (string.IsNullOrWhiteSpace(apenasNumeros) || apenasNumeros.Length != 8)
            {
                return BadRequest(new { message = "CEP deve conter 8 dígitos." });
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://viacep.com.br/ws/{apenasNumeros}/json/");

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                if (responseString.Contains("\"erro\": true"))
                {
                    return NotFound(new { message = "CEP não encontrado." });
                }

                var data = JsonSerializer.Deserialize<CepResponse>(responseString);
                return Ok(data);
            }

            return StatusCode((int)response.StatusCode, new { message = "Erro ao consultar CEP na API externa." });
        }
    }
}