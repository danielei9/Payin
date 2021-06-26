using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Arguments;
using PayIn.Application.Dto.Results;
using PayIn.Domain.Public;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Public.Handlers
{
	public class ServiceCategoryGetHandler :
		IQueryBaseHandler<ServiceCategoryGetArguments, ServiceCategoryGetResult>
	{
		[Dependency] public IEntityRepository<ServiceCategory> Repository { get; set; }
		
		#region ExecuteAsync
		public async Task<ResultBase<ServiceCategoryGetResult>> ExecuteAsync(ServiceCategoryGetArguments arguments)
		{
			var items = (await Repository.GetAsync())
				.Where(x => x.Id == arguments.Id);

			var result = items
				.Select(x => new
				{
					Id = x.Id,
					Name = x.Name,
					AllMembersInSomeGroup = x.AllMembersInSomeGroup,
					AMemberInOnlyOneGroup = x.AMemberInOnlyOneGroup,
					AskWhenEmit = x.AskWhenEmit,
					DefaultGroupWhenEmitId = x.DefaultGroupWhenEmitId,
					DefaultGroupWhenEmitName = x.DefaultGroupWhenEmit.Name
				})
				.ToList()
				.Select(x => new ServiceCategoryGetResult
				{
					Id = x.Id,
					Name = x.Name,
					AllMembersInSomeGroup = x.AllMembersInSomeGroup,
					AMemberInOnlyOneGroup = x.AMemberInOnlyOneGroup,
					AskWhenEmit = x.AskWhenEmit,
					DefaultGroupWhenEmitId = x.DefaultGroupWhenEmitId,
					DefaultGroupWhenEmitName = x.DefaultGroupWhenEmitName
				});

			return new ResultBase<ServiceCategoryGetResult> { Data = result };
		}
	}
	#endregion ExecuteAsync
}

