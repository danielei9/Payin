using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class MobileServiceIncidenceGetAllArguments : IArgumentsBase
	{
		public string	Filter			{ get; set; }

		#region Constructors
		public MobileServiceIncidenceGetAllArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
