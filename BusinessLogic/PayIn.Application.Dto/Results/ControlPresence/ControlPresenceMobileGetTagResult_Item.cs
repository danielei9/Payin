using PayIn.Common;

namespace PayIn.Application.Dto.Results.ControlPresence
{
	public partial class ControlPresenceMobileGetTagResult_Item
	{
		public int Id { get; set; }
		public PresenceType? PresenceType { get; set; }
		public string Name { get; set; }
		public string Observations { get; set; }
		public bool SaveTrack { get; set; }
		public bool SaveFacialRecognition { get; set; }
	}
}
