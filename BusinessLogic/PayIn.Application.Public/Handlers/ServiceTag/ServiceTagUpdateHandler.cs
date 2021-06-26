using PayIn.Application.Dto.Arguments.ServiceTag;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;
namespace PayIn.Application.Public
{
	public class ServiceTagUpdateHandler : IServiceBaseHandler<ServiceTagUpdateArguments>
	{
		private readonly IEntityRepository<ServiceTag> _Repository;
		#region Constructors
		public ServiceTagUpdateHandler(IEntityRepository<ServiceTag> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors
		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceTagUpdateArguments>.ExecuteAsync(ServiceTagUpdateArguments arguments)
		{
			var item = await _Repository.GetAsync(arguments.Id);
			item.Type = arguments.Type;
			item.Reference = arguments.Reference;

			return item;
		}
		#endregion ExecuteAsync
	}
}