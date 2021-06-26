using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PublicEntranceGetByUserArguments: IArgumentsBase
	{
		/// <summary>
		/// Usuario del que se quieren ver sus entradas
		/// </summary>
		public string Login { get; set; }

		#region Constructors
		public PublicEntranceGetByUserArguments(string login)
		{
			Login = login;
		}
		#endregion Constructors
	}
}
