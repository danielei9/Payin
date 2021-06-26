using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.JustMoney.Arguments
{
	public class JustMoneyMobilePrepaidCardGetArguments : IArgumentsBase
	{
        public int Id { get; set; }

        #region Constructors
        public JustMoneyMobilePrepaidCardGetArguments(int id)
        {
            Id = id;
        }
        #endregion Constructors
    }
}
