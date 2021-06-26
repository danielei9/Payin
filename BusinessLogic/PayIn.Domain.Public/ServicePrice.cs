using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ServicePrice : IEntity
	{
		                              public int      Id    { get; set; }
		[Required()] [Precision(9,3)] public decimal  Price { get; set; }
		[Required()]                  public TimeSpan Time  { get; set; }

		#region Zone
		public int ZoneId { get; set; }
		public ServiceZone Zone { get; set; }
		#endregion Zone
	}
}
