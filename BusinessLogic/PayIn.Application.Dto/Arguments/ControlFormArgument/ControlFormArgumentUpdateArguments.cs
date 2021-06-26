using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Xp.Application.Dto;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ControlFormArgument
{
	public class ControlFormArgumentUpdateArguments : IArgumentsBase
	{
		                                                                          public int                       Id           { get; set; }
		[Display(Name = "resources.controlFormArgument.name")]		              public string                    Name         { get; set; }
		[Display(Name = "resources.controlFormArgument.observations")]		      public string                    Observations { get; set; }
		[Display(Name = "resources.enumResources.controlFormArgumentType_")]	  public ControlFormArgumentType   Type         { get; set; }
		[Display(Name = "resources.controlFormArgument.minOptions")]		      public int                       MinOptions   { get; set; }
		[Display(Name = "resources.controlFormArgument.maxOptions")]			  public int?					   MaxOptions	{ get; set; }
		[Display(Name = "resources.controlFormArgument.isRequired")]			  public bool					   Required		{ get; set; }
		[Display(Name = "resources.controlFormArgument.order")]					  public int					   Order		{ get; set; }
		
		#region Constructors
		public ControlFormArgumentUpdateArguments(int id, string name, string observations, ControlFormArgumentType type, int minOptions, int? maxOptions, bool required, int order)
		{
			Id           = id;
			Name         = name;
			Observations = observations;
			Type         = type;
			MinOptions   = minOptions;
			MaxOptions	 = maxOptions;
			Required	 = required;
			Order		 = order;
		}
		#endregion Constructors
	}
}
