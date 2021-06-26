using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;
using PayIn.Common;

namespace PayIn.Domain.Internal
{
	public class User : Entity
	{
		[Required(AllowEmptyStrings=false)]	public string     Login      { get; set; }
		[Required(AllowEmptyStrings=false)]	public string     Pin        { get; set; }
		[Required]                          public int        PinRetries { get; set; }
		[Required]                          public UserState  State      { get; set; }
		
		#region PaymentMedias
		[InverseProperty("User")]
		public ICollection<PaymentMedia> PaymentMedias { get; set; } = new List<PaymentMedia>();
		#endregion PaymentMedias
	}
}
