using L01_2020PG601.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2020PG601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class pedidosController : ControllerBase
    {
        private readonly restauranteContext _restauranteContext;
        public pedidosController(restauranteContext restauranteContext)
        {
            _restauranteContext = restauranteContext;
        }

        //CRUD
        [HttpGet]
        [Route("GetAll")]
        //obtiene todos los registros
        public IActionResult Obtenerpedidos()
        {
            List<pedidos> pedidosLista = (from e in _restauranteContext.pedidos
                                          select e).ToList();



            if (pedidosLista.Count == 0)
            {
                return NotFound();
            }

            return Ok(pedidosLista);
        }


        //agregar 
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarPedido([FromBody] pedidos pedido)
        {
            try
            {
                _restauranteContext.pedidos.Add(pedido);
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

        public IActionResult ActualizarPedidos(int id, [FromBody] pedidos pedidoModificar)
        {
            pedidos? pedidoActual = (from e in _restauranteContext.pedidos
                                     where e.pedidoId == id
                                     select e).FirstOrDefault();
            if (pedidoActual == null)
                return NotFound();

            pedidoActual.motoristaId = pedidoModificar.motoristaId;
            pedidoActual.clienteId = pedidoModificar.clienteId;
            pedidoActual.platoId = pedidoModificar.platoId;
            pedidoActual.cantidad = pedidoModificar.cantidad;
            pedidoActual.precio = pedidoModificar.precio;

            _restauranteContext.Entry(pedidoActual).State = EntityState.Modified;
            _restauranteContext.SaveChanges();

            return Ok(pedidoModificar);
        }


        //eliminar
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarPedidos(int id)
        {
            pedidos? pedido = (from e in _restauranteContext.pedidos
                               where e.pedidoId == id
                               select e).FirstOrDefault();

            if (pedido == null) return NotFound();

            _restauranteContext.pedidos.Attach(pedido);
            _restauranteContext.pedidos.Remove(pedido);
            _restauranteContext.SaveChanges();

            return Ok(pedido);
        }

        //filtrado por cliente
        [HttpGet]
        [Route("GetByClientId/{clienteId}")]
        public IActionResult GetByClientId(int clienteId)
        {
            pedidos? pedido = (from e in _restauranteContext.pedidos
                               where e.clienteId == clienteId
                               select e).FirstOrDefault();  

            if(pedido == null) return NotFound();   

            return Ok(pedido);
        }

        [HttpGet]
        [Route("GetByMotoristaId/{motoristaId}")]
        public IActionResult GetByMotoristaId(int motoristaId)
        {
            pedidos? pedido = (from e in _restauranteContext.pedidos
                               where e.motoristaId == motoristaId
                               select e).FirstOrDefault();

            if (pedido == null) return NotFound();

            return Ok(pedido);
        }
    }
}
