using PayIn.Common;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results
{
	public partial class ApiControlFormGetResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Observations { get; set; }

		public IEnumerable<ApiControlFormGetResult_Arguments> Arguments { get; set; }
	}
}
