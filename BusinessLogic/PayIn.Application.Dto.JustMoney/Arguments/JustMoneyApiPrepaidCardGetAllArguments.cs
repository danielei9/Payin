using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.JustMoney.Arguments
{
	public class JustMoneyApiPrepaidCardGetAllArguments : IArgumentsBase
	{
        public string Filter { get; set; }

        #region Constructors
        public JustMoneyApiPrepaidCardGetAllArguments(string filter)
        {
            Filter = filter ?? "";
        }
        #endregion Constructors
    }
}
