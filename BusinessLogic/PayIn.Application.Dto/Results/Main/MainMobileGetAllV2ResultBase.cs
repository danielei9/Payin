using Xp.Application.Dto;

namespace PayIn.Application.Dto.Results.Main
{
    public partial class MainMobileGetAllV2ResultBase : ResultBase<MainMobileGetAllV2Result>
	{
		public int                   NumNotifications { get; set; }
		public int					 NumReceipts	  { get; set; }
		public string                AppVersion       { get; set; }
		public bool                  WorkerHasConcession { get; set; }
		public bool                  UserHasConcession { get; set; }
	}
}
