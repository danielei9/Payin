using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public class MobileEntranceTypeGetPursesArguments : IArgumentsBase
    {
		public long Uid { get; set; }

		#region Constructors
		public MobileEntranceTypeGetPursesArguments(long uid)
		{
			Uid = uid;
		}
        #endregion Constructors
    }
}
