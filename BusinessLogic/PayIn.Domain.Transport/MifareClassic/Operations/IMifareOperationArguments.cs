using Xp.Domain.Transport.MifareClassic;

namespace PayIn.Domain.Transport.MifareClassic.Operations
{
	public interface IMifareOperationArguments
	{
		MifareOperationType Operation { get; set; }
	}
}
