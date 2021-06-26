using System;

namespace PayIn.Application.Dto.Security.Results
{
	public class AccountGetUsersResult
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }		
		public string TaxNumber { get; set; }		
		public int FailedCount { get; set; }
		public bool Block { get; set; }
		public DateTime? BlockDate { get; set; }
		public bool EmailConfirmed { get; set; }
		public string Phone { get; set; }
	}
}