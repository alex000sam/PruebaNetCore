using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PruebaNetCore.Modelos;
using PruebaNetCore.Modelos.Dtos;
using PruebaNetCore.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Authorization;

namespace PruebaNetCore.Controllers
{
    [ApiController]
    [Route("api/movimientos")]
    public class MovimientosController : ControllerBase
    {
        private readonly IMovimientoRepositorio movimientoRepositorio;
        private readonly ICuentaRepositorio cuentaRepositorio;
        private readonly IClienteRepositorio clienteRepositorio;
        private readonly IMapper mapper;

        public MovimientosController(IMovimientoRepositorio movimientoRepositorio, ICuentaRepositorio cuentaRepositorio,
            IClienteRepositorio clienteRepositorio, IMapper mapper)
        {
            this.movimientoRepositorio = movimientoRepositorio;
            this.cuentaRepositorio = cuentaRepositorio;
            this.clienteRepositorio = clienteRepositorio;
            this.mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetMovimientos()
        {
            try
            {
                var listaMovimientos = movimientoRepositorio.GetMovimientos();
                var listaMovimientosDto = new List<MovimientoDto>();

                foreach (var lista in listaMovimientos)
                {
                    listaMovimientosDto.Add(mapper.Map<MovimientoDto>(lista));
                }
                return Ok(listaMovimientosDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos.");
            }
        }

        [AllowAnonymous]
        [HttpGet("{movimientoId:int}", Name = "GetMovimiento")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetMovimiento(int movimientoId)
        {
            try
            {
                var itemMovimiento = movimientoRepositorio.GetMovimiento(movimientoId);
                if (itemMovimiento == null)
                {
                    return NotFound();
                }
                var itemMovimientoDto = mapper.Map<MovimientoDto>(itemMovimiento);
                return Ok(itemMovimientoDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos.");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(MovimientoDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearMovimiento(int cuentaId, [FromBody] MovimientoCreacionDto movimientoCreacionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (movimientoCreacionDto == null)
                {
                    return BadRequest(ModelState);
                }
                if (!cuentaRepositorio.ExisteCuenta(cuentaId))
                {
                    ModelState.AddModelError("", "El número de cuenta no existe.");
                    return StatusCode(404, ModelState);
                }

                var movimiento = mapper.Map<Movimiento>(movimientoCreacionDto);
                movimiento.Saldo = cuentaRepositorio.SaldoActual(cuentaId) + movimiento.Valor;

                if (movimiento.Saldo < 0)
                {
                    return BadRequest("Saldo no disponible.");
                }

                movimiento.CuentaId = cuentaId;
                movimiento.Fecha = DateTime.Now;

                if (!movimientoRepositorio.CrearMovimiento(movimiento))
                {
                    ModelState.AddModelError("", $"Algo salió mal guardando el registro de movimiento de la cuenta {cuentaId}");
                    return StatusCode(500, ModelState);
                }
                return CreatedAtRoute("GetMovimiento", new { movimientoId = movimiento.Id }, movimiento);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos.");
            }
        }

        [AllowAnonymous]
        [HttpPatch("{movimientoId:int}", Name = "ActualizarPatchMovimiento")]
        [ProducesResponseType(201, Type = typeof(MovimientoDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ActualizarPatchMovimiento(int movimientoId, int cuentaId, [FromBody] MovimientoCreacionDto movimientoCreacionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (movimientoCreacionDto == null)
                {
                    return BadRequest(ModelState);
                }
                if (!movimientoRepositorio.ExisteMovimiento(movimientoId))
                {
                    ModelState.AddModelError("", "El número de movimiento no existe.");
                    return StatusCode(404, ModelState);
                }
                if (!cuentaRepositorio.ExisteCuenta(cuentaId))
                {
                    ModelState.AddModelError("", "El número de cuenta no existe.");
                    return StatusCode(404, ModelState);
                }

                var movimiento = mapper.Map<Movimiento>(movimientoCreacionDto);
                movimiento.Id = movimientoId;
                movimiento.CuentaId = cuentaId;

                if (!movimientoRepositorio.ActualizarMovimiento(movimiento))
                {
                    ModelState.AddModelError("", $"Algo salió mal actualizando el registro {movimiento.Id}");
                    return StatusCode(500, ModelState);
                }
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos.");
            }
        }

        [AllowAnonymous]
        [HttpDelete("{movimientoId:int}", Name = "BorrarMovimiento")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult BorrarMovimiento(int movimientoId)
        {
            try
            {
                if (!movimientoRepositorio.ExisteMovimiento(movimientoId))
                {
                    return NotFound();
                }
                var movimiento = movimientoRepositorio.GetMovimiento(movimientoId);

                if (movimiento is not null)
                {
                    if (!movimientoRepositorio.BorrarMovimiento(movimiento))
                    {
                        ModelState.AddModelError("", $"Algo salió mal borrando el registro {movimiento.Id}");
                        return StatusCode(500, ModelState);
                    }
                }
                
                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos.");
            }
        }

        [AllowAnonymous]
        [HttpGet("cliente")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetMovimientosDeCuentasDeCliente(int clienteId)
        {
            try
            {
                if (!clienteRepositorio.ExisteCliente(clienteId))
                {
                    ModelState.AddModelError("", "El cliente no existe.");
                    return StatusCode(404, ModelState);
                }

                var cliente = clienteRepositorio.GetCliente(clienteId);
                var nombreCliente = "";
                if (cliente is not null)
                {
                    nombreCliente = cliente.Nombre;
                }
                var listaMovimientos = movimientoRepositorio.GetMovimientosDeCuentasDeCliente(clienteId)
                    .Join(cuentaRepositorio.GetCuentas(), m => m.CuentaId, c => c.NumeroCuenta, (m, c) => new
                    {
                        m.Fecha,
                        nombreCliente,
                        numeroCuenta = m.CuentaId,
                        c.TipoCuenta,
                        saldoInicial = m.Saldo - m.Valor,
                        c.Estado,
                        m.Valor,
                        saldoDisponible = m.Saldo
                    });

                return Ok(listaMovimientos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos.");
            }
        }

        [AllowAnonymous]
        [HttpGet("reporte")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetMovimientosDeCuentasDeClientePorRangoFecha(int clienteId, DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                if (!clienteRepositorio.ExisteCliente(clienteId))
                {
                    ModelState.AddModelError("", "El cliente no existe.");
                    return StatusCode(404, ModelState);
                }

                var listaMovimientos = movimientoRepositorio.GetMovimientosDeCuentasDeCliente(clienteId)
                    .Where(m => m.Fecha >= fechaInicio && m.Fecha <= fechaFin)
                    .Join(cuentaRepositorio.GetCuentas(), m => m.CuentaId, c => c.NumeroCuenta, (m, c) => new
                    {
                        numeroCuenta = m.CuentaId,
                        m.Fecha,
                        c.TipoCuenta,
                        saldoInicial = m.Saldo - m.Valor,
                        m.Valor,
                        saldoDisponible = m.Saldo
                    }).OrderBy(m => m.numeroCuenta).ThenBy(m => m.Fecha);
                return Ok(listaMovimientos);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos.");
            }
        }
    }
}
