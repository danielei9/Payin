using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.JustMoney.Arguments
{
	public class JustMoneyMobilePrepaidCardPutDetailsArguments : IArgumentsBase
	{
        public int Id { get; set; }
		public string Alias { get; set; }

		#region Constructors
		public JustMoneyMobilePrepaidCardPutDetailsArguments(string alias)
        {
			Alias = alias;
		}
        #endregion Constructors
    }
}
