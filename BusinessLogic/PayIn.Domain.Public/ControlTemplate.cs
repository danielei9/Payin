using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using Xp.Common;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ControlTemplate : IEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Observations { get; set; }
		public DateTime? CheckDuration { get; set; }
		public bool Monday { get; set; }
		public bool Tuesday { get; set; }
		public bool Wednesday { get; set; }
		public bool Thursday { get; set; }
		public bool Friday { get; set; }
		public bool Saturday { get; set; }
		public bool Sunday { get; set; }

		#region Item
		public int ItemId { get; set; }
		public ControlItem Item { get; set; }
		#endregion Item

		#region TemplateItems
		[InverseProperty("Template")]
		public ICollection<ControlTemplateItem> TemplateItems { get; set; }
		#endregion TemplateItems

		#region Checks
		[InverseProperty("Template")]
		public ICollection<ControlTemplateCheck> Checks { get; set; }
		#endregion Checks

		#region Constructors
		public ControlTemplate()
		{
			TemplateItems = new List<ControlTemplateItem>();
			Checks = new List<ControlTemplateCheck>();
		}
		#endregion Constructors
	}
}
