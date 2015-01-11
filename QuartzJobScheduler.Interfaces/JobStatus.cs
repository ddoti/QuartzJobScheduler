namespace QuartzJobScheduler
{
	public enum JobStatus
	{
		Success = 0,
		Failure = 1,
		Error = 2,
		Queued = 3,
		Running = 4,
		Unknown = -1
	}
}
