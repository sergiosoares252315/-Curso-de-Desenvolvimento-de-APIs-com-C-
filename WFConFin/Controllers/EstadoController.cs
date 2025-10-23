using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WFConFin.Data;
using WFConFin.Models;

namespace WFConFin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EstadoController : Controller
    {
        private readonly WFConFinDbContext _context;

        public EstadoController(WFConFinDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetEstados()
        {
            try
            {
                //No curso este metodo não ficou assíncrono
                var result = await _context.Estado.ToListAsync();
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro na listagem de estados. Exceção: {e.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostEstado([FromBody] Estado estado)
        {
            try
            {
                await _context.Estado.AddAsync(estado);
                int valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, estado incluído.");
                }
                else
                {
                    return BadRequest("Erro, estado não incluído.");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro, estado não incluído. Exeção: {e.Message}");
            }
        }


        [HttpPut]
        public async Task<IActionResult> PutEstado([FromBody] Estado estado)
        {
            try
            {
                _context.Estado.Update(estado);
                int valor = await _context.SaveChangesAsync();
                if (valor == 1)
                {
                    return Ok("Sucesso, estado alterado.");
                }
                else
                {
                    return BadRequest("Erro, estado não alterado.");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro, estado não alterado. Exeção: {e.Message}");
            }
        }


        [HttpDelete("{sigla}")]
        public async Task<IActionResult> DeleteEstado([FromRoute] string sigla)
        {
            try
            {

                var estado = await _context.Estado.FindAsync(sigla);

                if (estado.Sigla == sigla && !string.IsNullOrEmpty(estado.Sigla))
                {
                    _context.Estado.Remove(estado);
                    int valor = await _context.SaveChangesAsync();
                    if (valor == 1)
                    {
                        return Ok("Sucesso, estado excluído.");
                    }
                    else
                    {
                        return BadRequest("Erro, estado não excluído.");
                    }
                }
                else
                {
                    return BadRequest("Erro, estado não existe.");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro, estado não excluído. Exeção: {e.Message}");
            }
        }

        [HttpGet("{sigla}")]
        public async Task<IActionResult> GetEstado([FromRoute] string sigla)
        {
            try
            {

                var estado = await _context.Estado.FindAsync(sigla);

                if (estado.Sigla == sigla && !string.IsNullOrEmpty(estado.Sigla))
                {
                    return Ok(estado);
                }
                else
                {
                    return BadRequest("Erro, estado não existe.");
                }
            }
            catch (Exception e)
            {
                return BadRequest($"Erro, na consulta do estado. Exeção: {e.Message}");
            }
        }

        [HttpGet("Pesquisa")]
        public async Task<IActionResult> GetEstadoPesquisa([FromQuery] string valor)
        {
            try
            {
                //select * from Estado where upper(sigla) like(%valor%) or upper(nome) like(%valor%) 
                //Query Criteria
                //No curso este metodo não ficou assíncrono
                var lista = from o in await _context.Estado.ToListAsync()
                            where o.Sigla.ToUpper().Contains(valor.ToUpper())
                            || o.Nome.ToUpper().Contains(valor.ToUpper())
                            select o;

                /*Entity outra forma de fazer!
                lista = _context.Estado.Where(x => x.Sigla.ToUpper().Contains(valor.ToUpper())
                                || x.Nome.ToUpper().Contains(valor.ToUpper())).ToList(); */

                /*Expression outra forma de fazer!
                Expression<Func<Estado, bool>> expressao = x => true;
                expressao = x => x.Sigla.ToUpper().Contains(valor.ToUpper())
                            || x.Nome.ToUpper().Contains(valor.ToUpper());

                lista = _context.Estado.Where(expressao).ToList(); */

                return Ok(lista);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro, na pesquisa do estado. Exeção: {e.Message}");
            }
        }

        [HttpGet("Paginacao")]
        public async Task<IActionResult> GetEstadoPaginacao([FromQuery] string valor, int skip, int take, bool ordemDesc)
        {
            try
            {
                //select * from Estado where upper(sigla) like(%valor%) or upper(nome) like(%valor%) 
                //Query Criteria
                //No curso este metodo não ficou assíncrono
                var lista = from o in await _context.Estado.ToListAsync()
                            where o.Sigla.ToUpper().Contains(valor.ToUpper())
                            || o.Nome.ToUpper().Contains(valor.ToUpper())
                            select o;

                if (ordemDesc)
                {
                    lista = from x in lista
                            orderby x.Nome descending
                            select x;
                }
                else
                {
                    lista = from x in lista
                            orderby x.Nome ascending
                            select x;
                }

                var qtde = lista.Count();

                lista = lista.Skip(skip).Take(take).ToList();

                var paginacaoResponse = new PaginacaoResponse<Estado>(lista, qtde, skip, take);
                
                return Ok(paginacaoResponse);
            }
            catch (Exception e)
            {
                return BadRequest($"Erro, na pesquisa de estado. Exeção: {e.Message}");
            }
        }
    }
}
