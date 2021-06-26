using Xp.Common;

namespace PayIn.Application.Dto.Results.ControlTemplate
{
	public partial class ControlTemplateGetResult
	{
		public int        Id            { get; set; }
		public string     Name          { get; set; }
		public string     Observations  { get; set; }
		public XpDuration CheckDuration { get; set; }
		public bool       Monday        { get; set; }
		public bool       Tuesday       { get; set; }
		public bool       Wednesday     { get; set; }
		public bool       Thursday      { get; set; }
		public bool       Friday        { get; set; }
		public bool       Saturday      { get; set; }
		public bool       Sunday        { get; set; }
	}
}
