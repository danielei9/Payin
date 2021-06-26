namespace Xp.Domain
{
	public interface IQueueEntity
	{
		string MessageId { get; set; }
		string PopReceipt { get; set; }
	}
}
