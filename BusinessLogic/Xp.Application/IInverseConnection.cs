namespace Xp.Application
{
	public interface IInverseConnection
	{
		T Send<T>(object args);
	}
}
