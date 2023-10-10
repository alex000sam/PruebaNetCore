using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PruebaNetCore.Modelos;
using PruebaNetCore.Modelos.Dtos;
using PruebaNetCore.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;

namespace PruebaNetCore.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteRepositorio clienteRepositorio;
        private readonly IMapper mapper;

        public ClientesController(IClienteRepositorio clienteRepositorio, IMapper mapper)
        {
            this.clienteRepositorio = clienteRepositorio;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetClientes()
        {
            var listaClientes = clienteRepositorio.GetClientes();
            var listaClientesDto = new List<ClienteDto>();

            foreach (var lista in listaClientes)
            {
                listaClientesDto.Add(mapper.Map<ClienteDto>(lista));
            }
            return Ok(listaClientesDto);
        }

        [AllowAnonymous]
        [HttpGet("{clienteId:int}", Name = "GetCliente")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCliente(int clienteId)
        {
            var itemCliente = clienteRepositorio.GetCliente(clienteId);
            if (itemCliente == null)
            {
                return NotFound();
            }
            var itemClienteDto = mapper.Map<ClienteDto>(itemCliente);
            return Ok(itemClienteDto);
        }

        [AllowAnonymous] //[Authorize(Roles = "admin")]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ClienteDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearCliente([FromBody] ClienteCreacionDto clienteCreacionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (clienteCreacionDto == null)
            {
                return BadRequest(ModelState);
            }
            if (clienteRepositorio.ExisteCliente(clienteCreacionDto.Nombre))
            {
                ModelState.AddModelError("", "El cliente ya existe.");
                return StatusCode(404, ModelState);
            }

            var cliente = mapper.Map<Cliente>(clienteCreacionDto);
            if (!clienteRepositorio.CrearCliente(cliente))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro {cliente.Nombre}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCliente", new { clienteId = cliente.Id }, cliente);
        }

        [AllowAnonymous] //[Authorize(Roles = "admin")]
        [HttpPatch("{clienteId:int}", Name = "ActualizarPatchCliente")]
        [ProducesResponseType(201, Type = typeof(ClienteDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[HttpPut("{id:int}")]
        public IActionResult ActualizarPatchCliente(int clienteId, [FromBody] ClienteCreacionDto clienteCreacionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (clienteCreacionDto == null)// || clienteId != clienteDto.Id)
            {
                return BadRequest(ModelState);
            }
            var cliente = mapper.Map<Cliente>(clienteCreacionDto);
            cliente.Id = clienteId;
            if (!clienteRepositorio.ActualizarCliente(cliente))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro {cliente.Nombre}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [AllowAnonymous] //[Authorize(Roles = "admin")]
        [HttpDelete("{clienteId:int}", Name = "BorrarCliente")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult BorrarCliente(int clienteId)
        {
            if (!clienteRepositorio.ExisteCliente(clienteId))
            {
                return NotFound();
            }
            var cliente = clienteRepositorio.GetCliente(clienteId);

            if (cliente is not null)
            {
                if (!clienteRepositorio.BorrarCliente(cliente))
                {
                    ModelState.AddModelError("", $"Algo salió mal borrando el registro {cliente.Nombre}");
                    return StatusCode(500, ModelState);
                }
            }
            
            return NoContent();
        }
    }
}
