using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public class ControlFormOptionUpdateArguments : IArgumentsBase
    {
        public int Id { get; set; }

        [Display(Name = "resources.controlFormOption.text")]
        [Required(AllowEmptyStrings = false)]
		public string Text { get; set; }

		[Display(Name = "resources.controlFormOption.value")]
        [Required]
        public int Value { get; set; }

		#region Constructors
		public ControlFormOptionUpdateArguments(string text, int value, int id)
		{
			Text = text;
			Value = value;
			Id = id;
		}
		#endregion Constructors
	}
}
