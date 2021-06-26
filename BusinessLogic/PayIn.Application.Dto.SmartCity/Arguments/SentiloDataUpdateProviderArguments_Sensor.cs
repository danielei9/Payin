using System.Collections.Generic;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.SmartCity.Arguments
{
	public class SentiloDataUpdateProviderArguments_Sensor : IArgumentsBase
	{
		public string Sensor { get; set; }
		public string Location { get; set; }

		public IEnumerable<SentiloDataUpdateProviderArguments_Observation> Observations { get; set; }

		#region Constructors
		public SentiloDataUpdateProviderArguments_Sensor(string sensor, string location, IEnumerable<SentiloDataUpdateProviderArguments_Observation> observations)
		{
			Sensor = sensor;
			Location = location;
			Observations = observations;
		}
		#endregion Constructors
	}
}
