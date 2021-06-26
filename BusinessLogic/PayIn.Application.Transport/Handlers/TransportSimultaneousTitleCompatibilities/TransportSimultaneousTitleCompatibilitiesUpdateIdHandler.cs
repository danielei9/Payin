using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Transport.Arguments.TransportSimultaneousTitleCompatibilities;
using PayIn.Application.Dto.Transport.Results.TransportSimultaneousTitleCompatibilities;
using PayIn.BusinessLogic.Common;
using PayIn.Domain.Transport;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Transport.Handlers
{
	public class TransportSimultaneousTitleCompatibilitiesUpdateIdHandler : IQueryBaseHandler<TransportSimultaneousTitleCompatibilitiesUpdateIdArguments, TransportSimultaneousTitleCompatibilitiesUpdateIdResult>
	{
		[Dependency] public ISessionData SessionData { get; set; }
		[Dependency] public IEntityRepository<TransportSimultaneousTitleCompatibility> Repository { get; set; }

        #region ExecuteAsync
        public async Task<ResultBase<TransportSimultaneousTitleCompatibilitiesUpdateIdResult>> ExecuteAsync(TransportSimultaneousTitleCompatibilitiesUpdateIdArguments arguments)
        {
            var titleName = (await Repository.GetAsync())
                .Where(x => x.Id == arguments.Id)
                .Select(x => x.TransportTitle.Name)
                .FirstOrDefault();
            if (titleName == null)
                throw new ArgumentNullException("Id");

			var results = (await Repository.GetAsync())
                .Where(x =>
					x.Id == arguments.Id
				)
				.Select(x => new TransportSimultaneousTitleCompatibilitiesUpdateIdResult
                {
					Id = x.Id,
					TransportTitleId = x.TransportTitleId,
                    //TransportTitle2Id = x.TransportTitle2Id,
					Name = x.TransportTitle2.Name
                    
                });

            return new TransportSimultaneousTitleCompatibilitiesUpdateIdResultBase
            {
				Data = results,
				TitleName = titleName
			};
		}
		#endregion ExecuteAsync
	}
}

