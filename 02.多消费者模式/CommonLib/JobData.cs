namespace CommonLib;

/// <summary>
/// 任务数据
/// </summary>
public class JobData
{
    /// <summary>
    /// 任务ID
    /// </summary>
    public int JobId { get; set; }

    /// <summary>
    /// 耗时秒
    /// </summary>
    public int ConsumingSecond => new Random().Next(1, 10);

    /// <summary>
    /// 任务描述
    /// </summary>
    public string JobDescribe => $"耗时 {ConsumingSecond} 秒的任务";

    public JobData(int jobId)
    {
        JobId = jobId;
    }
}