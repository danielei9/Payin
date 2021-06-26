using System.Collections.Generic;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.SmartCity.Arguments
{
	public class SentiloDataUpdateProviderArguments : IArgumentsBase
	{
		public string ProviderCode { get; set; }
		public IEnumerable<SentiloDataUpdateProviderArguments_Sensor> Sensors { get; set; }

		#region Constructors
		public SentiloDataUpdateProviderArguments(IEnumerable<SentiloDataUpdateProviderArguments_Sensor> sensors)
		{
			Sensors = sensors;
		}
		#endregion Constructors
	}
}
