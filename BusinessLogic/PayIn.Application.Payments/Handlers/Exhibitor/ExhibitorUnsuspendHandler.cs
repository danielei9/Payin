using PayIn.Application.Dto.Payments.Arguments;
using PayIn.Common;
using PayIn.Domain.Payments;
using System;
using System.Threading.Tasks;
using Xp.Application.Dto;
using Xp.Domain;

namespace PayIn.Application.Payments.Handlers
{
    public class ExhibitorUnsuspendHandler :
        IServiceBaseHandler<ExhibitorUnsuspendArguments>
    {
        private readonly IEntityRepository<Exhibitor> Repository;

        #region Constructors
        public ExhibitorUnsuspendHandler(IEntityRepository<Exhibitor> repository)
        {
            if (repository == null) throw new ArgumentNullException("repository");
            Repository = repository;
        }
        #endregion Constructors

        #region ExecuteAsync
        public async Task<dynamic> ExecuteAsync(ExhibitorUnsuspendArguments arguments)
        {
            var item = (await Repository.GetAsync(arguments.Id));

            item.State = ExhibitorState.Active;

            return item;
        }
        #endregion ExecuteAsync
    }
}
