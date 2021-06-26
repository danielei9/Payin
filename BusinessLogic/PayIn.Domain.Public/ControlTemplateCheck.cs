using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ControlTemplateCheck : IEntity
	{
		public int Id { get; set; }
		public DateTime Time { get; set; }

		#region Template
		public int TemplateId { get; set; }
		public ControlTemplate Template { get; set; }
		#endregion Template

		#region TemplateItemsSince
		[InverseProperty("Since")]
		public ICollection<ControlTemplateItem> ItemsSince { get; set; }
		#endregion TemplateItemsSince

		#region TemplateItemsUntil
		[InverseProperty("Until")]
		public ICollection<ControlTemplateItem> ItemsUntil { get; set; }
		#endregion TemplateItemsUntil

		#region CheckPoint
		public int? CheckPointId { get; set; }
		public ServiceCheckPoint CheckPoint { get; set; }
		#endregion CheckPoint

		#region FormAssignTemplates
		[InverseProperty("Check")]
		public ICollection<ControlFormAssignTemplate> FormAssignTemplates { get; set; }
		#endregion FormAssignTemplates

		#region Constructors
		public ControlTemplateCheck()
		{
			FormAssignTemplates = new List<ControlFormAssignTemplate>();
		}
		#endregion Constructors
	}
}
