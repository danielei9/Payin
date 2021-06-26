using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ServiceConfigurationData : IEntity
	{
		                                      public int    Id             { get; set; }
		[Required(AllowEmptyStrings = false)] public string AwaitTimeAlert { get; set; }

		#region Constructors
		public ServiceConfigurationData()
		{
		}
		#endregion Constructors
	}
}
