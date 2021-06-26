using System;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Bus.Arguments
{
	public class BusApiStopUpdateLinkArguments : IArgumentsBase
	{
        [Required]                                                      public int Id               { get; set; }
					[Display(Name = "resources.bus.stop.weight")]		public int Weight { get; set; }
					[Display(Name = "resources.bus.stop.time_Seconds")]	public int Time_Seconds { get; set; }

		#region Constructors
		public BusApiStopUpdateLinkArguments(int id, int weight, int time_Seconds)
		{
            Id = id;
			Weight = weight;
			Time_Seconds = time_Seconds;
		}
		#endregion Constructors
	}
}
