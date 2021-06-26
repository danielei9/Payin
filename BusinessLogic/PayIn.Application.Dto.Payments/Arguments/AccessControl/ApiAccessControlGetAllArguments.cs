using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class ApiAccessControlGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors

		public ApiAccessControlGetAllArguments(string filter)
		{
			Filter = filter ?? "";
		}

		#endregion
	}
}
