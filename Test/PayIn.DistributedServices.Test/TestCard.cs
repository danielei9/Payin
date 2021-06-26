using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using PayIn.Domain.Transport.Eige;
using PayIn.Domain.Transport.MifareClassic.Operations;
using System;
using System.Collections.Generic;
using Xp.Application.Results;
using Xp.Domain.Transport;
using Xp.Domain.Transport.MifareClassic;

namespace PayIn.DistributedServices.Test
{
	public abstract class TestCard : EigeCard, ITestCard
	{
		#region Constructors
		public TestCard(string uid, string[] content, Byte[] uid2)
			: base(null, null)
		{
			Uid = uid.FromHexadecimal();
			for (var i = 0; i < 16; i++)
				for (var j = 0; j < 3; j++)
					Sectors[i].Blocks[j].Value = content[i * 4 + j].FromHexadecimal();
		}
		#endregion Constructors

		#region Read
		public string Read(int sector, byte block)
		{
			return Sectors[sector].Blocks[block].Value.ToHexadecimal();
		}
		#endregion Read

		#region Execute
		public IEnumerable<MifareOperationResultArguments> Execute(JToken scripts)
		{
			var operations = new List<IMifareOperation>();
			var result = new ScriptResult
			{
				Card = new ScriptResult.CardId
				{
					System = CardSystem.Mobilis,
					Type = CardType.MIFAREClassic,
					Uid = Uid.ToInt32().Value
				},
				Script = operations
			};
			var script = scripts["script"];
			foreach (var operation in script)
			{
				var operationCode = (MifareOperationType)operation.Value<int>("operation");
				if (operationCode == MifareOperationType.ReadBlock)
				{
					operations.Add(new MifareReadOperation
					{
						Block = operation.Value<byte>("block"),
						Operation = operationCode,
						Sector = operation.Value<byte>("sector")
					});
				}
				else if (operationCode == MifareOperationType.WriteBlock)
				{
					operations.Add(new MifareWriteOperation
					{
						Block = operation.Value<byte>("block"),
						Operation = operationCode,
						Sector = operation.Value<byte>("sector"),
						Data = operation.Value<string>("data"),
					});
				}
				else if (operationCode == MifareOperationType.Check)
				{
					operations.Add(new MifareCheckOperation
					{
						Block = operation.Value<byte>("block"),
						Operation = operationCode,
						Sector = operation.Value<byte>("sector"),
						Data = operation.Value<string>("data"),
					});
				}
			}

			return Execute(result);
		}
		public IEnumerable<MifareOperationResultArguments> Execute(ScriptResult script)
		{
			var operations = new List<MifareOperationResultArguments>();

			foreach (var operation in script.Script)
			{
				if (operation.Operation == MifareOperationType.ReadBlock)
				{
					var op = operation as MifareReadOperation;
					var item = new MifareOperationResultArguments()
					{
						Block = op.Block,
						Data = Sectors[op.Sector].Blocks[op.Block].Value.ToHexadecimal(),
						Operation = op.Operation,
						Sector = op.Sector
					};
					operations.Add(item);
				}
				else if (operation.Operation == MifareOperationType.WriteBlock)
				{
					var op = operation as MifareWriteOperation;
					Sectors[op.Sector].Blocks[op.Block].Value = op.Data.FromHexadecimal();
				}
				else if (operation.Operation == MifareOperationType.Check)
				{
					var op = operation as MifareCheckOperation;
					Assert.AreEqual(Sectors[op.Sector].Blocks[op.Block].Value.ToHexadecimal(), op.Data);
				}
			}
			return operations;
		}
		#endregion Execute
	}
}
