using PayIn.BusinessLogic.Common;
using System;
using System.Threading.Tasks;

namespace Xp.Infrastructure.Http
{
	public abstract class BaseService
	{
		public abstract string BaseAddress { get; }
		public virtual ISessionData SessionData { get; set; }

		#region Server
		private HttpServer _Server = null;
		protected HttpServer Server
		{
			get
			{
				if (_Server == null)
					_Server = new HttpServer(BaseAddress, SessionData.Token);
				return _Server;
			}
		}
		#endregion Server
	}
}
