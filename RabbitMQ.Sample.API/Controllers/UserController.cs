using MassTransit;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Sample.Common.Messages.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RabbitMQ.Sample.API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private IBus _bus;

        public UserController(IBus bus)
        {
            _bus = bus;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var addUserEndpoint = await _bus.GetSendEndpoint(new Uri("rabbitmq://localhost/AddUser1"));

            await addUserEndpoint.Send<IAddUserCommand>(new
            {
                FirstName = "Douglas",
                LastName = "Franco",
                Email = "douglas.franco@dzfweb.com.br"
            });
            return Ok();
        }
    }
}