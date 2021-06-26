using PayIn.Application.Dto.Results.ControlItem;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlItem
{
	public partial class ControlItemGetArguments : IGetIdArgumentsBase<ControlItemGetResult, PayIn.Domain.Public.ControlItem>
	{
		public int Id { get; set; }

		#region Constructors
		public ControlItemGetArguments(int id)
		{
			Id = id;
		}
		public ControlItemGetArguments()
		{
		}
		#endregion Constructors
	}
}
