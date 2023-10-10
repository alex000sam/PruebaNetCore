using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PruebaNetCore.Modelos;
using PruebaNetCore.Modelos.Dtos;
using PruebaNetCore.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;

namespace PruebaNetCore.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class CuentasController : ControllerBase
    {
        private readonly ICuentaRepositorio cuentaRepositorio;
        private readonly IClienteRepositorio clienteRepositorio;
        private readonly IMapper mapper;

        public CuentasController(ICuentaRepositorio cuentaRepositorio, IClienteRepositorio clienteRepositorio, IMapper mapper)
        {
            this.cuentaRepositorio = cuentaRepositorio;
            this.clienteRepositorio = clienteRepositorio;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCuentas()
        {
            var listaCuentas = cuentaRepositorio.GetCuentas();
            var listaCuentasDto = new List<CuentaDto>();

            foreach (var lista in listaCuentas)
            {
                listaCuentasDto.Add(mapper.Map<CuentaDto>(lista));
            }
            return Ok(listaCuentasDto);
        }

        [AllowAnonymous]
        [HttpGet("{numeroCuenta:int}", Name = "GetCuenta")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCuenta(int numeroCuenta)
        {
            var itemCuenta = cuentaRepositorio.GetCuenta(numeroCuenta);
            if (itemCuenta == null)
            {
                return NotFound();
            }
            var itemCuentaDto = mapper.Map<CuentaDto>(itemCuenta);
            return Ok(itemCuentaDto);
        }

        [AllowAnonymous]
        [HttpGet("cliente")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCuentasDeCliente(int clienteId)
        {
            var listaCuentas = cuentaRepositorio.GetCuentasDeCliente(clienteId);
            var listaCuentasDto = new List<CuentaDto>();

            foreach (var lista in listaCuentas)
            {
                listaCuentasDto.Add(mapper.Map<CuentaDto>(lista));
            }
            return Ok(listaCuentasDto);
        }

        [AllowAnonymous] //[Authorize(Roles = "admin")]
        [HttpPost]//("{clienteId:int}", Name = "CrearCuenta")]
        [ProducesResponseType(201, Type = typeof(CuentaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearCuenta(int clienteId, [FromBody] CuentaDto cuentaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (cuentaDto == null)
            {
                return BadRequest(ModelState);
            }
            if (cuentaRepositorio.ExisteCuenta(cuentaDto.NumeroCuenta))
            {
                ModelState.AddModelError("", "El número de cuenta ya existe.");
                return StatusCode(404, ModelState);
            }
            if (!clienteRepositorio.ExisteCliente(clienteId))
            {
                ModelState.AddModelError("", "El cliente no existe.");
                return StatusCode(404, ModelState);
            }

            var cuenta = mapper.Map<Cuenta>(cuentaDto);
            cuenta.ClienteId = clienteId;
            if (!cuentaRepositorio.CrearCuenta(cuenta))
            {
                ModelState.AddModelError("", $"Algo salió mal guardando el registro{cuenta.NumeroCuenta}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCuenta", new { numeroCuenta = cuenta.NumeroCuenta }, cuenta);
        }

        [AllowAnonymous] //[Authorize(Roles = "admin")]
        [HttpPatch("{numeroCuenta:int}", Name = "ActualizarPatchCuenta")]
        [ProducesResponseType(201, Type = typeof(CuentaDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ActualizarPatchCuenta(int numeroCuenta, int clienteId, [FromBody] CuentaDto cuentaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (cuentaDto == null)
            {
                return BadRequest(ModelState);
            }
            if (!cuentaRepositorio.ExisteCuenta(numeroCuenta))
            {
                ModelState.AddModelError("", "El número de cuenta no existe.");
                return StatusCode(404, ModelState);
            }
            if (!clienteRepositorio.ExisteCliente(clienteId))
            {
                ModelState.AddModelError("", "El cliente no existe.");
                return StatusCode(404, ModelState);
            }

            var cuenta = mapper.Map<Cuenta>(cuentaDto);
            cuenta.NumeroCuenta = numeroCuenta;
            cuenta.ClienteId = clienteId;
            if (!cuentaRepositorio.ActualizarCuenta(cuenta))
            {
                ModelState.AddModelError("", $"Algo salió mal actualizando el registro {cuenta.NumeroCuenta}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [AllowAnonymous] //[Authorize(Roles = "admin")]
        [HttpDelete("{numeroCuenta:int}", Name = "BorrarCuenta")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult BorrarCuenta(int numeroCuenta)
        {
            if (!cuentaRepositorio.ExisteCuenta(numeroCuenta))
            {
                return NotFound();
            }
            var cuenta = cuentaRepositorio.GetCuenta(numeroCuenta);

            if (cuenta is not null)
            {
                if (!cuentaRepositorio.BorrarCuenta(cuenta))
                {
                    ModelState.AddModelError("", $"Algo salió mal borrando el registro {cuenta.NumeroCuenta}");
                    return StatusCode(500, ModelState);
                }
            }
            return NoContent();
        }
    }
}
