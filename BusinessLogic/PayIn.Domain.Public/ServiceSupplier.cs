using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ServiceSupplier : IEntity
	{
		                                                       public int    Id         { get; set; }
		[Required(AllowEmptyStrings = false)] [MaxLength(200)] public string Login      { get; set; }
		[Required(AllowEmptyStrings = false)]                  public string Name       { get; set; }
		[Required(AllowEmptyStrings = false)]                  public string TaxName    { get; set; }
		[Required(AllowEmptyStrings = false)]                  public string TaxNumber  { get; set; }
		[Required(AllowEmptyStrings = false)]                  public string TaxAddress { get; set; }

		#region PaymentTest
		public bool PaymentTest { get; set; }
		#endregion PaymentTest

		#region Workers
		[InverseProperty("Supplier")]
		public ICollection<ServiceWorker> Workers { get; set; }
		#endregion Workers

		#region Concessions
		[InverseProperty("Supplier")]
		public ICollection<ServiceConcession> Concessions { get; set; }
		#endregion Concessions

		#region CheckPoints
		[InverseProperty("Supplier")]
		public ICollection<ServiceCheckPoint> CheckPoints { get; set; }
		#endregion CheckPoints

		#region Tags
		[InverseProperty("Supplier")]
		public ICollection<ServiceTag> Tags { get; set; }
		#endregion Tags

		#region Constructors
		public ServiceSupplier()
		{
			Concessions = new List<ServiceConcession>();
			CheckPoints = new List<ServiceCheckPoint>();
			Tags = new List<ServiceTag>();
		}
		#endregion Constructors
	}
}
