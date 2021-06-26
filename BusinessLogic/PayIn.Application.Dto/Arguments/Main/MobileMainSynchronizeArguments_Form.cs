using System.Collections.Generic;

namespace PayIn.Application.Dto.Arguments
{
	public class MobileMainSynchronizeArguments_Form
	{
		public int Id { get; set; }

		public IEnumerable<MobileMainSynchronizeArguments_FormArgument> Arguments { get; set; }
	}
}
