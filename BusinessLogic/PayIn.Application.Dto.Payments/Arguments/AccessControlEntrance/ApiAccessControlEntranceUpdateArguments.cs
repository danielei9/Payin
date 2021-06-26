using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public partial class ApiAccessControlEntranceUpdateArguments : IArgumentsBase
	{
		[JsonIgnore] public int Id { get; set; }

		[Display(Name = "resources.accessControl.entrance.name")]
		[Required(AllowEmptyStrings = false)]
		public string Name { get; set; }

		#region Constructors

		public ApiAccessControlEntranceUpdateArguments(int id, string name)
		{
			Id = id;
			Name = name;
		}

		#endregion
	}
}
