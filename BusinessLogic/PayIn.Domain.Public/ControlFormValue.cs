using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;


namespace PayIn.Domain.Public
{
	public class ControlFormValue : Entity
	{
		                                      public string                    Observations  { get; set; }
		                                      public ControlFormArgumentTarget Target        { get; set; }
		                                      public string                    ValueString   { get; set; }
		                                      public decimal?                  ValueNumeric  { get; set; }
		                                      public bool?                     ValueBool     { get; set; }
		                                      public DateTime?                 ValueDateTime { get; set; }

		#region Assign
		public int? AssignId { get; set; }
        [ForeignKey("AssignId")]
		public ControlFormAssign Assign { get; set; }
		#endregion Assign

		#region Argument
		public int ArgumentId { get; set; }
		public ControlFormArgument Argument { get; set; }
		#endregion Argument

		#region Option
		public ICollection<ControlFormOption> ValueOptions { get; set; } = new List<ControlFormOption>();
		#endregion Option
	}
}
