using PayIn.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PayIn.Application.Dto.Results.ControlForm
{
    public class ControlFormGetResult
    {
		public class ControlFormGetResult_Arguments
		{
			public int                       Id           { get; set; }
			public string                    Name         { get; set; }
			public string                    Observations { get; set; }
			public ControlFormArgumentType   Type         { get; set; }
			public ControlFormArgumentTarget Target       { get; set; }
			public bool                      IsRequired   { get; set; }
		}

		public int    Id           { get; set; }
		public string Name         { get; set; }
		public string Observations { get; set; }
		public IEnumerable<ControlFormGetResult_Arguments> Arguments { get; set; }
    }
}
