using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using CommonLib;
using Newtonsoft.Json;
using System.Diagnostics;

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

//进程ID
var processId = Process.GetCurrentProcess().Id;

//等待消息
Console.WriteLine($" [{processId}] Waiting for messages.");
var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) => {
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var jobData = JsonConvert.DeserializeObject<JobData>(message);
    if (jobData == null)
    {
        Console.WriteLine($" [{processId}] Received No Content.");
    }
    else
    {
        Console.Write($" [{processId}] {jobData.JobId}.处理任务一个{jobData.JobDescribe}...");
        Thread.Sleep(jobData.ConsumingSecond * 1000);
        Console.WriteLine("完成。");

        channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
    }
};
channel.BasicConsume(QueueName, false, consumer);

Console.ReadKey();