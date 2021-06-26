using PayIn.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ControlFormArgument : IEntity
	{
												public int							Id				{ get; set; }
		[Required(AllowEmptyStrings = false)]	public string						Name			{ get; set; }
												public string						Observations	{ get; set; }
												public ControlFormArgumentType		Type			{ get; set; }
												public ControlFormArgumentTarget	Target			{ get; set; }
												public int							MinOptions		{ get; set; }
												public int?							MaxOptions		{ get; set; }
												public ControlFormArgumentState		State			{ get; set; }
												public int							Order			{ get; set; }
												public bool							Required		{ get; set; }

		#region Form
		public int FormId { get; set; }
		public ControlForm Form { get; set; }
		#endregion Form

		#region Values
		[InverseProperty("Argument")]
		public ICollection<ControlFormValue> Values { get; set; }
		#endregion Values

		#region Options
		[InverseProperty("Argument")]
		public ICollection<ControlFormOption> Options { get; set; } = new List<ControlFormOption>();
		#endregion Options
	}
}
