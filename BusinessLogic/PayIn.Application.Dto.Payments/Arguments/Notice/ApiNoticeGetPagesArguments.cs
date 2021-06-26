using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public partial class ApiNoticeGetPagesArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public ApiNoticeGetPagesArguments(string filter)
		{
			Filter = filter ?? "";
        }
		#endregion Constructors
	}
}
