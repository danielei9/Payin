using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceIncidenceGetAllArguments : IArgumentsBase
	{
		public string	Filter			{ get; set; }
		//public int?		ServiceUserId	{ get; set; }

		#region Constructors
		public ServiceIncidenceGetAllArguments(string filter) //, int? serviceUserId)
		{
			Filter = filter ?? "";
			//ServiceUserId = serviceUserId;
		}
		#endregion Constructors
	}
}
