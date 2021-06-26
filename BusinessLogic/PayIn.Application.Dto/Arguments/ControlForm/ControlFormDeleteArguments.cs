using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlForm 
{
    public class ControlFormDeleteArguments : IArgumentsBase
    {
		public int Id { get; set; }

		#region Constructors
		public ControlFormDeleteArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors
    }
}
