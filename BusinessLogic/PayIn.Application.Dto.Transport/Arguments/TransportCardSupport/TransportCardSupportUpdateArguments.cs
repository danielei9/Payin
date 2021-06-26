using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Transport.Arguments.TransportCardSupport
{
	public class TransportCardSupportUpdateArguments : IArgumentsBase
	{
		public int Id { get; set; }
		[Display(Name = "resources.transportCardSupport.name")]
		[Required]
		public string Name { get; set; }		
		[Display(Name = "resources.transportCardSupport.ownerName")]
		[Required]
		public string OwnerName { get; set; }
		[Display(Name = "resources.transportCardSupport.ownerCode")]
		[Required]
		public int OwnerCode { get; set; }
		[Display(Name = "resources.transportCardSupport.type")]
		[Required]
		public int Type { get; set; }
		[Display(Name = "resources.transportCardSupport.subType")]
		public int? SubType { get; set; }

		#region Constructors
		public TransportCardSupportUpdateArguments(int id, string name, int ownerCode, string ownerName, int type, int subType)
		{
			Id = Id;
			Name = name;
			OwnerCode = ownerCode;
			OwnerName = ownerName;
			Type = type;
			SubType = subType;
		}
		#endregion Constructors
	}
}
