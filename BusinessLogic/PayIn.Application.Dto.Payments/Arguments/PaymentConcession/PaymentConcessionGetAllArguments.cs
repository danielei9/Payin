using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public partial class PaymentConcessionGetAllArguments : IArgumentsBase
    {
        public string Filter { get; set; }

        #region Constructors
        public PaymentConcessionGetAllArguments(string filter)
        {
            Filter = filter ?? "";
        }
        #endregion Constructors
    }

}

