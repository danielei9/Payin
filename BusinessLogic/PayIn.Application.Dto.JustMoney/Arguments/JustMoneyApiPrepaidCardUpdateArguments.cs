using PayIn.Infrastructure.JustMoney.Enums;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.JustMoney.Arguments
{
	public class JustMoneyApiPrepaidCardUpdateArguments : IArgumentsBase
	{
        public int Id { get; set; }
		public string Login { get; set; }

        #region Constructors
        public JustMoneyApiPrepaidCardUpdateArguments(int id, string login)
        {
            Id = id;
            Login = login;
        }
        #endregion Constructors
    }
}
