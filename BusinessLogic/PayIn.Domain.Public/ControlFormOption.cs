using PayIn.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
    public class ControlFormOption : Entity
	{
		[Required(AllowEmptyStrings = false)]	public string					Text		{ get; set; }
												public int						Value		{ get; set; }
												public ControlFormOptionState	State		{ get; set; }

		#region Argument
		public int ArgumentId { get; set; }
		[ForeignKey("ArgumentId")]
		public ControlFormArgument Argument { get; set; }
		#endregion Argument

		#region Value
		[InverseProperty("ValueOptions")]
		public ICollection<ControlFormValue> Values { get; set; } = new List<ControlFormValue>();
		#endregion Value
	}
}
