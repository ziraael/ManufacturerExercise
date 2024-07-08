using MassTransit;
using MassTransit.Transports;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using OrderService.Api.OrderService.Application.Requests;
using OrderService.Domain.Entities;

namespace OrderService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISendEndpointProvider _sendEndpointProvider;


        public OrderController(IMediator mediator, IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpointProvider)
        {
            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpGet(nameof(GetOrder))]
        public async Task<Order?> GetOrder(int orderId)
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
            await _publishEndpoint.Publish(new OrderCreatedEvent
            {
                Id = new Guid(),
                CreatedAt = new DateTime()
            });
            var orderDetails = await _mediator.Send(new GetOrderRequest() { OrderId = orderId });
            
            return orderDetails;
        }

        [HttpPost(nameof(CreateOrder))]
        public async Task<bool> CreateOrder(Order order)
        {
            return await _mediator.Send(new CreateOrderRequest() { Order = order });
        }

        [HttpPost(nameof(CancelOrder))]
        public async Task<bool> CancelOrder(Guid orderId)
        {
            return await _mediator.Send(new CancelOrderRequest() { OrderId = orderId });
        }

        [HttpPost(nameof(ChangeOrderStatus))]
        public async Task<bool> ChangeOrderStatus(Guid orderId, string type, bool statusValue)
        {
            return await _mediator.Send(new ChangeOrderStatusRequest() { OrderId = orderId, Type = type, StatusValue = statusValue });
        }
    }
}