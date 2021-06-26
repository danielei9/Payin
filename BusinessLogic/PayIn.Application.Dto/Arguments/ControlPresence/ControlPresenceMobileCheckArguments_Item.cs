using System.Collections.Generic;

namespace PayIn.Application.Dto.Arguments.ControlPresence
{
	public partial class ControlPresenceMobileCheckArguments_Item
	{
		public int Id { get; set; }
		public int CheckPointId { get; set; }
		public int? CheckId { get; set; }
		public IEnumerable<ControlPresenceMobileCheckArguments_Assign> Assigns { get; set; }
	}
}
