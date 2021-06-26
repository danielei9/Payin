namespace PayIn.Infrastructure.JustMoney.Enums
{
	public enum CardStatus
	{
		Issued = 0x0,
		Open = 0x1,
		Lost = 0x2,
		Stolen = 0x3,
		DepositOnly = 0x4,
		CheckReason = 0x6,
		Closed = 0x9,
		PinChangeRequired = 0xA,
		PhoneNumberVerification = 0xC
	}
}
