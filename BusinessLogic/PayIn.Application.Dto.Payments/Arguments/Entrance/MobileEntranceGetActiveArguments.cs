using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileEntranceGetActiveArguments : IArgumentsBase
    {
        public long Uid { get; set; }

		#region Constructors
		public MobileEntranceGetActiveArguments(long uid)
		{
			Uid = uid;
		}
		#endregion Constructors
	}
}