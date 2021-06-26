using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xp.Domain;

namespace PayIn.Domain.Public
{
	public class Profile : Entity
	{
		[Required(AllowEmptyStrings = false)]		public string		Name		{ get; set; }
		[Required(AllowEmptyStrings = false)]		public string		Icon		{ get; set; }
		[Required(AllowEmptyStrings = true)]		public string		Color		{ get; set; }
		[Required(AllowEmptyStrings = true)]		public string		ImageUrl	{ get; set; }
		[Required(AllowEmptyStrings = true)]		public string		Layout		{ get; set; }
		[Required(AllowEmptyStrings = true)]		public string		LayoutPro	{ get; set; }
	}
}
