using System;
using System.Collections.Generic;

namespace PayIn.Application.Dto.Payments.Results
{
    public partial class ApiAccessControlGetPlacesResult
	{
		public int Id						{ get; set; }
		public string Name					{ get; set; }
		public string Schedule				{ get; set; }
		public int CurrentCapacity			{ get; set; }
		public int MaxCapacity				{ get; set; }
		public string Map					{ get; set; }
		public DateTime DateTime			{ get; set; }
		public IEnumerable<dynamic> Stats	{ get; set; }
	}
}