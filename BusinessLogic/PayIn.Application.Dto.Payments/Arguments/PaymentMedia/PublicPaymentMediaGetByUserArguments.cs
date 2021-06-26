using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PublicPaymentMediaGetByUserArguments: IArgumentsBase
	{
		public string Login { get; set; }

		#region Constructors
		public PublicPaymentMediaGetByUserArguments(string login)
		{
			Login = login;
		}
		#endregion Constructors
	}
}
