using System.Collections.Generic;
using Xp.Common;

namespace PayIn.Application.Dto.Results.ControlTemplate
{
	public partial class ControlTemplateGetAllResult
	{
		public int        Id                 { get; set; }
		public string     Name               { get; set; }
		public XpDuration CheckDuration      { get; set; }
		public bool       Monday             { get; set; }
		public bool       Tuesday            { get; set; }
		public bool       Wednesday          { get; set; }
		public bool       Thursday           { get; set; }
		public bool       Friday             { get; set; }
		public bool       Saturday           { get; set; }
		public bool       Sunday             { get; set; }
		public int        ItemId             { get; set; }
		public string     ItemName           { get; set; }
		public int        TemplateItemsCount { get; set; }
		public IEnumerable<ControlTemplateGetAllResult_Item> Items { get; set; }
		public int	     TemplateChecksCount { get; set; }
		public IEnumerable<ControlTemplateGetAllResult_Check> Checks { get; set; }
	}
}
