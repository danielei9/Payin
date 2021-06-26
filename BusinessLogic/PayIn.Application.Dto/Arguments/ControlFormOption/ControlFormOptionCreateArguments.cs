using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ControlFormOptionCreateArguments : IArgumentsBase
	{
		[Display(Name = "resources.controlFormOption.text")]
		[Required(AllowEmptyStrings = false)]
		public string Text										{ get; set; }

		[Display(Name = "resources.controlFormOption.value")]
        [Required]
        public int Value										{ get; set; }

		public int ArgumentId									{ get; set; }

		#region Constructors
		public ControlFormOptionCreateArguments(string text, int value)
		{
			Text = text;
			Value = value;
		}
		#endregion Constructors
	}
}
