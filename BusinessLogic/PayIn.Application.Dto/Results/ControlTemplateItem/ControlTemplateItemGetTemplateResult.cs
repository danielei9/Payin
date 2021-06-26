using System;
using System.ComponentModel.DataAnnotations;
using Xp.Common;

namespace PayIn.Application.Dto.Results.ControlTemplateItem
{
	public partial class ControlTemplateItemGetTemplateResult
	{
		public int    Id			  { get; set; }
		public XpTime Since			  { get; set; }
		public XpTime Until			  { get; set; }
		public int    SinceFormsCount { get; set; }
		public int    UntilFormsCount { get; set; }
		public int    SinceId		  { get; set; }
		public int	  UntilId		  { get; set; }
	}
}
