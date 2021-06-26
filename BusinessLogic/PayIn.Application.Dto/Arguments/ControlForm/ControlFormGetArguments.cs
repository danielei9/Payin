using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlForm
{
    public class ControlFormGetArguments : IArgumentsBase
    {
		public int Id { get; set; }

		
		#region Constructors
		public ControlFormGetArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
    }
}
