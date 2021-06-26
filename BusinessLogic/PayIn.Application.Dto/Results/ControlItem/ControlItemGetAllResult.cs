using Xp.Common;
namespace PayIn.Application.Dto.Results.ControlItem
{
    public partial class ControlItemGetAllResult
    {
		public int    Id                    { get; set; }
		public string Name                  { get; set; }
		public string Observations          { get; set; }
		public int    EntranceCount         { get; set; }
		public int    ExitCount             { get; set; }
		public int    CheckCount            { get; set; }
		public int    RoundCount		    { get; set; }
		public bool   SaveTrack             { get; set; }
		public bool   SaveFacialRecognition { get; set; }
		public bool   CheckTimetable        { get; set; }		
		
    }
}
