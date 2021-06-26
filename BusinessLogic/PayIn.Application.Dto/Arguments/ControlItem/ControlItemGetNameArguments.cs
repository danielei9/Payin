using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlItem
{
	public partial class ControlItemGetNameArguments : IArgumentsBase
	{
		public int Id { get; private set; }

		#region Constructors
		public ControlItemGetNameArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
