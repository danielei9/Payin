using System;

namespace PayIn.Domain.Transport
{
	[Flags]
	public enum BlackListServiceType
	{
		Rejection = 0,
		Title1 = 1,
		Title2 = 2,
		Purse = 4,
		Bonus = 8,
		BikeRentals = 16,
		Taxis = 32,
		Debit = 64
	}
}