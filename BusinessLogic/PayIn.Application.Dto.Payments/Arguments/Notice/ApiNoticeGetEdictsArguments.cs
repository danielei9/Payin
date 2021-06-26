using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public partial class ApiNoticeGetEdictsArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ApiNoticeGetEdictsArguments(string filter)
		{
			Filter = filter ?? "";
        }
		#endregion Constructors
	}
}
