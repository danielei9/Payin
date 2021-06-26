using PayIn.Common;
using System.ComponentModel.DataAnnotations;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments
{
	public partial class ServiceIncidenceUpdateArguments : IArgumentsBase
	{
		public int Id { get; set; }

		[Display(Name = "resources.incidence.state")]
		public IncidenceState State { get; set; }

		[Display(Name = "resources.incidence.observations")]
		public string InternalObservations { get; set; }

		#region Constructors
		public ServiceIncidenceUpdateArguments(int id, IncidenceState state, string internalObservations)
		{
			Id = id;
			State = state;
			InternalObservations = internalObservations;
		}
		#endregion Constructors
	}
}
