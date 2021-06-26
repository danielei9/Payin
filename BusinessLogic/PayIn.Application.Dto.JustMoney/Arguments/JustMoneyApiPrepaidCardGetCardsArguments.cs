using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.JustMoney.Arguments
{
	public class JustMoneyApiPrepaidCardGetCardsArguments : IArgumentsBase
	{
        public string Filter { get; set; }

        #region Constructors
        public JustMoneyApiPrepaidCardGetCardsArguments(string filter)
        {
            Filter = filter ?? "";
        }
        #endregion Constructors
    }
}
