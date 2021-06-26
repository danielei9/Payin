using PayIn.Application.Dto.Arguments.ServiceNumberPlate;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers.ServiceNumberPlate
{
	public class ServiceNumberPlateUpdateHandler :
		IServiceBaseHandler<ServiceNumberPlateUpdateArguments>
	{
		private readonly IEntityRepository<PayIn.Domain.Public.ServiceNumberPlate> _Repository;

		#region Constructors
		public ServiceNumberPlateUpdateHandler(IEntityRepository<PayIn.Domain.Public.ServiceNumberPlate> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceNumberPlateUpdateArguments>.ExecuteAsync(ServiceNumberPlateUpdateArguments arguments)
		{
			var item = await _Repository.GetAsync(arguments.Id);
			item.NumberPlate = arguments.NumberPlate;
			item.Model = arguments.Model;
			item.Color = arguments.Color;

			return item;
		}
		#endregion ExecuteAsync
	}
}
