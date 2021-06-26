using System.ComponentModel.DataAnnotations;


namespace PayIn.Application.Dto.Security.Arguments
{
	public class AccountConfirmEmailAndDataArguments
	{
		[Display(Name = "resources.security.userId")]		public string userId { get; set; }  
		[Display(Name = "resources.security.code")]			public string code { get; set; }
		[Display(Name = "resources.security.mobile")]		public string mobile { get; set; }
		[Display(Name = "resources.security.password")]		public string password { get; set; }
	}
}
