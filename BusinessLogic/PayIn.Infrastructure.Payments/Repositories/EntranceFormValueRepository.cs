using PayIn.Domain.Payments;
using PayIn.Infrastructure.Public.Db;
using System.Linq;

namespace PayIn.Infrastructure.Payments.Repositories
{
    public class EntranceFormValueRepository : PublicRepository<EntranceFormValue>
    {

        #region Contructors
        public EntranceFormValueRepository(
            IPublicContext context
        )
            : base(context)
        {
        }
        #endregion Contructors
    }
}
