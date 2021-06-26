using System.Collections.Generic;

namespace PayIn.Application.Dto.Results.ControlPresence
{
	public partial class ControlPresenceMobileGetTagResult
	{
		public int               Id        { get; set; }
		public string            Reference { get; set; }
		public IEnumerable<ControlPresenceMobileGetTagResult_Item> Items     { get; set; }
	}
}
