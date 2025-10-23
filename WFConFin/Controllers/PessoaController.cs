using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PessoaController : Controller
    {
        private readonly WFConFinDbContext _context;
        public PessoaController(WFConFinDbContext context)
        {
            _context = context;
        }

        // GET: PessoaController
        [HttpGet]
        public async Task<IActionResult> GetPessoas()
        {
            try
            {
                var result = await _context.Pessoa.ToListAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na listagem de pessoas. Exeção: {e.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostPessoa([FromBody] Pessoa pessoa)
        {
            try
            {
                await _context.Pessoa.AddAsync(pessoa);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, pessoa incluída.");
                }
                else
                {
                    return BadRequest("Erro, pessoa não incluída.");
                }

            }
            catch (Exception e)
            {
                return BadRequest($"Erro na inclusão de pessoa. Exeção: {e.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutPessoa([FromBody] Pessoa pessoa)
        {
            try
            {
                _context.Pessoa.Update(pessoa);
                var valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, pessoa alterada.");
                }
                else
                {
                    return BadRequest("Erro, pessoa não alterada.");
                }

            }
            catch (Exception e)
            {
                return BadRequest($"Erro na alteração de pessoas. Exeção: {e.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePessoa([FromRoute] Guid id)
        {
            try
            {
                Pessoa pessoa = await _context.Pessoa.FindAsync(id);
                if (pessoa != null)
                {
                    _context.Pessoa.Remove(pessoa);
                    var valor = await _context.SaveChangesAsync();
                    if (valor == 1)
                    {
                        return Ok("Sucesso, pessoa excluída.");
                    }
                    else
                    {
                        return BadRequest("Erro, pessoa não excluída.");
                    }
                }
                else
                {
                    return NotFound($"Erro, pessoa não existe.");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na inclusão de pessoas. Exeção: {e.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPessoa([FromRoute] Guid id)
        {
            try
            {
                Pessoa pessoa = await _context.Pessoa.FindAsync(id);

                if (pessoa != null)
                {
                    return Ok(pessoa);
                }
                else
                {
                    return NotFound("Erro, pessoa não existe.");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na consulta de pessoa. Exceção: {e.Message}");
            }
        }

        [HttpGet("Pesquisa")]
        public async Task<IActionResult> GetPessoaPesquisa([FromQuery] string valor)
        {
            try
            {
                //Query Criteria
                var lista = from z in await _context.Pessoa.ToListAsync()
                            where z.Nome.ToUpper().Contains(valor.ToUpper())
                            || z.Telefone.ToUpper().Contains(valor.ToUpper())
                            || z.Email.ToUpper().Contains(valor.ToUpper())
                            select z;

                return Ok(lista);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro, pequisa de pessoa. Exceção: {e.Message}");
            }
        }

        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetPessoaPaginacao([FromQuery] string valor, int skip, int take, bool ordemDesc)
        {
            try
            {
                //Query Criteria
                //No curso este metodo não ficou assíncrono
                var lista = from z in await _context.Pessoa.ToListAsync()
                            where z.Nome.ToUpper().Contains(valor.ToUpper())
                            || z.Telefone.ToUpper().Contains(valor.ToUpper())
                            || z.Email.ToUpper().Contains(valor.ToUpper())
                            select z;

                //Tipo de ordenação dos dados!
                if (ordemDesc)
                {
                    lista = from z in lista
                            orderby z.Nome descending
                            select z;
                }
                else
                {
                    lista = from z in lista
                            orderby z.Nome ascending
                            select z;
                }

                int qtde = lista.Count();

                lista = lista
                        .Skip(skip)
                        .Take(take)
                        .ToList();

                var paginacaoResponse = new PaginacaoResponse<Pessoa>(lista, qtde, skip, take);

                return Ok(paginacaoResponse);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro, pesquisa de pessoa. Exceção: {e.Message}");
            }
        }

    }
}
