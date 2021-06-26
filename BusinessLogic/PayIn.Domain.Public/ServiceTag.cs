using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ServiceTag : IEntity
	{
		           public int            Id        { get; set; }
		[Required] public string         Reference { get; set; }
		           public CheckPointType Type      { get; set; }

		#region CheckPoints
		[InverseProperty("Tag")]
		public ICollection<ServiceCheckPoint> CheckPoints { get; set; }
		#endregion CheckPoints

		#region Supplier
		public int SupplierId { get; set; }
		public ServiceSupplier Supplier { get; set; }
		#endregion Item

		#region Constructors
		public ServiceTag()
		{
			CheckPoints = new List<ServiceCheckPoint>();
		}
		#endregion Constructors
	}
}
