using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlItem
{
	public partial class ControlItemCreateArguments : ICreateArgumentsBase<PayIn.Domain.Public.ControlItem>
	{
		[Display(Name="resources.controlItem.name")]                  [Required] public string Name                  { get; private set; }
		[Display(Name="resources.controlItem.observations")]                     public string Observations          { get; private set; }
		[Display(Name="resources.controlItem.concession")]            [Required] public int    ConcessionId          { get; private set; }
		[Display(Name="resources.controlItem.saveTrack")]                        public bool   SaveTrack             { get; private set; }
		[Display(Name="resources.controlItem.saveFacialRecognition")]            public bool   SaveFacialRecognition { get; private set; }
		[Display(Name="resources.controlItem.checkTimetable")]		             public bool CheckTimetable { get; private set; }
		[Display(Name ="resources.controlItem.trackFrecuency")]         		 public XpDuration TrackFrecuency { get; private set; }


		#region Constructors
		public ControlItemCreateArguments(string name, string observations, int concessionId, bool saveTrack, bool saveFacialRecognition, bool checkTimetable, XpDuration trackFrecuency)
		{
			Name                  = name ?? "";
			Observations          = observations ?? "";
			ConcessionId          = concessionId;
			SaveTrack             = saveTrack;
			SaveFacialRecognition = saveFacialRecognition;
			CheckTimetable        = checkTimetable;
			TrackFrecuency        = trackFrecuency;
		}
		#endregion Constructors
	}
}
