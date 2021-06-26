using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class PaymentConcessionPurseGetAllArguments : IArgumentsBase
	{
		public int Id { get; set; }

		#region Constructors
		public PaymentConcessionPurseGetAllArguments(int id)
		{
			Id = id;
		}
		public PaymentConcessionPurseGetAllArguments()
		{
		}
		#endregion Constructors
	}
}
