using System;
namespace Xp.Domain.Transport
{
	public enum OperationType
	{
		Charge = 1,
		Recharge = 2,
		Read = 3,
		Revoke = 4,
		InstantPromotion = 5,
		CreateCard = 6,
		Search = 7,
        Refund = 8,
        Purchaise = 9,
        Unemit = 10
    }
}
