using PayIn.Common;
using System;

namespace PayIn.Application.Dto.Results.Main
{
	public partial class MainMobileGetAllResult
	{
		public int               Id              { get; set; }
		public string            Title           { get; set; }
		public int?              VisualOrder     { get; set; }
		public string            NumberHash      { get; set; }
		public int?              ExpirationMonth { get; set; }
		public int?              ExpirationYear  { get; set; }
		public string            BankEntity      { get; set; }
		public string            ImagePath       { get; set; }
		public PaymentMediaState State           { get; set; }
		public PaymentMediaType  Type            { get; set; }
	}
}
