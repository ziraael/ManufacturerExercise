using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

//public class EventBusRabbitMQ : IEventBus, IDisposable
//{
    // Member objects and other methods ...
    // ...
//    private string _brokerName = "Manufacturer_Event_Bus";
    // public void Publish(IntegrationEvent @event)
    // {
    //     var eventName = @event.GetType().Name;
    //     var factory = new ConnectionFactory() { HostName = "localhost" };
    //     using (var connection = factory.CreateConnection())
    //     using (var channel = connection.CreateModel())
    //     {
    //         Console.WriteLine("Declaring RabbitMQ exchange to publish event: {EventId}", @event.Id);

    //         channel.ExchangeDeclare(exchange: _brokerName,
    //             type: "direct");
    //         string message = JsonConvert.SerializeObject(@event);
    //         var body = Encoding.UTF8.GetBytes(message);
    //         Console.WriteLine("Publishing event to RabbitMQ: {EventId}", @event.Id);

    //         channel.BasicPublish(exchange: _brokerName,
    //             routingKey: eventName,
    //             basicProperties: null,
    //             body: body);
    //     }
    // }
//}