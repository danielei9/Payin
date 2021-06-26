using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.SmartCity.Arguments
{
	public class ApiSensorGetInstantaneousArguments : IArgumentsBase
	{
		[Display(Name = "resources.smartCity.sensor.getInstantaneousPeriod")]
		public ApiSensorGetInstantaneousArguments_Period Period { get; set; }
		[Display(Name = "resources.smartCity.sensor.getInstantaneousDate")]
		public XpDateTime Date { get; set; }
		public int Id { get; set; }

		#region Constructors
		public ApiSensorGetInstantaneousArguments(ApiSensorGetInstantaneousArguments_Period period, XpDateTime date)
		{
			Period = period;
			Date = date;
		}
		#endregion Constructors
	}
}
