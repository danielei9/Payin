using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Application.Dto.Transport.Results.TransportCardSupport
{
	public class TransportCardSupportGetResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int OwnerCode { get; set; }
		public string OwnerName { get; set; }
		public int Type { get; set; }
		public int? SubType { get; set; }
		public int State { get; set; }
	}
}
