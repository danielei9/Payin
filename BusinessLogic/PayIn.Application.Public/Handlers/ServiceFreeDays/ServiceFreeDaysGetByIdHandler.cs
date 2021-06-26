using PayIn.Application.Dto.Arguments.ServiceFreeDays;
using PayIn.Application.Dto.Results.ServiceFreeDays;
using PayIn.BusinessLogic.Common;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
				public class ServiceFreeDaysGetByIdHandler :
					IQueryBaseHandler<ServiceFreeDaysGetByIdArguments, ServiceFreeDaysGetByIdResult>
				{
								private readonly IEntityRepository<ServiceFreeDays> _Repository;
								private readonly IEntityRepository<ServiceSupplier> _RepositorySupplier;
								private readonly ISessionData _SessionData;

								#region Constructors
								public ServiceFreeDaysGetByIdHandler(IEntityRepository<ServiceFreeDays> repository, IEntityRepository<ServiceSupplier> repositorySupplier, ISessionData sessionData)
								{
												if (repository == null)
																throw new ArgumentNullException("repository");
												_Repository = repository;
												if (repositorySupplier == null)
																throw new ArgumentNullException("repositorySupplier");
												_RepositorySupplier = repositorySupplier;
												if (sessionData == null)
																throw new ArgumentNullException("sessionData");
												_SessionData = sessionData;
								}
								#endregion Constructors

								#region ServiceFreeDaysGetById
								async Task<ResultBase<ServiceFreeDaysGetByIdResult>> IQueryBaseHandler<ServiceFreeDaysGetByIdArguments, ServiceFreeDaysGetByIdResult>.ExecuteAsync(ServiceFreeDaysGetByIdArguments arguments)
								{
												var items = (await _Repository.GetAsync());

												items = items.Where(x => x.Id == arguments.Id);

												var result = items
													.Select(x => new
													{
																	Id = x.Id,
																	Name = x.Name,
																	ConcessionId = x.ConcessionId,
																	ConcessionName = x.Concession.Name,
																	From = x.From,
																	Until = x.Until
													})
													.ToList()
													.Select(x => new ServiceFreeDaysGetByIdResult
													{
																	Id = x.Id,
																	Name = x.Name,
																	ConcessionId = x.ConcessionId,
																	ConcessionName = x.ConcessionName,
																	From = x.From,
																	Until = x.Until
													});
												return new ResultBase<ServiceFreeDaysGetByIdResult> { Data = result };
								}
								#endregion
				}
}
