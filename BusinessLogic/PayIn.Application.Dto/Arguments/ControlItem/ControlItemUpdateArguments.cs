using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlItem
{
	public partial class ControlItemUpdateArguments : IUpdateArgumentsBase<PayIn.Domain.Public.ControlItem>
	{
		                                                              [Required] public int        Id                    { get; set; }
		[Display(Name="resources.controlItem.name")]                  [Required] public string     Name                  { get; private set; }
		[Display(Name="resources.controlItem.observations")]                     public string     Observations          { get; private set; }
		[Display(Name="resources.controlItem.saveTrack")]                        public bool       SaveTrack             { get; private set; }
		[Display(Name="resources.controlItem.saveFacialRecognition")]            public bool       SaveFacialRecognition { get; private set; }
		[Display(Name="resources.controlItem.checkTimetable")]                   public bool       CheckTimetable        { get; private set; }
		[Display(Name="resources.controlItem.trackFrecuency")]         	         public XpDuration TrackFrecuency        { get; private set; }


		#region Constructors
		public ControlItemUpdateArguments(int id, string name, string observations, bool saveTrack, bool saveFacialRecognition, bool checkTimetable, XpDuration trackFrecuency)
		{
			Id                    = Id;
			Name                  = name;
			Observations          = observations;
			SaveTrack             = saveTrack;
			SaveFacialRecognition = saveFacialRecognition;
			CheckTimetable        = checkTimetable;
			TrackFrecuency        = trackFrecuency;
		}
		#endregion Constructors
	}
}
