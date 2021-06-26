using System;
using System.Collections.Generic;
using System.Text;
using PayIn.Common;

namespace PayIn.Application.Dto.Results.ControlForm
{
    public class ControlFormGetAllResult
    {
		public int    Id                { get; set; }
		public string Name              { get; set; }
		public string Observations      { get; set; }
		public ControlFormState State	{ get; set; }
		public int NumArguments			{ get; set; }
	}
}
