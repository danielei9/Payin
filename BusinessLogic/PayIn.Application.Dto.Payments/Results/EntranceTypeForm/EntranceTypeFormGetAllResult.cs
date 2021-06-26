using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayIn.Application.Dto.Payments.Results
{
	public partial class EntranceTypeFormGetAllResult
	{
		public int Id { get; set; }
		public int FormId { get; set; }
		public int EntranceTypeId { get; set; }
		public int Order { get; set; }
		public string Name { get; set; }
		public string EntranceTypeName { get; set; }
	}
}
