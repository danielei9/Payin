namespace PayIn.Common
{
	public enum ServiceOperationType
	{
		Other = 0,
		CardEmited = 1,
		CardReturned = 2,
		PurseRecharged = 3, //venta sold
		PurseReturned = 4, // pueserSellReturned
		ProductBought = 5,
		ProductReturned = 6,
		EntranceBought = 7,
		EntranceReturned = 8,
		TpvBought = 9,
		TpvReturned = 10,
		CardRead = 11
	}
}
