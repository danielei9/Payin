using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class ExhibitorSuspendHandler :
        IServiceBaseHandler<ExhibitorSuspendArguments>
    {
        private readonly IEntityRepository<Exhibitor> Repository;

        #region Constructors
        public ExhibitorSuspendHandler(IEntityRepository<Exhibitor> repository)
        {
            if (repository == null) throw new ArgumentNullException("repository");
            Repository = repository;
        }
        #endregion Constructors

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(ExhibitorSuspendArguments arguments)
        {
            var item = (await Repository.GetAsync(arguments.Id));

            item.State = ExhibitorState.Suspended;

            return item;
        }
        #endregion ExecuteAsync
    }
}
