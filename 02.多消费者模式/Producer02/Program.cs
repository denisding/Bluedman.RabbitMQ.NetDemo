using CommonLib;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

//队列名称
const string QueueName = "job";

while (true)
{
    Console.Write("添加几个耗时任务到消息队列：");
    var inputLine = Console.ReadLine();

    if (string.IsNullOrEmpty(inputLine))
    {
        Console.WriteLine("请输入一个整数！");
    }
    else if(int.TryParse(inputLine, out int jobCount))
    {
        if (jobCount <= 0)
        {
            Console.WriteLine("请输入一个有效的整数！");
        }
        else
        {
            AddJob(jobCount);
            Console.WriteLine($"自动添加 {jobCount} 个耗时任务到消息队列！");
        }
    }
    else
    {
        Console.WriteLine("请输入一个有效的整数！");
    }
}

/// <summary>
/// 添加任务
/// </summary>
static void AddJob(int jobCount)
{
    //连接通道构建
    var factory = new ConnectionFactory { HostName = "localhost", UserName = "admin", Password = "123456" };
    var connection = factory.CreateConnection();
    var channel = connection.CreateModel();

    //申明队列
    channel.QueueDeclare(QueueName,
             durable: false,
             exclusive: false,
             autoDelete: false,
             arguments: null);

    //循环添加
    for (var index = 0; index < jobCount; index ++)
    {
        //消息
        var message = JsonConvert.SerializeObject(new JobData(index + 1));
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(string.Empty, QueueName, mandatory: false, basicProperties: null, body: body);
        Console.WriteLine($" [x] Sent {message}");
    }
}