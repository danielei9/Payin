using PayIn.Common;
using System;

namespace PayIn.Application.Dto.Results.ServiceTag
{
	public class ServiceTagMobileGetResult_Value
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Observations { get; set; }
		public ControlFormArgumentType Type { get; set; }
		public ControlFormArgumentTarget Target { get; set; }
		public bool IsRequired { get; set; }
		public string ValueString { get; set; }
		public decimal? ValueNumeric { get; set; }
		public bool? ValueBool { get; set; }
		public DateTime? ValueDateTime { get; set; }
	}
}
