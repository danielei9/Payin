using Microsoft.Practices.Unity;
using PayIn.Infrastructure.Bus.Db;
using PayIn.Infrastructure.JustMoney.Db;
using PayIn.Infrastructure.Public.Db;
using PayIn.Infrastructure.SmartCity.Db;
using System.Threading.Tasks;
using Xp.Domain;

namespace PayIn.Infrastructure.Public
{
	public class UnitOfWork : IUnitOfWork
	{
		[Dependency] public IUnitOfWork<IPublicContext> PublicContext { get; set; }
		[Dependency] public IUnitOfWork<ISmartCityContext> SmartCityContext { get; set; }
		[Dependency] public IUnitOfWork<IJustMoneyContext> JustMoneyContext { get; set; }
		[Dependency] public IUnitOfWork<IBusContext> BusContext { get; set; }

		#region SaveAsync
		public async Task SaveAsync()
		{
			if (PublicContext != null)
				await PublicContext.SaveAsync();
			if (SmartCityContext != null)
				await SmartCityContext.SaveAsync();
			if (JustMoneyContext != null)
				await JustMoneyContext.SaveAsync();
			if (BusContext != null)
				await BusContext.SaveAsync();
		}
		#endregion SaveAsync

		#region Dispose
		public void Dispose()
		{
			if (PublicContext != null)
				PublicContext.Dispose();
			if (SmartCityContext != null)
				SmartCityContext.Dispose();
			if (JustMoneyContext != null)
				JustMoneyContext.Dispose();
		}
		#endregion Dispose
		
		#region BeginTransaction
		public void BeginTransaction()
		{
			if (PublicContext != null)
				PublicContext.BeginTransaction();
			if (SmartCityContext != null)
				SmartCityContext.BeginTransaction();
			if (JustMoneyContext != null)
				JustMoneyContext.BeginTransaction();
		}
		#endregion BeginTransaction

		#region CommitTransaction
		public void CommitTransaction()
		{
			if (PublicContext != null)
				PublicContext.CommitTransaction();
			if (SmartCityContext != null)
				SmartCityContext.CommitTransaction();
			if (JustMoneyContext != null)
				JustMoneyContext.CommitTransaction();
		}
		#endregion CommitTransaction

		#region RollbackTransaction
		public void RollbackTransaction()
		{
			if (PublicContext != null)
				PublicContext.RollbackTransaction();
			if (SmartCityContext != null)
				SmartCityContext.RollbackTransaction();
			if (JustMoneyContext != null)
				JustMoneyContext.RollbackTransaction();
		}
		#endregion RollbackTransaction
	}
}
