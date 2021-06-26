using PayIn.Common;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Results
{
	public partial class ApiControlFormGetResult_Arguments
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Observations { get; set; }
		public ControlFormArgumentType Type { get; set; }
		public ControlFormArgumentTarget Target { get; set; }
		public int MinOptions { get; set; }
		public int? MaxOptions { get; set; }
		public ControlFormArgumentState State { get; set; }

        public IEnumerable<ApiControlFormGetResult_Options> Options { get; set; }
    }
}
