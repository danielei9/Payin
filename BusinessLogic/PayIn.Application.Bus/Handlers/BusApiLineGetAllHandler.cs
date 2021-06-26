using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Bus.Arguments;
using PayIn.Application.Dto.Bus.Results;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Bus;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Bus.Handlers
{
	public class BusApiLineGetAllHandler : IQueryBaseHandler<BusApiLineGetAllArguments, BusApiLineGetAllResult>
	{
		[Dependency] public IEntityRepository<Line> LineRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<BusApiLineGetAllResult>> ExecuteAsync(BusApiLineGetAllArguments arguments)
		{
			var result = (await LineRepository.GetAsync())
				.Where(x => x.Login == SessionData.Login)
				.Select(x => new BusApiLineGetAllResult
				{
					Id = x.Id,
					Login = x.Login,
					Name = x.Name
				})
				.ToList();
			
			return new ResultBase<BusApiLineGetAllResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
