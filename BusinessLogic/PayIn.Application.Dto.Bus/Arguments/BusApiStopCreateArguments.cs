using PayIn.Domain.Bus.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Bus.Arguments
{
	public class BusApiStopCreateArguments : IArgumentsBase
	{
		[Required(AllowEmptyStrings = false)] [Display(Name = "resources.bus.stop.code")]					public string Code                  { get; set; }
		[Required(AllowEmptyStrings = false)] [Display(Name = "resources.bus.stop.masterCode")]				public string MasterCode            { get; set; }
		[Required(AllowEmptyStrings = false)] [Display(Name = "resources.bus.stop.name")]					public string Name                  { get; set; }
		                                      [Display(Name = "resources.bus.stop.location")]				public string Location              { get; set; }
		                                      [Display(Name = "resources.bus.stop.longitude")]				public decimal? Longitude           { get; set; }
		                                      [Display(Name = "resources.bus.stop.latitude")]				public decimal? Latitude            { get; set; }
		                                      [Display(Name = "resources.bus.stop.geofenceRadious")]		public decimal? GeofenceRadious     { get; set; }
		[Required]                            [Display(Name = "resources.bus.stop.waitingTime_Seconds")]	public int WaitingTime_Seconds		{ get; set; }
		[Required]                            [Display(Name = "resources.bus.stop.lineId")] 	            public int LineId       		    { get; set; }
        [Required]                            [Display(Name = "resources.bus.stop.type")] 	            public NodeType Type       		    { get; set; }

		#region Constructors
		public BusApiStopCreateArguments( string code, string masterCode, string name, string location, decimal? longitude, decimal? latitude, decimal? geofenceRadious, int waitingTime_Seconds, int lineId, NodeType type)
		{ 
			Code = code;
			MasterCode = masterCode;
            Name = name;
			Location = location ?? "";
            Longitude = longitude;
            Latitude = latitude;
            GeofenceRadious = geofenceRadious;
			WaitingTime_Seconds = waitingTime_Seconds;
            LineId = lineId;
            Type = type;
        }
		#endregion Constructors
	}
}
