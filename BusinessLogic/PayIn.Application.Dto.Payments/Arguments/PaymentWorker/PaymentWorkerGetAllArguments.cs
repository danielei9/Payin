using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentWorkerGetAllArguments :IArgumentsBase
	{
        public string Filter { get; set; }

        #region Contructors
        public PaymentWorkerGetAllArguments(string filter)
        {
            Filter = filter;
        }
        #endregion Contructors
    }
}
