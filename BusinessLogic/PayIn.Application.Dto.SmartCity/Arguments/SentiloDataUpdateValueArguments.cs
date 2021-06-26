using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.SmartCity.Arguments
{
	public class SentiloDataUpdateValueArguments : IArgumentsBase
	{
		public string ProviderCode { get; set; }
		public string SensorCode { get; set; }
		public decimal Value { get; set; }

		#region Constructors
		public SentiloDataUpdateValueArguments(string providerCode, string sensorCode, decimal value)
		{
			ProviderCode = providerCode;
			SensorCode = sensorCode;
			Value = value;
		}
		#endregion Constructors
	}
}
