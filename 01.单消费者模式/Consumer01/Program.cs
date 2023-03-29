using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

//队列名称
const string QueueName = "job";

//连接通道构建
var factory = new ConnectionFactory { HostName = "localhost", UserName = "admin", Password = "123456" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

//申明队列
channel.QueueDeclare(QueueName,
         durable: false,
         exclusive: false,
         autoDelete: false,
         arguments: null);


//等待消息
Console.WriteLine(" [*] Waiting for messages.");
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) => {
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");
};
channel.BasicConsume(QueueName, true, consumer);

Console.ReadKey();