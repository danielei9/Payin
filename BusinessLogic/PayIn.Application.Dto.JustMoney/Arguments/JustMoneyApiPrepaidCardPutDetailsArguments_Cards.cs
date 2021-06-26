using PayIn.Infrastructure.JustMoney.Enums;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.JustMoney.Arguments
{
	public class JustMoneyApiPrepaidCardPutDetailsArguments_Cards
	{
        public int Id { get; set; }
		public string Alias { get; set; }

		#region Constructors
		public JustMoneyApiPrepaidCardPutDetailsArguments_Cards(int id, string alias)
        {
            Id = id;
			Alias = alias;
		}
        #endregion Constructors
    }
}
