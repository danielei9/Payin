using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceSupplier
{
	public class ServiceSupplierUpdateArguments : IArgumentsBase
	{
		                                                                public int Id { get;  set; }
		[Display(Name = "resources.serviceSupplier.login")]		        public string Login { get; set; }
		[Display(Name = "resources.serviceSupplier.name")]		        public string Name { get; set; }
		[Display(Name = "resources.serviceSupplier.taxNumber")]	        public string TaxNumber { get; set; }
		[Display(Name = "resources.serviceSupplier.taxName")]	        public string TaxName { get; set; }
		[Display(Name = "resources.serviceSupplier.taxAddress")]		public string TaxAddress { get; set; }

		#region Constructors
		public ServiceSupplierUpdateArguments(int id, string login, string name, string taxName, string taxNumber, string taxAddress)
		{
			Id = id;
			Login = login;
			Name = name;
			TaxName = taxName;
			TaxNumber = taxNumber;
			TaxAddress = taxAddress;
		}
		#endregion Constructors
	}
}
	 