using PayIn.Domain.Bus.Enums;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Bus.Results
{
	public class BusApiGraphGetAllResult
	{
		public int Id { get; set; }
		public RouteSense Sense { get; set; }

		public IEnumerable<BusApiGraphGetAllResult_Node> Nodes { get; set; }
		public IEnumerable<BusApiGraphGetAllResult_Link> Links { get; set; }
	}
}
