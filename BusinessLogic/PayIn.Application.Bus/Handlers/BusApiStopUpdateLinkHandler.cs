using Microsoft.Practices.Unity;
using PayIn.Application.Dto.Bus.Arguments;
using PayIn.Application.Dto.Bus.Results;
using PayIn.Domain.Bus;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class BusApiStopUpdateLinkHandler :
        IServiceBaseHandler<BusApiStopUpdateLinkArguments>
    {
        [Dependency] public IEntityRepository<Link> Repository { get; set; }

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(BusApiStopUpdateLinkArguments arguments)
        {
            var item = (await Repository.GetAsync())
                .Where(x => x.Id == arguments.Id)
                .FirstOrDefault();
            if (item == null)
                throw new ArgumentNullException("id");

			var time = new TimeSpan(0, 0, arguments.Time_Seconds);

			item.Time= time;
			item.Weight= arguments.Weight;

            return item;
        }
        #endregion ExecuteAsync
    }
}
