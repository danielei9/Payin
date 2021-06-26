using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlForm
{
    public class ControlFormUpdateArguments : IArgumentsBase
    {
		                                                                public int    Id           { get; set; }
		[Display(Name="resources.controlForm.name")]         [Required] public string Name         { get; set; }
		[Display(Name="resources.controlForm.observations")]            public string Observations { get; set; }

		
		#region Constructors
		public ControlFormUpdateArguments(int id, string name, string observations)
		{
			Id = id;
			Name = name;
			Observations = observations;
		}
		#endregion Constructors
	}
}
