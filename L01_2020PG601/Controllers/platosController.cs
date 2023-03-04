using L01_2020PG601.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2020PG601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class platosController : ControllerBase
    {
        private readonly restauranteContext _restauranteContext;
        public platosController(restauranteContext restauranteContext)
        {
            _restauranteContext = restauranteContext;
        }


        //CRUD
        [HttpGet]
        [Route("GetAll")]
        //obtiene todos los registros
        public IActionResult Obtenerplatos()
        {
            List<platos> platosLista = (from e in _restauranteContext.platos
                                        select e).ToList();



            if (platosLista.Count == 0)
            {
                return NotFound();
            }

            return Ok(platosLista);
        }


        //agregar 
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarPlato([FromBody] platos plato)
        {
            try
            {
                _restauranteContext.platos.Add(plato);
                _restauranteContext.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //actualizar
        [HttpPut]
        [Route("actuzlizar/{id}")]

        public IActionResult ActualizarPlatos(int id, [FromBody] platos platoModificar)
        {
            platos? platoActual = (from e in _restauranteContext.platos
                                   where e.platoId == id
                                   select e).FirstOrDefault();
            if (platoActual == null)
                return NotFound();

            platoActual.nombrePlato = platoModificar.nombrePlato;
            platoActual.precio = platoModificar.precio;

            _restauranteContext.Entry(platoActual).State = EntityState.Modified;
            _restauranteContext.SaveChanges();

            return Ok(platoModificar);
        }


        //eliminar
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarPlatos(int id)
        {
            platos? plato = (from e in _restauranteContext.platos
                             where e.platoId == id
                             select e).FirstOrDefault();

            if (plato == null) return NotFound();

            _restauranteContext.platos.Attach(plato);
            _restauranteContext.platos.Remove(plato);
            _restauranteContext.SaveChanges();

            return Ok(plato);
        }

        [HttpGet]
        [Route("GetByPlatoNombre/{nombrePlato}")]
        public IActionResult GetByClientId(string nombrePlato)
        {
            platos? plato = (from e in _restauranteContext.platos
                               where e.nombrePlato.Contains(nombrePlato)
                               select e).FirstOrDefault();

            if (plato == null) return NotFound();

            return Ok(plato);
        }

    }
}
