using PayIn.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayIn.Application.Dto.Results.ControlFormArgument
{
	public class ControlFormArgumentGetFormResult
	{
		public int							Id				{ get; set; }
		public string						Name			{ get; set; }
		public ControlFormArgumentType		Type			{ get; set; }
		public string						TypeAlias		{ get; set; }
		public int							MinOptions		{ get; set; }
		public int?							MaxOptions		{ get; set; }
		public ControlFormArgumentState		State			{ get; set; }
		public int							Order			{ get; set; }
		public bool							Required		{ get; set; }
	}
}
