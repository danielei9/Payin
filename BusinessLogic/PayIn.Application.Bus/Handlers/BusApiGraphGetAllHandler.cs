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
	public class BusApiGraphGetAllHandler : IQueryBaseHandler<BusApiGraphGetAllArguments, BusApiGraphGetAllResult>
	{
		[Dependency] public IEntityRepository<Route> GraphRepository { get; set; }
		[Dependency] public ISessionData SessionData { get; set; }

		#region ExecuteAsync
		public async Task<ResultBase<BusApiGraphGetAllResult>> ExecuteAsync(BusApiGraphGetAllArguments arguments)
		{
			var result = (await GraphRepository.GetAsync())
				.Where(x =>
					(x.LineId == arguments.LineId) &&
					(x.Line.Login == SessionData.Login)
				)
				.Select(x => new BusApiGraphGetAllResult
				{
					Id = x.Id,
					Sense = x.Sense,
					Nodes = (
						x.Links
							.Select(y => new BusApiGraphGetAllResult_Node
							{
								Id = y.From.Id,
								Code = y.From.Code,
								Label = y.From.Name
							})
					).Union(
						x.Links
							.Select(y => new BusApiGraphGetAllResult_Node
							{
								Id = y.To.Id,
								Code = y.To.Code,
								Label = y.To.Name
							})
					),
					Links = x.Links
						.Select(y => new BusApiGraphGetAllResult_Link
						{
							Value = y.Weight,
							From = y.FromId,
							To = y.ToId
						})
				})
				.ToList();
			
			return new ResultBase<BusApiGraphGetAllResult>
			{
				Data = result
			};
		}
		#endregion ExecuteAsync
	}
}
