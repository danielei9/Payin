
namespace System
{
	public static class DoubleExtension
	{
		#region ToDecimal
		public static decimal ToDecimal(this double that)
		{
			return Convert.ToDecimal(that);
		}
		#endregion ToDecimal

		#region FromRadians
		public static double FromRadians(this double valor)
		{
			return Convert.ToSingle(Math.PI / 180) * valor;
		}
		#endregion FromRadians
	}
}
