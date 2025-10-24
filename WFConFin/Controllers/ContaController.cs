using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContaController : Controller
    {
        private readonly WFConFinDbContext _context;
        public ContaController(WFConFinDbContext context)
        {
            _context = context;
        }

        // GET: ContaController
        [HttpGet]
        public async Task<IActionResult> GetContas()
        {
            try
            {
                var result = await _context.Conta.ToListAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na listagem de contas. Exceção: {e.Message}");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PostConta([FromBody] Conta conta)
        {
            try
            {
                await _context.Conta.AddAsync(conta);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, conta incluída.");
                }
                else
                {
                    return BadRequest("Erro, conta não incluída.");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na inclusão de conta. Exceção: {e.Message}");
            }
        }

        [HttpPut]
        [Authorize(Roles = "Gerente,Empregado")]
        public async Task<IActionResult> PutConta([FromBody] Conta conta)
        {
            try
            {
                _context.Conta.Update(conta);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, conta alterada.");
                }
                else
                {
                    return BadRequest("Erro, conta não alterada.");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na alteração de conta. Exceção: {e.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> DeleteConta([FromRoute] Guid id)
        {
            try
            {
                Conta conta = await _context.Conta.FindAsync(id);
                if (conta != null)
                {
                    _context.Conta.Remove(conta);
                    var valor = await _context.SaveChangesAsync();

                    if (valor == 1)
                    {
                        return Ok("Sucesso, conta excluída.");
                    }
                    else
                    {
                        return BadRequest("Erro, conta não excluída.");
                    }
                }
                else
                {
                    return NotFound($"Erro, conta não existe.");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na exclusão de conta. Exceção: {e.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetConta([FromRoute] Guid id)
        {
            try
            {
                Conta conta = await _context.Conta.FindAsync(id);
                if (conta != null)
                {
                    return Ok(conta);
                }
                else
                {
                    return NotFound("Erro, conta não existe.");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na consulta de conta. Exceção: {e.Message}");
            }
        }

        [HttpGet("Pesquisa")]
        public async Task<IActionResult> GetContaPesquisa([FromQuery] string valor)
        {
            try
            {
                var lista = from q in await _context.Conta.Include(q => q.Pessoa).ToListAsync()
                            where q.Descricao.ToUpper().Contains(valor.ToUpper())
                            || q.Pessoa.Nome.ToUpper().Contains(valor.ToUpper())
                            select q;

                return Ok(lista);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na pesquisa de conta. Exceção: {e.Message}");
            }
        }

        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetContaPaginacao([FromQuery] string valor, int skip, int take, bool ordemDesc)
        {
            try
            {
                //Query Criteria
                //No curso este metodo não ficou assíncrono
                var lista = from q in await _context.Conta.Include(q => q.Pessoa).ToListAsync()
                            where q.Descricao.ToUpper().Contains(valor.ToUpper())
                            || q.Pessoa.Nome.ToUpper().Contains(valor.ToUpper())
                            select q;

                //Tipo de ordenação dos dados!
                if (ordemDesc)
                {
                    lista = from q in lista
                            orderby q.Descricao descending
                            select q;
                }
                else
                {
                    lista = from q in lista
                            orderby q.Descricao ascending
                            select q;
                }

                int qtde = lista.Count();

                lista = lista
                        .Skip(skip)
                        .Take(take)
                        .ToList();

                var paginacaoResponse = new PaginacaoResponse<Conta>(lista, qtde, skip, take);

                return Ok(paginacaoResponse);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro, pesquisa de conta. Exceção: {e.Message}");
            }
        }

        [HttpGet("Pessoa/{pessoaId}")]
        public async Task<IActionResult> GetContasPessoa([FromRoute] Guid pessoaId)
        {
            try
            {
                var lista = from q in await _context.Conta.Include(q => q.Pessoa).ToListAsync()
                            where q.PessoaId == pessoaId
                            select q;


                return Ok(lista);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na pesquisa de conta por pessoa. Exceção: {e.Message}");
            }
        }
    }
}
