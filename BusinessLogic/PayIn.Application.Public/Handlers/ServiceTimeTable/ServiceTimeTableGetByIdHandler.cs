using PayIn.Application.Dto.Arguments.ServiceTimeTable;
using PayIn.Application.Dto.Results.ServiceTimeTable;
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
				public class ServiceTimeTableGetByIdHandler :
					IQueryBaseHandler<ServiceTimeTableGetByIdArguments, ServiceTimeTableGetByIdResult>
				{
								private readonly IEntityRepository<ServiceTimeTable> _Repository;
								private readonly IEntityRepository<ServiceSupplier> _RepositorySupplier;
								private readonly ISessionData _SessionData;

								#region Constructors
								public ServiceTimeTableGetByIdHandler(IEntityRepository<ServiceTimeTable> repository, IEntityRepository<ServiceSupplier> repositorySupplier, ISessionData sessionData)
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

								#region ServiceTimeTableGetById
								async Task<ResultBase<ServiceTimeTableGetByIdResult>> IQueryBaseHandler<ServiceTimeTableGetByIdArguments, ServiceTimeTableGetByIdResult>.ExecuteAsync(ServiceTimeTableGetByIdArguments arguments)
								{
												var items = (await _Repository.GetAsync());

												items = items.Where(x => x.Id == arguments.Id);

												var result = items
													.Select(x => new
													{
																	Id = x.Id,
																	FromHour = x.FromHour,
																	DurationHour = x.DurationHour,
																	FromWeekday = x.FromWeekday,
																	FromWeekdayLabel = x.FromWeekday.ToString(),
																	UntilWeekday = x.UntilWeekday,
																	UntilWeekdayLabel = x.UntilWeekday.ToString(),
																	ZoneId = x.ZoneId,
																	ZoneName = x.Zone.Name,
																	ConcessionId = x.Zone.ConcessionId,
																	ConcessionName = x.Zone.Concession.Name,
													})
													.ToList()
													.Select(x => new ServiceTimeTableGetByIdResult
													{
																	Id = x.Id,
																	FromHour = x.FromHour,
																	DurationHour = x.DurationHour,
																	UntilHour = x.FromHour + x.DurationHour,
																	FromWeekday = x.FromWeekday,
																	FromWeekdayLabel = x.FromWeekday.ToString(),
																	UntilWeekday = x.UntilWeekday,
																	UntilWeekdayLabel = x.UntilWeekday.ToString(),
																	ZoneId = x.ZoneId,
																	ZoneName = x.ZoneName,
																	ConcessionId = x.ConcessionId,
																	ConcessionName = x.ConcessionName,
													});
												return new ResultBase<ServiceTimeTableGetByIdResult> { Data = result };
								}
								#endregion
				}
}
