using PayIn.Application.Dto.Arguments.ServiceFreeDays;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
				public class ServiceFreeDaysCreateHandler :
				IServiceBaseHandler<ServiceFreeDaysCreateArguments>
				{
								private readonly IEntityRepository<ServiceFreeDays> _Repository;

								#region Constructors
								public ServiceFreeDaysCreateHandler(IEntityRepository<ServiceFreeDays> repository)
								{
												if (repository == null)
																throw new ArgumentNullException("repository");
												_Repository = repository;
								}
								#endregion Constructors

								#region ExecuteAsync
								async Task<dynamic> IServiceBaseHandler<ServiceFreeDaysCreateArguments>.ExecuteAsync(ServiceFreeDaysCreateArguments arguments)
								{
												var itemServiceFreeDays = new ServiceFreeDays
												{
																ConcessionId = arguments.ConcessionId,
																From = arguments.From,
																Until = arguments.Until,
																Name = arguments.Name,
												};
												await _Repository.AddAsync(itemServiceFreeDays);

												return itemServiceFreeDays;
								}
								#endregion ExecuteAsync
				}
}
