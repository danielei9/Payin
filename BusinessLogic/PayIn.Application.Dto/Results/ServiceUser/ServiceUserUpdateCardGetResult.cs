using PayIn.Common;

namespace PayIn.Application.Dto.Results
{
	public partial class ServiceUserUpdateCardGetResult
	{
		public int Id                       { get; set; }
		public long Uid                     { get; set; }
		public ServiceCardState CardState   { get; set; }
		public string NewCardConcessionName { get; set; }
		public bool OwnCard                 { get; set; }
		public bool NewCard                 { get; set; }
		public string OwnerVatNumber        { get; set; }
		public string OwnerName             { get; set; }
		public string OwnerLastName         { get; set; }
	}
}
