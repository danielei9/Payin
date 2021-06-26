using PayIn.Common;

namespace PayIn.Application.Dto.Results
{
	public class ServiceUserGetAllByDashBoardResult
	{
		public int					Id			   { get; set; }
		public long					Uid			   { get; set; }
		public string				Name		   { get; set; }
		public string				LastName	   { get; set; }
		public string				VatNumber	   { get; set; }
		public ServiceUserState		State		   { get; set; }
		public int                  LastDay        { get; set; }
		public int                  LastWeek       { get; set; }
		public int                  LastMonth      { get; set; }
		public int                  LastYear       { get; set; }
	}
}
