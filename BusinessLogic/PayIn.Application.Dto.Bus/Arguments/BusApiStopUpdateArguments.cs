using System;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Bus.Arguments
{
	public class BusApiStopUpdateArguments : IArgumentsBase
	{
        [Required]																							public int Id                       { get; set; }
		[Required(AllowEmptyStrings = false)] [Display(Name = "resources.bus.stop.code")]					public string Code                  { get; set; }
		[Required(AllowEmptyStrings = false)] [Display(Name = "resources.bus.stop.masterCode")]				public string MasterCode            { get; set; }
		[Required(AllowEmptyStrings = false)] [Display(Name = "resources.bus.stop.name")]					public string Name                  { get; set; }
		                                      [Display(Name = "resources.bus.stop.location")]				public string Location              { get; set; }
		                                      [Display(Name = "resources.bus.stop.longitude")]				public decimal? Longitude           { get; set; }
		                                      [Display(Name = "resources.bus.stop.latitude")]				public decimal? Latitude            { get; set; }
		                                      [Display(Name = "resources.bus.stop.geofenceRadious")]		public decimal? GeofenceRadious     { get; set; }
		[Required]                            [Display(Name = "resources.bus.stop.waitingTime_Seconds")]	public int WaitingTime_Seconds		{ get; set; }

		#region Constructors
		public BusApiStopUpdateArguments(int id, string code, string masterCode, string name, string location, decimal? longitude, decimal? latitude, decimal? geofenceRadious, int waitingTime_Seconds)
		{
            Id = id;
			Code = code;
			MasterCode = masterCode;
            Name = name;
			Location = location ?? "";
            Longitude = longitude;
            Latitude = latitude;
            GeofenceRadious = geofenceRadious;
			WaitingTime_Seconds = waitingTime_Seconds;
        }
		#endregion Constructors
	}
}
