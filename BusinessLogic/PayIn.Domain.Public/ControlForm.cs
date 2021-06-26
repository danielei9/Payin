using PayIn.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ControlForm : IEntity
	{
												public int					Id					{ get; set; }
		[Required(AllowEmptyStrings = false)]	public string				Name				{ get; set; }
												public string				Observations		{ get; set; }
												public ControlFormState		State				{ get; set; }

		#region Concession
		public int ConcessionId { get; set; }
		public ServiceConcession Concession { get; set; }
		#endregion Concession

		#region Arguments
		[InverseProperty("Form")]
		public ICollection<ControlFormArgument> Arguments { get; set; }
		#endregion Arguments

		#region Assigns
		[InverseProperty("Form")]
		public ICollection<ControlFormAssign> Assigns { get; set; }
		#endregion Assigns

		#region AssignTemplates
		[InverseProperty("Form")]
		public ICollection<ControlFormAssignTemplate> AssignTemplates { get; set; }
		#endregion Assigns

		#region Constructors
		public ControlForm()
		{
			Arguments = new List<ControlFormArgument>();
			Assigns = new List<ControlFormAssign>();
			AssignTemplates = new List<ControlFormAssignTemplate>(); 
		}
		#endregion Constructors
	}
}
