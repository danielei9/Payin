using System;

namespace PayIn.Application.Dto.Arguments.ControlPresence
{
	public partial class ControlPresenceMobileCheckArguments_Value
	{
		public int Id { get; set; }
		public string ValueString { get; set; }
		public decimal? ValueNumeric { get; set; }
		public bool? ValueBool { get; set; }
		public DateTime? ValueDateTime { get; set; }
		public byte[] Image { get; set; }
	}
}
