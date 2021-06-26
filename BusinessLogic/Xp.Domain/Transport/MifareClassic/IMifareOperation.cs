namespace Xp.Domain.Transport.MifareClassic
{
	public interface IMifareOperation
	{
		MifareOperationType Operation { get; set; }
		byte Sector { get; set; }
	}
}
