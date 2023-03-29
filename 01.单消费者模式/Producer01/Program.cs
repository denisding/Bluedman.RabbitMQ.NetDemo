using RabbitMQ.Client;
using System.Text;

//队列名称
const string QueueName = "job";

while (true)
{
    Console.Write("Input: ");
    var message = Console.ReadLine();
    if (message != null)
    {
        Send(message);
    }
}

/// <summary>
/// 发送
/// </summary>
static void Send(string message)
{
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

    //消息
    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(string.Empty, QueueName, mandatory: false, basicProperties: null, body: body);
    Console.WriteLine($" [x] Sent {message}");
}