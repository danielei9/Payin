using PayIn.Common;
using Xp.Application.Dto;
using Xp.Common;

namespace PayIn.Application.Dto.Results
{
	public partial class ServiceUserGetResult
	{
		public int					Id					{ get; set; }
		public string				Name				{ get; set; }
		public string				LastName			{ get; set; }
		public long?				Phone				{ get; set; }
		public string				Address				{ get; set; }
		public string				Email				{ get; set; }
		//public string				AssertDoc			{ get; set; }
		//public FileDto				AssertDocument		{ get; set; }
		public bool					OwnCard				{ get; set; }
		public string				VatNumber			{ get; set; }
		public long?				Uid					{ get; set; }
		public XpDate				BirthDate			{ get; set; }
		public string				PhotoUrl			{ get; set; }
		public ServiceCardState?	CardState			{ get; set; }
		public string				CardConcessionName	{ get; set; }
		public string				Code				{ get; set; }
		public string				Observations		{ get; set; }
	}
}
