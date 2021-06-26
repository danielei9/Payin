using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class EntranceTypeGetPursesArguments : IArgumentsBase
    {
		public int Id { get; set; }

		#region Constructors
		public EntranceTypeGetPursesArguments(int id)
		{
			Id = id;
		}
        #endregion Constructors
    }
}
