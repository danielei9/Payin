using System.Collections.Generic;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.SmartCity.Arguments
{
	public class SentiloDataUpdateSensorArguments : IArgumentsBase
	{
		public string ProviderCode { get; set; }
		public string SensorCode { get; set; }
		public XpDateTime Timestamp { get; set; }
		public string Location { get; set; }

		public IEnumerable<SentiloDataUpdateProviderArguments_Observation> Observations { get; set; }

		#region Constructors
		public SentiloDataUpdateSensorArguments(XpDateTime timestamp, string location, IEnumerable<SentiloDataUpdateProviderArguments_Observation> observations)
		{
			Timestamp = timestamp;
			Location = location ?? "";
			Observations = observations;
		}
		#endregion Constructors
	}
}
