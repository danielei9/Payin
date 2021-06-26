namespace PayIn.Domain.Transport.MifareClassic.Operations
{
	public interface IMifareReadOperationsArguments : IMifareOperationArguments
	{
		byte Sector { get; set; }
		byte Block { get; set; }
		string Data { get; set; }
	}
}
