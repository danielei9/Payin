using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileEntranceLoadQrArguments : IArgumentsBase
	{
		public string Code { get; set; }

		#region Constructors
		public MobileEntranceLoadQrArguments(string code)
		{
			Code = code;
		}
		#endregion Constructors
	}
}
