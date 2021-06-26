using Xp.Common;

namespace PayIn.Application.Dto.Results.ControlTemplate
{
	public partial class ControlTemplateGetAllResult_Item
	{
		public int Id { get; set; }
		public XpTime Since { get; set; }
		public XpTime Until { get; set; }
	}
}
