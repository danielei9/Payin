using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public partial class PaymentGetAllByCardArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public PaymentGetAllByCardArguments(int id)
		{
			Id = id;
		}
		#endregion Constructors 
	}
}
