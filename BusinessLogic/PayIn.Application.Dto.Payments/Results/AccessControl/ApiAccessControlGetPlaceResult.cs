using System;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Payments.Results
{
    public partial class ApiAccessControlGetPlaceResult
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public DateTime DateTime { get; set; }
		public int CurrentCapacity { get; set; }
		public int MaxCapacity { get; set; }
		public IEnumerable<dynamic> Entries { get; set; }
	}
}