
namespace System
{
	public static class DecimalExtension
	{
		#region ToDouble
		public static double ToDouble(this decimal that)
		{
			var result = Convert.ToDouble(that);
			return result;
		}
		#endregion ToDouble
	}
}
