using PayIn.Common;
using PayIn.Common.Resources;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceConcession
{
	public partial class ServiceConcessionUpdateArguments : IUpdateArgumentsBase<PayIn.Domain.Public.ServiceConcession>
	{
		                                                                public int             Id         { get; private set; }
		[Display(Name = "resources.serviceConcession.name")]		    public string          Name       { get; private set; }
		                                                                public ServiceType     Type       { get; private set; }
		[Display(Name = "resources.serviceConcession.maxWorkers")]		public int             MaxWorkers { get; private set; }
		[Display(Name = "resources.enumResources.concessionState_")]	public ConcessionState State      { get; private set; }
	

		
		#region Constructors
		public ServiceConcessionUpdateArguments(int id, string name, ServiceType type,int maxWorkers, ConcessionState state) 
		{
			Id = id;
			Name = name;
			Type = type;
			MaxWorkers = maxWorkers;
			State = state;			
		}
		#endregion Constructors
	}
}
