using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoescuelas.Infrastructure.Interfaces.Repositories
{
	public interface IChatRepository : IPushSpecificRepository
	{
		Task Add(string connectionId);
		Task Remove(string connectionId);
	}
}
