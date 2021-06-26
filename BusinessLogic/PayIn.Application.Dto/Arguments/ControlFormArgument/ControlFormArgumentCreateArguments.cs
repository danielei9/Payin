using PayIn.Common;
using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlFormArgument
{
    public class ControlFormArgumentCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.controlFormArgument.name")]                      public string                    Name         { get; set; }
		[Display(Name = "resources.controlFormArgument.observations")]              public string                    Observations { get; set; }
		[Display(Name = "resources.enumResources.controlFormArgumentType_")]	   	public ControlFormArgumentType   Type         { get; set; }
																	                public int                       FormId       { get; set; }
		[Display(Name = "resources.controlFormArgument.minOptions")]				public int						 MinOptions	  { get; set; }
		[Display(Name = "resources.controlFormArgument.maxOptions")]				public int						 MaxOptions   { get; set; }
		[Display(Name = "resources.controlFormArgument.isRequired")]				public bool						 Required	  { get; set; }
		[Display(Name = "resources.controlFormArgument.order")]						public int						 Order		  { get; set; }

		#region Constructors
		public ControlFormArgumentCreateArguments(string name, string observations, ControlFormArgumentType type, int formId, bool required, int minOptions, int maxOptions, int order)
		{
			Name         = name;
			Observations = observations;
			Type         = type;
			FormId       = formId;
			MinOptions	 = minOptions;
			MaxOptions	 = maxOptions;
			Required	 = required;
			Order		 = order;
		}
		#endregion Constructors
    }
}
