using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class ServiceNumberPlate : IEntity
	{
		                                      public int    Id          { get; set; }
		[Required(AllowEmptyStrings = false)] public string NumberPlate { get; set; }
		[Required(AllowEmptyStrings = false)] public string Model       { get; set; }
		[Required(AllowEmptyStrings = false)] public string Color       { get; set; }

		#region Constructors
		public ServiceNumberPlate()
		{
		}
		#endregion Constructors
	}
}
