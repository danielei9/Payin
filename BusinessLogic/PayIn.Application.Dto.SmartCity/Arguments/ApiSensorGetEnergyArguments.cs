using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.SmartCity.Arguments
{
	public class ApiSensorGetEnergyArguments : IArgumentsBase
	{
		[Display(Name = "resources.smartCity.sensor.getEnergyPeriod")]
		public ApiSensorGetEnergyArguments_Period Period { get; set; }
		[Display(Name = "resources.smartCity.sensor.getEnergyDate")]
		public XpDateTime Date { get; set; }
		public int Id { get; set; }

		#region Constructors
		public ApiSensorGetEnergyArguments(ApiSensorGetEnergyArguments_Period period, XpDateTime date)
		{
			Period = period;
			Date = date;
		}
		#endregion Constructors
	}
}
