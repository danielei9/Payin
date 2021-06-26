using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.SmartCity
{
	public class DataSet : Entity
	{
		public DateTime Timestamp { get; set; }

		#region Component
		public int ComponentId { get; set; }
		[ForeignKey(nameof(ComponentId))]
		public Component Component { get; set; }
		#endregion Component

		#region Datas
		[InverseProperty(nameof(Data.DataSet))]
		public ICollection<Data> Datas { get; set; }
		#endregion Datas

		#region Create
		public static DataSet Create(Component component, DateTime timestamp)
		{
			var item = new DataSet
			{
				Component = component,
				Timestamp = timestamp
			};
			return item;
		}
		#endregion Create
	}
}
