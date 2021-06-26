using PayIn.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ServiceZone : IEntity
	{
		                                        public int      Id                { get; set; }
		[Required(AllowEmptyStrings = false)]	public string   Name              { get; set; }
		                                        public decimal?	CancelationAmount { get; set; }

		#region Addresses
		[InverseProperty("Zone")]
		public ICollection<ServiceAddress> Addresses { get; set; }
		#endregion Addresses

		#region Concession
		public int ConcessionId { get; set; }
		public ServiceConcession Concession	{ get; set; }
		#endregion Concession

		#region Prices
		[InverseProperty("Zone")]
		public ICollection<ServicePrice> Prices { get; set; }
		#endregion Prices

		#region TimeTables
		[InverseProperty("Zone")]	
		public ICollection<ServiceTimeTable> TimeTables { get; set; }
		#endregion TimeTables

		//#region Tickets
		//[InverseProperty("Zone")]	
		//public ICollection<Ticket> Tickets { get; set; }
		//#endregion Tickets

		#region Constructors
		public ServiceZone()
		{
			Addresses = new List<ServiceAddress>();
			Prices = new List<ServicePrice>();
			TimeTables = new List<ServiceTimeTable>();
			//Tickets = new List<Ticket>();
		}
		#endregion Constructors
	}
}
