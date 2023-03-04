using L01_2020PG601.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace L01_2020PG601.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class clientesController : ControllerBase
    {
        private readonly restauranteContext _restauranteContext;
        public clientesController(restauranteContext restauranteContext)
        {
            _restauranteContext = restauranteContext;
        }


        //CRUD
        [HttpGet]
        [Route("GetAll")]
        //obtiene todos los registros
        public IActionResult Obtenerclientes()
        {
            List<clientes> clientesLista = (from e in _restauranteContext.clientes
                                        select e).ToList();



            if (clientesLista.Count == 0)
            {
                return NotFound();
            }

            return Ok(clientesLista);
        }


        //agregar 
        [HttpPost]
        [Route("Add")]
        public IActionResult GuardarCliente([FromBody] clientes cliente)
        {
            try
            {
                _restauranteContext.clientes.Add(cliente);
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

        public IActionResult ActualizarClientes(int id, [FromBody] clientes clienteModificar)
        {
            clientes? clienteActual = (from e in _restauranteContext.clientes
                                   where e.clienteId == id
                                   select e).FirstOrDefault();
            if (clienteActual == null)
                return NotFound();

            clienteActual.nombreCliente = clienteModificar.nombreCliente;
            clienteActual.direccion = clienteModificar.direccion;

            _restauranteContext.Entry(clienteActual).State = EntityState.Modified;
            _restauranteContext.SaveChanges();

            return Ok(clienteModificar);
        }


        //eliminar
        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult EliminarClientes(int id)
        {
            clientes? cliente = (from e in _restauranteContext.clientes
                             where e.clienteId == id
                             select e).FirstOrDefault();

            if (cliente == null) return NotFound();

            _restauranteContext.clientes.Attach(cliente);
            _restauranteContext.clientes.Remove(cliente);
            _restauranteContext.SaveChanges();

            return Ok(cliente);
        }

        [HttpGet]
        [Route("GetByDireccion/{direccion}")]
        public IActionResult GetByDireccion(string direccion)
        {
            clientes? cliente = (from e in _restauranteContext.clientes
                               where e.direccion.Contains(direccion)
                               select e).FirstOrDefault();

            if (cliente == null) return NotFound();

            return Ok(cliente);
        }
    }
}
