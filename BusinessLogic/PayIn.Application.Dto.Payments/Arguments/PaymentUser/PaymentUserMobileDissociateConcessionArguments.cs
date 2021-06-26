using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Payments.Arguments.PaymentUser
{
	public class PaymentUserMobileDissociateConcessionArguments : IArgumentsBase
	{
        public int Id { get; set; }

        #region Constructors
        public PaymentUserMobileDissociateConcessionArguments(int id)
        {
            Id = id;
        }
        #endregion Constructors
    }
}
