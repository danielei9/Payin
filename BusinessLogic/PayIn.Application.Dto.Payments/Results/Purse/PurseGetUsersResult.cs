using PayIn.Common;
using System;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results.Purse
{
	public partial class PurseGetUsersResult
	{
		public int Id { set; get; }
		public string Login { set; get; }	
		public decimal Amount { set; get; }
	
	}
}
