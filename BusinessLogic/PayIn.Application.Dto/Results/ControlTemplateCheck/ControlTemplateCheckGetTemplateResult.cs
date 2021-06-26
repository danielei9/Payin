using System;
using Xp.Common;


namespace PayIn.Application.Dto.Results
{
	public partial class ControlTemplateCheckGetTemplateResult
	{	
		public int Id { get; set; }
		public XpTime Time { get; set; }
		public int FormsCount { get; set; }
		public string CheckPoint { get; set; }
	}
}
