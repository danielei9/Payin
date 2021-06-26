using PayIn.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ServiceFreeDays : IEntity
	{
		                                      public int      Id    { get; set; }
		[Required(AllowEmptyStrings = false)] public string   Name  { get; set; }
		                                      public DateTime From  { get; set; }
		                                      public DateTime Until { get; set; }

		#region Concession
		public int ConcessionId { get; set; }
		public ServiceConcession Concession { get; set; }
		#endregion Concession

		#region Constructors
		public ServiceFreeDays()
		{
		}
		#endregion Constructors
	}
}
