using System;
using System.Collections;
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
    public class CidadeController : Controller
    {
        private readonly WFConFinDbContext _context;

        public CidadeController(WFConFinDbContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetCidades()
        {
            try
            {
                // Se fosse adicionar o campo estado, a variavél deveria reseber a consulta abaixo!
                //var result = _context.Cidade.Include(x => x.Estado).ToList();
                //No curso este metodo não ficou assíncrono
                var result = await _context.Cidade.ToListAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na listagem das cidades. Exceção: {e.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostCidade([FromBody] Cidade cidade)
        {
            try
            {
                await _context.Cidade.AddAsync(cidade);
                int valor = await _context.SaveChangesAsync();

                if (valor == 1)
                {
                    return Ok("Sucesso, cidade incluída.");
                }
                else
                {
                    return BadRequest("Erro, cidade não incluída.");
                }

            }
            catch (Exception e)
            {
                return BadRequest($"Erro na inclusão da cidade. Exceção: {e.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutCidade([FromBody] Cidade cidade)
        {
            try
            {
                _context.Cidade.Update(cidade);
                int valor = await _context.SaveChangesAsync();

                if (valor == 1)
                {
                    return Ok("Sucesso, cidade alterda.");
                }
                else
                {
                    return BadRequest("Erro, cidade não alterada.");
                }

            }
            catch (Exception e)
            {
                return BadRequest($"Erro na alteração da cidade. Exceção: {e.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCidade([FromRoute] Guid id)
        {
            try
            {
                Cidade cidade = await _context.Cidade.FindAsync(id);

                if (cidade != null)
                {
                    _context.Cidade.Remove(cidade);
                    int valor = await _context.SaveChangesAsync();
                    if (valor == 1)
                    {
                        return Ok("Sucesso, cidade excluída.");
                    }
                    else
                    {
                        return BadRequest("Erro, cidade não excluída.");
                    }
                }
                else
                {
                    return NotFound("Erro, cidade não existe.");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na exclusão da cidade. Exceção: {e.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCidade([FromRoute] Guid id)
        {
            try
            {
                Cidade cidade = await _context.Cidade.FindAsync(id);

                if (cidade != null)
                {
                    return Ok(cidade);
                }
                else
                {
                    return NotFound("Erro, cidade não existe.");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na consulta de cidade. Exceção: {e.Message}");
            }
        }

        [HttpGet("Pesquisa")]
        public async Task<IActionResult> GetCidadePesquisa([FromQuery] string valor)
        {
            try
            {
                //Query Criteria!
                //No curso este metodo não ficou assíncrono
                var lista = from x in await _context.Cidade.ToListAsync()
                            where x.Nome.ToUpper().Contains(valor.ToUpper())
                            || x.EstadoSigla.ToUpper().Contains(valor.ToUpper())
                            select x;


                return Ok(lista);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro, pesquisa de cidade. Exceção: {e.Message}");
            }
        }

        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetCidadePaginacao([FromQuery] string valor, int skip, int take, bool ordemDesc)
        {
            try
            {
                //Query Criteria
                //No curso este metodo não ficou assíncrono
                var lista = from x in await _context.Cidade.ToListAsync()
                            where x.Nome.ToUpper().Contains(valor.ToUpper())
                            || x.EstadoSigla.ToUpper().Contains(valor.ToUpper())
                            select x;

                //Tipo de ordenação dos dados!
                if (ordemDesc)
                {
                    lista = from y in lista
                            orderby y.Nome descending
                            select y;
                }
                else
                {
                    lista = from y in lista
                            orderby y.Nome ascending
                            select y;
                }

                int qtde = lista.Count();

                lista = lista
                        .Skip(skip)
                        .Take(take)
                        .ToList();

                var paginacaoResponse = new PaginacaoResponse<Cidade>(lista, qtde, skip, take);

                return Ok(paginacaoResponse);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro, pesquisa de cidade. Exceção: {e.Message}");
            }
        }
    }
}
