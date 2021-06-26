using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
    public partial class MobileServiceCardUnlinkArguments : IArgumentsBase
	{
		public int Id { get; set; }
		public string LinkedToLogin { get; set; }

		#region Constructors
		public MobileServiceCardUnlinkArguments(int id, string linkedToLogin)
		{
			Id = id;
			LinkedToLogin = linkedToLogin;
		}
		#endregion Constructors
	}
}
