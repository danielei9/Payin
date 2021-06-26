using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.JustMoney.Arguments
{
	public class JustMoneyApiPrepaidCardGetLogArguments : IArgumentsBase
	{
		public int		SinceDay			{ get; set; }
		public int		SinceMonth		{ get; set; }
        public int		SinceYear		{ get; set; }
		public int		UntilDay			{ get; set; }
		public int		UntilMonth			{ get; set; }
        public int		UntilYear			{ get; set; }

        #region Constructors
        public JustMoneyApiPrepaidCardGetLogArguments(int sinceDay, int sinceMonth, int sinceYear, int untilDay, int untilMonth, int untilYear)
        {
            SinceDay = sinceDay;
            SinceMonth = sinceMonth;
            SinceYear = sinceYear;
            UntilDay = untilDay;
            UntilMonth = untilMonth;
            UntilYear = untilYear;
        }
        #endregion Constructors
    }
}
