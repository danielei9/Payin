using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlFormAssignCheckPoint
{
	public partial class ControlFormAssignCheckPointDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ControlFormAssignCheckPointDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
