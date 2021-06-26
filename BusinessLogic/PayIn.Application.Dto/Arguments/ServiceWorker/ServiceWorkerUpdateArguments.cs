using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceWorker
{
	public class ServiceWorkerUpdateArguments : IArgumentsBase
	{
		                                                                                    public int    Id         { get; set; }
		[Display(Name = "resources.serviceWorker.login")] [DataType(DataType.EmailAddress)] public string Login      { get; set; }
		[Display(Name = "resources.serviceWorker.name")]                                    public string Name       { get; set; }

		#region Constructor
		public ServiceWorkerUpdateArguments(int id, string name, string login)
		{
			Id         = id;
			Name       = name;
			Login      = login;
		}
		#endregion Constructor
	}
}
