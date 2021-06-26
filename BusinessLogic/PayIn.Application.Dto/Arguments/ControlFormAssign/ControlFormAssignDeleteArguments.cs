using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlFormAssign
{
	public partial class ControlFormAssignDeleteArguments : IArgumentsBase
  {
		public int Id { get; set; }

		#region Constructors
		public ControlFormAssignDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
