using System;
using Xp.Common.Dto.Arguments;

namespace PayIn.Application.Dto.Arguments.ServiceWorker
{
    public class ServiceWorkerChangeRoleArguments : IArgumentsBase
    {
		public String userLogin { get; set; }
		public String roleName  { get; set; }
		public bool?   add       { get; set; }
		public bool?   remove    { get; set; }
    }
}
