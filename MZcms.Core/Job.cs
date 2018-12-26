using MZcms.Core.Helper;
using Quartz;
using Quartz.Impl;
using Quartz.Simpl;
using Quartz.Xml;
using System;

namespace MZcms.Core
{
	public static class Job
	{
		static Job()
		{
			XMLSchedulingDataProcessor xMLSchedulingDataProcessor = new XMLSchedulingDataProcessor(new SimpleTypeLoadHelper());
			IScheduler scheduler = (new StdSchedulerFactory()).GetScheduler();
			xMLSchedulingDataProcessor.ProcessFileAndScheduleJobs(IOHelper.GetMapPath("/quartz_jobs.xml"), scheduler);
			scheduler.Start();
		}

		public static void Start()
		{
		}
	}
}