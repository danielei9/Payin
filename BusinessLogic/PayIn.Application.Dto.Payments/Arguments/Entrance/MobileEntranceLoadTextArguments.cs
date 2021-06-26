using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class MobileEntranceLoadTextArguments : IArgumentsBase
	{
		public string Code { get; set; }

		#region Constructors
		public MobileEntranceLoadTextArguments(string code)
		{
			Code = code;
		}
		#endregion Constructors
	}
}
