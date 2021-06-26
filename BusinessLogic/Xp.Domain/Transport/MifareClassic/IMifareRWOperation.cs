namespace Xp.Domain.Transport.MifareClassic
{
	public interface IMifareRWOperation : IMifareOperation
	{
		byte Block { get; set; }
		string Data { get; set; }
	}
}
