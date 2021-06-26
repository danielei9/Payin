namespace PayIn.Domain.Transport
{
	public enum RechargeType
	{
		Charge = 1,
		Recharge = 2,
		Exchange = 3,
		Replace = 4,
		RechargeAndUpdatePrice = 5,
		RechargeAndUpdateZone = 6,	
		Revoke = 7,
		RechargeExpiredPrice = 8
	}
}