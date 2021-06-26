using PayIn.Common.Resources;
using System;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceFreeDays
{
	public class ServiceFreeDaysCreateArguments : IArgumentsBase
	{
		[Required]                                                                                 public string   Name         { get; private set; }
		[Required]                                                                                 public DateTime From         { get; private set; }
		[Required]                                                                                 public DateTime Until        { get; private set; }
		[Required] [Display(Name = "resources.serviceFreeDays.concession")]                        public int      ConcessionId { get; private set; }

		#region Constructors
		public ServiceFreeDaysCreateArguments(string name, DateTime from, DateTime until, int concessionId)
		{
			Name = name;
			From = from;
			Until = until;
			ConcessionId = concessionId;
		}
		#endregion Constructors
	}
}
