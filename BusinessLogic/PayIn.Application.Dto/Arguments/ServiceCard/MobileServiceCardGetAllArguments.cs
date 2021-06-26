using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class MobileServiceCardGetAllArguments : IArgumentsBase
	{
		public string Filter { get; set; }

		#region Constructors
		public MobileServiceCardGetAllArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
