using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlItem
{
	public partial class ControlItemGetAllArguments : IGetArgumentsBase<PayIn.Domain.Public.ControlItem>
	{
		public string Filter { get; set; }

		#region Constructors
		public ControlItemGetAllArguments(string filter)
		{
			Filter = filter ?? "";
		}
		#endregion Constructors
	}
}
