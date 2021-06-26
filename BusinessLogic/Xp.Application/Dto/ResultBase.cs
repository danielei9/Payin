using System.Collections.Generic;

namespace Xp.Application.Dto
{
	public class ResultBase<TResult>
	{
		public IEnumerable<TResult> Data { get; set; }
	}
}
