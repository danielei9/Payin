using PayIn.Common;

namespace PayIn.Application.Dto.Results
{
	public partial class ServiceUserCreateCardGetResult
	{
		public bool OwnCard { get; set; }
		public long Uid { get; set; }
		public string Name { get; set; }
		public string LastName { get; set; }
		public string VatNumber { get; set; }
		public string Photo { get; set; }
		public int CardId { get; set; }
		public ServiceCardState CardState { get; set; }
		public int SystemCardId { get; set; }
		public string SystemCardName { get; set; }
		public string ConcessionName { get; set; }
		public bool NewCard { get; set; }
	}
}
