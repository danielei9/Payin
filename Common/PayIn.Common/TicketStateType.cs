namespace PayIn.Common
{
	public enum TicketStateType
	{
		Active        = 1,
		Cancelled     = 2,
		TimedOut	  = 3,
		Preactive	  = 4,
		Pending       = 5, // Pedido pagado
		Prepared      = 6,
		Created       = 7
	}
}
