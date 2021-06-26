using System.Collections.Generic;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
    public class ApiControlFormGetArguments : IArgumentsBase
	{
		public IEnumerable<int> Ids { get; set; }
	}
}
