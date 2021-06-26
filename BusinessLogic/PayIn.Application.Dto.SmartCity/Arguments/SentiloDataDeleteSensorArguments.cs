using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.SmartCity.Arguments
{
	public class SentiloDataDeleteSensorArguments : IArgumentsBase
	{
		public int ProviderId { get; set; }
		public int SensorId { get; set; }

		#region Constructors
		public SentiloDataDeleteSensorArguments(int providerId, int sensorId)
		{
			ProviderId = providerId;
			SensorId = sensorId;
		}
		#endregion Constructors
	}
}
