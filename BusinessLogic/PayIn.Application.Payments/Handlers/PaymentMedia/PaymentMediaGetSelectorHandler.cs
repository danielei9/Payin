using PayIn.Application.Dto.Arguments.PaymentMedia;
using PayIn.Application.Dto.Results.PaymentMedia;
using PayIn.Common;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
				public class PaymentMediaGetSelectorHandler :
					IQueryBaseHandler<PaymentMediaGetSelectorArguments, PaymentMediaGetSelectorResult>
				{
								private readonly IEntityRepository<PaymentMedia> _Repository;

								#region Constructors
								public PaymentMediaGetSelectorHandler(IEntityRepository<PaymentMedia> repository)
								{
												if (repository == null)
																throw new ArgumentNullException("repository");
												_Repository = repository;
								}
								#endregion Constructors

								#region ExecuteAsync
								async Task<ResultBase<PaymentMediaGetSelectorResult>> IQueryBaseHandler<PaymentMediaGetSelectorArguments, PaymentMediaGetSelectorResult>.ExecuteAsync(PaymentMediaGetSelectorArguments arguments)
								{
												arguments.Filter = (arguments.Filter ?? "").ToLower();

												var items = await _Repository.GetAsync();

												var result = items
													.Where(x => x.Name.Contains(arguments.Filter))
													.Select(x => new PaymentMediaGetSelectorResult
													{
																	Id = x.Id,
																	Value = x.Name
													});

												return new ResultBase<PaymentMediaGetSelectorResult> { Data = result };
								}
								#endregion ExecuteAsync
				}
}
