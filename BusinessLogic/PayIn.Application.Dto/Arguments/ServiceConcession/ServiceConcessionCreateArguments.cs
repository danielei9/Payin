using PayIn.Common;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceConcession
{
	public partial class ServiceConcessionCreateArguments : IArgumentsBase
	{
		public int         ConcessionId { get; private set; }
		public string      Name         { get; private set; }
		public ServiceType Type         { get; private set; }
		public int         MaxWorkers   { get; private set; }

		#region Constructors
		public ServiceConcessionCreateArguments(int concessionId, string name, ServiceType type,int maxWorkers) 
		{
			ConcessionId = concessionId;
			Name = name;
			Type = type;
			MaxWorkers = maxWorkers;
		}
		#endregion Constructors
	}
}
