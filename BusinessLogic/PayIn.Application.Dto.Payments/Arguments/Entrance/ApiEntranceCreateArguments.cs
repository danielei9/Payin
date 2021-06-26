using System;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
	public class ApiEntranceCreateArguments : IArgumentsBase
	{
		public int EntranceTypeId { get; set; }
		public decimal Quantity { get; set; }
		public decimal Amount { get; set; }
		public string TaxNumber { get; set; }
		public string TaxName { get; set; }
		public string Email { get; set; }
		public string Login { get; set; }
		public long? Uid { get; set; }
		public bool Payed { get; set; }
		public DateTime Now { get; set; }

		#region Constructor
		public ApiEntranceCreateArguments(
			int entranceTypeId,
			decimal quantity, decimal amount,
			string taxNumber, string taxName, string email, string login,
			long? uid, bool payed,
			DateTime now
		)
		{
			EntranceTypeId = entranceTypeId;
			Quantity = quantity;
			Amount = amount;
			TaxNumber = taxNumber;
			TaxName = taxName;
			Email = email;
			Login = login;
			Uid = uid;
			Payed = payed;
			Now = now;
		}
		#endregion Constructor
	}
}
