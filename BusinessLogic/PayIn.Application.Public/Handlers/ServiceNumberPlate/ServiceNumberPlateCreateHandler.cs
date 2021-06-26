using PayIn.Application.Dto.Arguments.ServiceNumberPlate;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers.ServiceNumberPlate
{
	public class ServiceNumberPlateCreateHandler :
	IServiceBaseHandler<ServiceNumberPlateCreateArguments>
	{
		private readonly IEntityRepository<PayIn.Domain.Public.ServiceNumberPlate> _Repository;

		#region Constructors
		public ServiceNumberPlateCreateHandler(IEntityRepository<PayIn.Domain.Public.ServiceNumberPlate> repository)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			_Repository = repository;
		}
		#endregion Constructors

		#region ExecuteAsync
		async Task<dynamic> IServiceBaseHandler<ServiceNumberPlateCreateArguments>.ExecuteAsync(ServiceNumberPlateCreateArguments arguments)
		{
			var item = new PayIn.Domain.Public.ServiceNumberPlate
			{
				NumberPlate = arguments.NumberPlate,
				Model = arguments.Model,
				Color = arguments.Color
			};
			await _Repository.AddAsync(item);

			return item;
		}
		#endregion ExecuteAsync
	}
}
