using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Xp.Domain;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Threading.Tasks;
using Microsoft.Azure;

namespace Xp.Infrastructure.Repositories
{
	public abstract class AzureQueueRepository<T> : IQueueRepository<T>
		where T : IQueueEntity
	{
		public readonly TimeSpan InvisibleTimeSpan = TimeSpan.FromMinutes(5);

		public readonly string ConnectionString;
		public readonly string Name;

		#region Account
		private CloudStorageAccount _Account;
		public CloudStorageAccount Account
		{
			get
			{
				if (_Account == null)
				{
					var accountName = CloudConfigurationManager.GetSetting(ConnectionString);
					_Account = CloudStorageAccount.Parse(accountName);
				}

				//if (_Account == null)
				//	_Account = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString);

				return _Account;
			}
		}
		#endregion Account

		#region Queue
		private CloudQueue _Queue;
		public CloudQueue Queue
		{
			get
			{
				if (_Queue == null)
				{
					var client = Account.CreateCloudQueueClient();
					_Queue = client.GetQueueReference(Name);
				}

				_Queue.CreateIfNotExists();

				return _Queue;
			}
		}
		#endregion Queue

		#region Contructors
		public AzureQueueRepository(string connectionString, string name)
		{
			ConnectionString = connectionString;
			Name = name;
		}
		#endregion Contructors

		#region PushAsync
		public async Task PushAsync(IQueueEntity entity)
		{
			var message = new CloudQueueMessage(entity.ToJson());

			await Queue.AddMessageAsync(message);
		}
		#endregion PushAsync

		#region PopAsync
		public async Task<TResult> PopAsync<TResult>()
			where TResult : IQueueEntity
		{
			var message = await Queue.GetMessageAsync(InvisibleTimeSpan, null, null);
			if (message == null)
				return default(TResult);

			var entity = message.AsString.FromJson<TResult>();
			entity.MessageId = message.Id;
			entity.PopReceipt = message.PopReceipt;

			return entity;
		}
		#endregion PopAsync

		#region PeekAsync
		public async Task<TResult> PeekAsync<TResult>()
			where TResult : IQueueEntity
		{
			var message = await Queue.PeekMessageAsync();
			if (message == null)
				return default(TResult);

			var entity = message.AsString.FromJson<TResult>();
			entity.MessageId = message.Id;
			entity.PopReceipt = message.PopReceipt;

			return entity;
		}
		#endregion PeekAsync

		#region DeleteAsync
		public async Task DeleteAsync(IQueueEntity entity)
		{
			await Queue.DeleteMessageAsync(entity.MessageId, entity.PopReceipt);
		}
		#endregion DeleteAsync
	}
}
