using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaNetCore.Controllers;
using PruebaNetCore.Data;
using PruebaNetCore.Modelos.Dtos;
using PruebaNetCore.Repositorio;
using PruebaNetCore.Utilidades;

namespace PruebaNetCore.Tests
{
    public class ClientesControllerTests
    {
        ApplicationDBContext context = new ApplicationDBContext(new DbContextOptionsBuilder<ApplicationDBContext>().UseSqlServer("Server=.;Database=PruebaNetCore;Integrated Security=True;TrustServerCertificate=True").Options);
        MapperConfiguration mapperConfig = new MapperConfiguration(cfg => { cfg.AddProfile(new AutoMapperProfiles()); });

        [Fact]
        public void GetCliente_ShouldReturnOK()
        {
            //Arrange
            ClienteRepositorio clienteRepositorio = new ClienteRepositorio(context);
            IMapper mapper = new Mapper(mapperConfig);
            var controller = new ClientesController(clienteRepositorio, mapper);

            //Act
            var result = controller.GetCliente(2);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<OkObjectResult>(result);
        }

        [Fact]
        public void GetCliente_ShouldReturnNotFound()
        {
            //Arrange
            ClienteRepositorio clienteRepositorio = new ClienteRepositorio(context);
            IMapper mapper = new Mapper(mapperConfig);
            var controller = new ClientesController(clienteRepositorio, mapper);

            //Act
            controller.BorrarCliente(999);
            var result = controller.GetCliente(999);

            //Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Fact]
        public void CrearCliente_ShouldReturnCreated()
        {
            //Arrange
            ClienteRepositorio clienteRepositorio = new ClienteRepositorio(context);
            IMapper mapper = new Mapper(mapperConfig);
            var controller = new ClientesController(clienteRepositorio, mapper);

            //Act
            var result = controller.CrearCliente(new ClienteCreacionDto() {
                Nombre = "Juan Perez " + (new Random()).Next(1, 10000).ToString(), // Juan Perez Random1234
                Genero = "M",
                Edad = 25,
                Direccion = "Av. Cultura 123",
                Telefono = "984999999",
                Contraseña = "12345678",
                Estado = true
            });

            //Assert
            Assert.NotNull(result);
            ObjectResult objectResult = Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal(201, objectResult.StatusCode); //HttpStatusCode.Created = 201
        }
    }
}