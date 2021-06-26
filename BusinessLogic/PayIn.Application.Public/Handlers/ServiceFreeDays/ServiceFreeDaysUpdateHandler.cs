using PayIn.Application.Dto.Arguments.ServiceFreeDays;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
				public class ServiceFreeDaysUpdateHandler :
				IServiceBaseHandler<ServiceFreeDaysUpdateArguments>
				{
								private readonly IEntityRepository<ServiceFreeDays> _Repository;

								#region Constructors
								public ServiceFreeDaysUpdateHandler(IEntityRepository<ServiceFreeDays> repository)
								{
												if (repository == null)
																throw new ArgumentNullException("repository");
												_Repository = repository;
								}
								#endregion Constructors

								#region ExecuteAsync
								async Task<dynamic> IServiceBaseHandler<ServiceFreeDaysUpdateArguments>.ExecuteAsync(ServiceFreeDaysUpdateArguments arguments)
								{
												var itemServiceFreeDays = await _Repository.GetAsync(arguments.Id);
												itemServiceFreeDays.ConcessionId = arguments.ConcessionId;
												itemServiceFreeDays.From = arguments.From;
												itemServiceFreeDays.Until = arguments.Until;
												itemServiceFreeDays.Name = arguments.Name;

												return itemServiceFreeDays;
								}
								#endregion ExecuteAsync
				}
}
