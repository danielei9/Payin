using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ApiAccessControlEntranceGetAllArguments : IArgumentsBase
	{
		public int AccessControlId { get; set; }
		public string Filter { get; set; }

		#region Constructors

		public ApiAccessControlEntranceGetAllArguments(int accessControlId, string filter)
		{
			AccessControlId = accessControlId;
			Filter = filter ?? "";
		}

		#endregion
	}
}
