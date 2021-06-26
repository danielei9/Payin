using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.SmartCity.Arguments
{
	public class ApiSmartCitySetTargetValueArguments : IArgumentsBase
	{
        [Required]                                                  public int Id { get; set; }
		[Display(Name = "resources.smartCity.sensor.targetValue")]  public decimal? TargetValue { get; set; }

		#region Constructors
		public ApiSmartCitySetTargetValueArguments(int id, decimal? targetValue)
		{
			Id = id;
			TargetValue = targetValue;
		}
		#endregion Constructors
	}
}
