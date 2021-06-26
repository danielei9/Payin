using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.SmartCity.Arguments
{
	public class ApiSensorGetMaxEnergyArguments : IArgumentsBase
	{
		[Display(Name = "resources.smartCity.sensor.getMaxEnergyPeriod")]
		public ApiSensorGetMaxEnergyArguments_Period Period { get; set; }
		[Display(Name = "resources.smartCity.sensor.getMaxEnergyDate")]
		public XpDateTime Date { get; set; }
		public int Id { get; set; }

		#region Constructors
		public ApiSensorGetMaxEnergyArguments(ApiSensorGetMaxEnergyArguments_Period period, XpDateTime date)
		{
			Period = period;
			Date = date;
		}
		#endregion Constructors
	}
}
