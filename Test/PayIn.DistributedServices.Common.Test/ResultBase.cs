using System.Collections.Generic;

namespace PayIn.DistributedServices.Common.Test
{
    public class ResultBase<T>
    {
        public IEnumerable<T> Data { get; set; }
    }
}
