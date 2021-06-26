using PayIn.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;
using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;

namespace PayIn.Application.Dto.Arguments.ServiceConcession
{
	public class ServiceConcessionUpdateCommerceArguments : IArgumentsBase
	{
		                                                            public int             Id         { get; set; }
	[Display(Name="resources.serviceConcession.name")]              public string          Name       { get; set; }
	[Display(Name="resources.serviceConcession.maxWorkers")]        public int             MaxWorkers { get; set; }
	[Display(Name="resources.enumResources.concessionState_")]      public ConcessionState State      { get; set; }
	[Display(Name="resources.enumResources.serviceType_")]        public ServiceType     Type       { get; set; }
	}
}
