using System.Collections.Generic;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
    public class MobileControlFormGetArguments : IArgumentsBase
	{
		public IEnumerable<int> Ids { get; set; }
	}
}
