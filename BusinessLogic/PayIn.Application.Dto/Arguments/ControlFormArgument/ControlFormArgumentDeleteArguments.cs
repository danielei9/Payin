using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlFormArgument
{
    public class ControlFormArgumentDeleteArguments : IArgumentsBase
    {
		public int Id { get; set; }

		#region Constructors
		public ControlFormArgumentDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
    }
}
