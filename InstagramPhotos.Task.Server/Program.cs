using System;
using System.Collections.Specialized;
using Quartz;
using Quartz.Impl;
using Quartz.Job;

namespace InstagramPhotos.Task.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("开启作业调度！");
            var properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "RemoteServerSchedulerClient";

            // 设置线程池
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            properties["quartz.threadPool.threadCount"] = "5";
            properties["quartz.threadPool.threadPriority"] = "Normal";

            // 远程输出配置
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "556";
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            properties["quartz.scheduler.exporter.channelType"] = "tcp";

            var schedulerFactory = new StdSchedulerFactory(properties);
            var scheduler = schedulerFactory.GetScheduler();

            var job = JobBuilder.Create<PrintMessageJob>()
                .WithIdentity("myJob", "group1")
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("myJobTrigger", "group1")
                .StartNow()
                .WithCronSchedule("/10 * * ? * *")
                .Build();
            scheduler.ScheduleJob(job, trigger);
            scheduler.Start();

        }
    }
}
