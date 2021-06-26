using PayIn.Application.Dto.Arguments.ControlPresence;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ControlPresenceDeleteHandler :
		IServiceBaseHandler<ControlPresenceDeleteArguments>
	{
		private readonly IEntityRepository<ControlPresence> Repository;

		#region Constructors
		public ControlPresenceDeleteHandler(IEntityRepository<ControlPresence> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			Repository = repository;
		}
		#endregion Constructors

		#region ControlPresenceDelete
		async Task<dynamic> IServiceBaseHandler<ControlPresenceDeleteArguments>.ExecuteAsync(ControlPresenceDeleteArguments arguments)
		{
			var item = await Repository.GetAsync(arguments.Id);
			await Repository.DeleteAsync(item);

			return null;
		}
		#endregion ControlPresenceDelete
	}
}
