using PayIn.Common;
using System;
using Xp.Common;

namespace PayIn.Application.Dto.Payments.Results.Purse
{
	public partial class PurseGetResult
	{
		public int Id { set; get; }
		public string Name { set; get; }		
		public XpDate Validity { set; get; }
		public XpDate Expiration { set; get; }
		public string PhotoUrl { set; get; }
		

	}
}
