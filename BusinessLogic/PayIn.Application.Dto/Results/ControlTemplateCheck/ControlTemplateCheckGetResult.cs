using Xp.Common;

namespace PayIn.Application.Dto.Results.ControlTemplateCheck
{
	public partial class ControlTemplateCheckGetResult
	{
		public int Id { get; set; }
		public XpTime Time { get; set; }
		public string CheckPoint { get; set; }
	}
}
