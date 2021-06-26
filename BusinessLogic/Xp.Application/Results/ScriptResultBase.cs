using System.Collections.Generic;
using Xp.Application.Dto;
using Xp.Domain.Transport;

namespace Xp.Application.Results
{
	public class ScriptResultBase<TResult> : ResultBase<TResult>
    {
        public class ScriptResultOperation
        {
            public OperationType Type { get; set; }
            public long? Uid { get; set; }
            public int Id { get; set; }
            public ScriptResultOperationCard Card { get; set; }
        }
        public class ScriptResultOperationCard
        {
            public long Uid { get; set; }
        }

        public IEnumerable<ScriptResult> Scripts { get; set; }
		public ScriptResultOperation Operation { get; set; }
		public dynamic Device { get; set; }
	}
}
