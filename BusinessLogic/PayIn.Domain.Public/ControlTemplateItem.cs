using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ControlTemplateItem : IEntity
	{	
		public int      Id    { get; set; }

		#region Since
		public int SinceId { get; set; }
		public ControlTemplateCheck Since { get; set; }
		#endregion Since

		#region Until
		public int UntilId { get; set; }
		public ControlTemplateCheck Until { get; set; }
		#endregion Until

		#region Template
		public int TemplateId { get; set; }
		public ControlTemplate Template { get; set; }
		#endregion Template
	}
}
