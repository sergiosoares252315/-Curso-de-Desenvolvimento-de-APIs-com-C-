using Microsoft.AspNetCore.Mvc;

namespace WFConFin.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult GetInformacao()
        {
            var result = "retorno em texto!";
            return Ok(result);
        }

        [HttpGet("info2")]
        public IActionResult GetInformacao2()
        {
            var result = "retorno em texto 2";
            return Ok(result);
        }

        [HttpGet("info3/{valor}")]
        public IActionResult GetInformacao3([FromRoute] string valor)
        {
            var result = "retorno em texto 3 - Valor: " + valor;
            return Ok(result);
        }

        [HttpPost("info4")]
        public ActionResult GetInformacao4([FromQuery] string valor)
        {
            var result = "retorno em texto 4 - Valor: " + valor;
            return Ok(result);
        }

        [HttpGet("info5")]
        public ActionResult GetInformacao5([FromHeader] string valor)
        {
            var result = "retorno em texto 5 - Valor: " + valor;
            return Ok(result);
        }

        [HttpPost("info6")]
        public ActionResult GetInformacao6([FromBody] Corpo corpo)
        {
            var result = "retorno em texto 6 - Valor: " + corpo.valor;
            return Ok(result);
        }
    }

    public class Corpo
    {
        public string valor { get; set; }
    }
}
