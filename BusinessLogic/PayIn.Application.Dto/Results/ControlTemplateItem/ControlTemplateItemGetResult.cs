using Xp.Common;

namespace PayIn.Application.Dto.Results.ControlTemplateItem
{
	public partial class ControlTemplateItemGetResult
	{
		public int    Id    { get; set; }
		public XpTime Since { get; set; }
		public XpTime Until { get; set; }
	}
}
