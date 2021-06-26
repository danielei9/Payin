using System.Collections.Generic;

namespace PayIn.Application.Dto.Results
{
	public class MobileMainSynchronizeResult_ConfigCard
    {
        public bool EmitPrimary { get; set; }
        public bool EmitSecondary { get; set; }
        public bool EmitAnonymous { get; set; }

        public IEnumerable<MobileMainSynchronizeResult_ConfigWallet> Wallets { get; set; }
    }
}
