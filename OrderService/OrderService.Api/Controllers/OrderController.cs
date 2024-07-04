using System.Text;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Api.OrderService.Application.Requests;
using OrderService.Domain.Entities;
//using RabbitMQ.Client;
//using RabbitMQ.Client.Events;

namespace OrderService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(nameof(GetOrderAsync))]
        public async Task<Order?> GetOrderAsync(int orderId)
        {
            //var factory = new ConnectionFactory { HostName = "localhost" };
            //using var connection = factory.CreateConnection();
            //using var channel = connection.CreateModel();

            //channel.QueueDeclare(queue: "task_queue",
            //                     durable: true,
            //                     exclusive: false,
            //                     autoDelete: false,
            //                     arguments: null);

            //channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            //Console.WriteLine(" [*] Waiting for messages.");

            //var consumer = new EventingBasicConsumer(channel);
            //consumer.Received += (model, ea) =>
            //{
            //    byte[] body = ea.Body.ToArray();
            //    var message = Encoding.UTF8.GetString(body);
            //    Console.WriteLine($" [x] Received {message}");

            //    int dots = message.Split('.').Length - 1;
            //    Thread.Sleep(dots * 1000);

            //    Console.WriteLine(" [x] Done");

            //    // here channel could also be accessed as ((EventingBasicConsumer)sender).Model
            //    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            //};
            //channel.BasicConsume(queue: "task_queue",
            //                     autoAck: false,
            //                     consumer: consumer);

            //Console.WriteLine(" Press [enter] to exit.");
            //Console.ReadLine();
            var orderDetails = await _mediator.Send(new GetOrderRequest() { OrderId = orderId });
            
            return orderDetails;
        }

        [HttpPost(nameof(CreateOrderAsync))]
        public async Task<Order> CreateOrderAsync(Order order)
        {
            var orderId = await _mediator.Send(new CreateOrderRequest() { Order = order });
            return orderId;
        }
    }
}