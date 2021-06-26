using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Internal.Arguments
{
	public class PaymentMediaRechargeArguments : IArgumentsBase
    {
        public int Purse { get; set; }        
        public string Name { get; set; }
        public string Number { get; set; }
        public string BankEntity { get; set; }
        public decimal Quantity { get; set; }

        #region Constructors

        public PaymentMediaRechargeArguments(int Purse,string name,string number,string bankEntity, decimal Quantity)
        {
            this.Purse = Purse;
            this.Name = name;
            this.Number = number;
            this.BankEntity = bankEntity;
            this.Quantity = Quantity;
        }
        #endregion Constructors
    }
}
