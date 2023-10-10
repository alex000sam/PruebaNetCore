using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaNetCore.Controllers;
using PruebaNetCore.Data;
using PruebaNetCore.Modelos.Dtos;
using PruebaNetCore.Repositorio;
using PruebaNetCore.Utilidades;
using System.Net;

namespace PruebaNetCore.Tests
{
    public class MovimientosControllerTests
    {
        ApplicationDBContext context = new ApplicationDBContext(new DbContextOptionsBuilder<ApplicationDBContext>().UseSqlServer("Server=.;Database=PruebaNetCore;Integrated Security=True;TrustServerCertificate=True").Options);
        MapperConfiguration mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new AutoMapperProfiles()); });

        [Fact]
        public void GetMovimiento_ShouldReturnOK()
        {
            //Arrange
            MovimientoRepositorio movimientoRepositorio = new MovimientoRepositorio(context);
            CuentaRepositorio cuentaRepositorio = new CuentaRepositorio(context);
            ClienteRepositorio clienteRepositorio = new ClienteRepositorio(context);
            IMapper mapper = new Mapper(mapperConfig);
            var controller = new MovimientosController(movimientoRepositorio, cuentaRepositorio, clienteRepositorio, mapper);

            int movimientoId = 1;

            //Act
            var result = controller.GetMovimiento(movimientoId);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public void CrearMovimiento_ShouldReturnCreated()
        {
            //Arrange
            MovimientoRepositorio movimientoRepositorio = new MovimientoRepositorio(context);
            CuentaRepositorio cuentaRepositorio = new CuentaRepositorio(context);
            ClienteRepositorio clienteRepositorio = new ClienteRepositorio(context);
            IMapper mapper = new Mapper(mapperConfig);
            var controller = new MovimientosController(movimientoRepositorio, cuentaRepositorio, clienteRepositorio, mapper);

            int cuentaId = 202;

            //Act
            var result = controller.CrearMovimiento(cuentaId, new MovimientoCreacionDto()
            {
                TipoMovimiento = "Depósito",
                Valor = 100
            });

            //Assert
            Assert.NotNull(result);
            ObjectResult objectResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal(201, objectResult.StatusCode); //HttpStatusCode.Created = 201
        }

        [Fact]
        public void CrearMovimiento_ShouldReturnSaldoNoDisponible()
        {
            //Arrange
            MovimientoRepositorio movimientoRepositorio = new MovimientoRepositorio(context);
            CuentaRepositorio cuentaRepositorio = new CuentaRepositorio(context);
            ClienteRepositorio clienteRepositorio = new ClienteRepositorio(context);
            IMapper mapper = new Mapper(mapperConfig);
            var controller = new MovimientosController(movimientoRepositorio, cuentaRepositorio, clienteRepositorio, mapper);

            int cuentaId = 202;

            //Act
            var result = controller.CrearMovimiento(cuentaId, new MovimientoCreacionDto()
            {
                TipoMovimiento = "Retiro",
                Valor = -1000000
            });

            //Assert
            Assert.NotNull(result);
            ObjectResult objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode); //HttpStatusCode.BadRequest = 400
            Assert.Equal("Saldo no disponible.", objectResult.Value);
        }
    }
}