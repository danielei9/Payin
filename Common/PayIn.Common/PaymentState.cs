namespace PayIn.Common
{
	public enum PaymentState
	{
		Active = 1,
		Error = 2,
		Pending = 3,
		Returned = 4 // Calculado: Simplemente vale como valor para devolver en consultas
	}
}
