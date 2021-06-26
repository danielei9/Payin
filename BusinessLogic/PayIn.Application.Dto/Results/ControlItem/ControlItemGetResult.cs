using System;
using Xp.Common;
namespace PayIn.Application.Dto.Results.ControlItem
{
	public partial class ControlItemGetResult
	{
		public int        Id                    { get; set; }
		public string     Name                  { get; set; }
		public string     Observations          { get; set; }
		public bool       SaveTrack             { get; set; }
		public bool       SaveFacialRecognition { get; set; }
		public bool       CheckTimetable        { get; set; }
		public XpDuration TrackFrecuency        { get; set; }
	}
}
