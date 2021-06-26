using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments
{
    public partial class ApiAccessControlCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.accessControl.name")]
		[Required(AllowEmptyStrings = false)]
		public string Name { get; set; }

		[Display(Name = "resources.accessControl.schedule")]
		public string Schedule { get; set; }

		[Display(Name = "resources.accessControl.maxCapacity")]
		public int MaxCapacity { get; set; }

		[Display(Name = "resources.accessControl.map")]
		public string Map { get; set; }

		#region Constructors

		public ApiAccessControlCreateArguments(string name, string schedule, int maxCapacity)
		{
			Name = name;
			Schedule = schedule;
			MaxCapacity = maxCapacity;
		}

		#endregion
	}
}
