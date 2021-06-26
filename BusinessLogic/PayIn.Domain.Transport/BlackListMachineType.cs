using System;

namespace PayIn.Domain.Transport
{
	[Flags]
	public enum BlackListMachineType
	{
		Charge = 1,
		Validation = 2,
		Inspection = 4,
		CustomerSupport = 8,
		BikeRentals = 16,
		InformationPosts = 32,
		ATMs = 64,
		Taxis = 128,
		TownHall = 256,
		RegionalIntermodals = 512,
		NationalIntermodals = 1024,
		All = 2047
	}
}