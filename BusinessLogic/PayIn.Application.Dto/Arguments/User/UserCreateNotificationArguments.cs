using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.User
{
	public class UserCreateNotificationArguments : IArgumentsBase
	{
		[Display(Name = "resources.user.allUsers")] 				    public bool AllUsers { get; set; }
		[Display(Name = "resources.user.text")] 		                public string Text { get; set; }
		[Display(Name = "resources.user.users")] 		        		public dynamic Users { get; set; }




		#region Constructors
		public UserCreateNotificationArguments(bool allUsers, string text, dynamic users, bool onlyAndroid, bool onlyIos)
		{
			AllUsers = allUsers;
			Text = text;
			Users = users;		
		}
		#endregion Constructors
	}
}
