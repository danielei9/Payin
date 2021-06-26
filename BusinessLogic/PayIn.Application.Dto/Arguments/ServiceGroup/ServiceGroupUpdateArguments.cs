using PayIn.Common;
using PayIn.Domain.Public;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xp.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceGroupUpdateArguments : IUpdateArgumentsBase<ServiceCategory>
	{
		public int	Id						{ get; set; }
		[Display(Name = "resources.serviceGroup.name")]
		[Required(AllowEmptyStrings = false)]
		public string Name					{ get; set; }

		#region Constructors
		public ServiceGroupUpdateArguments(int id, string name)
		{
			Id = id;
			Name = name;
		}
		#endregion Constructos
	}
}
