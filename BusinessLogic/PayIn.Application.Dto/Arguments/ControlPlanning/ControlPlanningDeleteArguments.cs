using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlPlanning
{
	public partial class ControlPlanningDeleteArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public ControlPlanningDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
	}
}
