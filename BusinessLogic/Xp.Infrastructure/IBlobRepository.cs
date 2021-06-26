using System.Threading.Tasks;

namespace Xp.Infrastructure
{
	public interface IBlobRepository
	{
		string SaveFile(string name, byte[] content);
		string SaveFile(string name, string content);
		Task<string> LoadStringFileAsync(string name);
		Task<string> LoadStringUrlAsync(string url);
		Task<bool> FileExistsAsync(string name, string containerName);
	}
}
