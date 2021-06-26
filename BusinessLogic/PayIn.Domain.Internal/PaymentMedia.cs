using PayIn.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Internal
{
	public class PaymentMedia : Entity
	{
		// Attributes
		                                      public PaymentMediaType    Type			 { get; set; }
		[Required(AllowEmptyStrings = true)]  public string              Name			 { get; set; }
		[Required(AllowEmptyStrings = true)]  public string              Number			 { get; set; }
		                                      public int?                ExpirationMonth { get; set; }
		                                      public int?                ExpirationYear	 { get; set; }
		                                      public int                 PublicId		 { get; set; }
		                                      public PaymentMediaState   State			 { get; set; }
		[Required(AllowEmptyStrings = true)]  public string              BankEntity		 { get; set; }
		[Required(AllowEmptyStrings = true)]  public string              Reference		 { get; set; }
		                                      public decimal?            Balance		 { get; set; }
		[Required(AllowEmptyStrings = true)]  public string				 Login           { get; set; }

		#region User
		public int  UserId { get; set; }
		public User	User { get; set; }
		#endregion User
	}
}
