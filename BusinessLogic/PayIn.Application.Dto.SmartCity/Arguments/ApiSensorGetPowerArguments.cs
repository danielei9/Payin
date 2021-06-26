using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.SmartCity.Arguments
{
	public class ApiSensorGetPowerArguments : IArgumentsBase
	{
		[Display(Name = "resources.smartCity.sensor.getEnergyPeriod")]
		public ApiSensorGetPowerArguments_Period Period { get; set; }
		[Display(Name = "resources.smartCity.sensor.getEnergyDate")]
		public XpDate Date { get; set; }
		public int Id { get; set; }

		#region Constructors
		public ApiSensorGetPowerArguments(ApiSensorGetPowerArguments_Period period, XpDate date)
		{
			Period = period;
			Date = date;
		}
		#endregion Constructors
	}
}
