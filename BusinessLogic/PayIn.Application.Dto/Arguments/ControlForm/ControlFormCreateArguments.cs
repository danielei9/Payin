using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlForm
{
	public class ControlFormCreateArguments : IArgumentsBase
	{
		[Display(Name="resources.controlForm.name")]          [Required] public string Name           { get; set; }
		[Display(Name="resources.controlItem.observations")]             public string Observations   { get; set; }
		[Display(Name="resources.controlItem.concession")]    [Required] public int    ConcessionId   { get; set; }

		#region Constructors
		public ControlFormCreateArguments(string name, string observations, int concessionId)
		{
			Name = name;
			Observations = observations;
			ConcessionId = concessionId;
		}
		#endregion Constructors
	}
}
