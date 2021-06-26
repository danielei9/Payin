using PayIn.Domain.Transport.Eige;
using PayIn.Domain.Transport.Eige.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Xp.Domain.Transport;
using Xp.Domain.Transport.MifareClassic;
using Xp.Domain.Transport.MifareClassic.Operaons;

namespace PayIn.Domain.Transport.MifareClassic.Operations
{
	public class MifareClassicScript<TCard> : IMifareClassicScript<TCard>
	  where TCard : MifareClassicCard
	{
		private const string METHOD_NAME = "MifareClassicScript";

		public TCard Card { get; set; }

		public List<IMifareRWOperation> Request { get; set; }
		public List<IMifareRWOperation> Response { get; set; }

		#region Constructors
		public MifareClassicScript(TCard card)
			: this()
		{
			Card = card;
		}
		public MifareClassicScript()
		{
			Request = new List<IMifareRWOperation>();
			Response = new List<IMifareRWOperation>();
		}
		#endregion Constructors

		#region Read
		public void Read(List<IMifareRWOperation> list, Expression<Func<TCard, object>> expression)
		{
			var property = expression.GetProperty();
			var attribute = property.GetCustomAttribute<MifareClassicMemoryAttribute>();
			if (attribute == null)
				return;

			list.Add(MifareReadOperation.Create(attribute.Sector, attribute.Block));
		}
		public void Read(List<IMifareRWOperation> list, byte sector, byte block)
		{
			// Blockes de copia
			if (sector == 1 && block == 1)
				list.Add(MifareReadOperation.Create(sector, 2));
			if (sector == 4 && block == 0)
				list.Add(MifareReadOperation.Create(sector, 1));

			list.Add(MifareReadOperation.Create(sector, block));
		}
		#endregion Read

		#region Write
		public void Write(List<IMifareRWOperation> list, Expression<Func<TCard, object>> expression)
		{
			var property = expression.GetProperty();
			var attribute = property.GetCustomAttribute<MifareClassicMemoryAttribute>();
			if (attribute == null)
				return;

			Write(list, attribute.Sector, attribute.Block, Card.Sectors[attribute.Sector].Blocks[attribute.Block].Value.ToHexadecimal());
		}
		public void Write(List<IMifareRWOperation> list, byte sector, byte block)
		{
			Write(list, sector, block, Card.Sectors[sector].Blocks[block].Value.ToHexadecimal());
		}
		public async Task WriteAsync(List<IMifareRWOperation> list, byte sector, byte block, long uid)
		{
			var item = Card.Sectors[sector].Blocks[block];

			if ((item.OldValue != null) && (item.OldValue != item.Value))
			{
				var value = await Card.GetValueWithCrcAsync(uid, sector, block);
				Write(list, sector, block, value.ToHexadecimal());
			}
		}
		public void Write(List<IMifareRWOperation> list, byte sector, byte block, string data, int? from = null, int? to = null)
		{
			list.Add(MifareWriteOperation.Create(sector, block, data, from, to));
		}
		#endregion Write

		//#region Sum
		//public void Sum(List<IMifareRWOperation> list, Expression<Func<TCard, object>> expression)
		//{
		//	var property = expression.GetProperty();
		//	var attribute = property.GetCustomAttribute<MifareClassicMemoryAttribute>();
		//	if (attribute == null)
		//		return;

		//	Sum(list, attribute.Sector, attribute.Block, Card.Sectors[attribute.Sector].Blocks[attribute.Block].Value.ToHexadecimal());
		//}
		//public void Sum(List<IMifareRWOperation> list, byte sector, byte block, string data, int? from = null, int? to = null)
		//{
		//	list.Add(MifareSumOperation.Create(sector, block, data, from, to));
		//}
		//#endregion Sum

		#region Check
		public void Check(List<IMifareRWOperation> list, Expression<Func<TCard, object>> expression)
		{
			var property = expression.GetProperty();
			var attribute = property.GetCustomAttribute<MifareClassicMemoryAttribute>();
			if (attribute == null)
				return;

			Check(list, attribute.Sector, attribute.Block, Card.Sectors[attribute.Sector].Blocks[attribute.Block].Value.ToHexadecimal());
		}
		public void Check(List<IMifareRWOperation> list, byte sector, byte block, string data, int? from = null, int? to = null)
		{
			list.Add(MifareCheckOperation.Create(sector, block, data, from, to));
		}
		#endregion Check

		#region GetKeysEncryptedAsync
		public async Task<string> GetKeysEncryptedAsync(IMifareClassicHsmService hsm, CardSystem cardSystem, IEnumerable<IMifareOperation> response, long uid, int aux)
		{
			var list = response
			  .Where(x => x is MifareAutenticateOperation)
			  .Cast<MifareAutenticateOperation>()
			  .GroupBy(x => new { x.Sector, x.KeyType })
			  .Select(x => new MifareKeyId
			  {
				  Sector = x.Key.Sector,
				  Type = x.Key.KeyType
			  });

			var result = await hsm.GetKeysEncryptedAsync(cardSystem, list, uid, aux);
			return result;
		}
		#endregion GetKeysEncryptedAsync

		#region GetResponseAndRequestAsync
		public async Task<IEnumerable<IMifareOperation>> GetResponseAndRequestAsync()
		{
			var result = await GetResponseAsync();
			if (result.Count() == 0)
				return result;
			result = result
				.Union(await GetRequestAsync());

			return result;
		}
		#endregion GetResponseAndRequestAsync

		#region GetRequestAsync
		public async Task<IEnumerable<IMifareOperation>> GetRequestAsync()
		{
#if DEBUG
			var watch = Stopwatch.StartNew();
#endif
			// Agrupar lecturas ad-hoc
			var listReduced = Request
			 .GroupBy(x => new { x.Operation, x.Sector, x.Block })
			 .Select(x => x.LastOrDefault() as IMifareOperation)
			 .ToList();
#if DEBUG
			watch.Stop();
			Debug.WriteLine(METHOD_NAME + "GetRequestAsync - Agrupar lecturas ad-hoc: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
			watch = Stopwatch.StartNew();
#endif

			// Crear autenticaciones de las lecturas
			var list = await Task.WhenAll(listReduced
			 .GroupBy(x => new { x.Sector })
			 .Select(async x => await MifareAutenticateOperation.CreateAsync(x.Key.Sector, x.Key.Sector == 9 ? MifareKeyType.B : MifareKeyType.A))
		  );
#if DEBUG
			watch.Stop();
			Debug.WriteLine(METHOD_NAME + "GetRequestAsync - Crear autenticaciones de las lecturas: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
			watch = Stopwatch.StartNew();
#endif

			// Ordenar lecturas y autenticaciones
			var result = list
			 .Union(listReduced)
			 .OrderBy(x => x.Sector)
			 .ThenBy(x => x.Operation)
			 .ToList();
#if DEBUG
			watch.Stop();
			Debug.WriteLine(METHOD_NAME + "GetRequestAsync - Ordenar lecturas y autenticaciones: " + TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
			watch = Stopwatch.StartNew();
#endif

			return result;
		}
		#endregion GetRequestAsync

		#region GetResponseAsync
		public async Task<IEnumerable<IMifareOperation>> GetResponseAsync()
		{
			var list = new List<IMifareOperation>();
			int? authorizedSector = null;

			#region Obtener los campos modificados
			var modifieds = Card.Sectors
				.SelectMany(x => x.Blocks
					.Where(y =>
						(y.Value != null) &&
						(y.OldValue != null) &&
						(!y.Value.SequenceEqual(y.OldValue))
					)
					.Select(y => new {
						Sector = (byte)x.Number,
						Block = (byte)y.Number,
						OldValue = y.OldValue,
						Value = y.Value
					})
				)
				.OrderByDescending(x => x.Sector)
				.ToList();
			if (modifieds.Count() == 0) //No devolver nada si no hay cambios
				return list;
			#endregion Obtener los campos modificados

			#region Comprobar el sector 0
			var value0_0 = Card.Sectors
				.Where(x =>
					(x.Number == 0)
				)
				.SelectMany(x => x.Blocks
					.Where(y =>
						(y.Value != null) &&
						(y.Number == 0)
					)
					.Select(y => y.Value.ToHexadecimal())
				)
				.FirstOrDefault();
			if (value0_0 != null)
			{
				list.Add(await MifareAutenticateOperation.CreateAsync(0, MifareKeyType.A));
				authorizedSector = 0;
				list.Add(MifareWriteOperation.Check((byte)0, (byte)0, value0_0));
			}
			#endregion Comprobar el sector 0

			#region Hacer check de los writes
			foreach (var item in modifieds)
			{
				// No comprobar los campos de copia
				if (
					(item.Sector == 1 && item.Block == 2) || // Quitar copia titulo 1
					(item.Sector == 4 && item.Block == 1) || // Quitar copia titulo 2
					(item.Sector == 9 && item.Block == 1) // Quitar copia titulo TuiN
				)
					continue;

				// Comprobar el check que no esté ya en la lista
				if (list
					.Where(x => (x is IMifareRWOperation))
					.Cast<IMifareRWOperation>()
					.Where(x =>
						(x.Operation == MifareOperationType.Check) &&
						(x.Sector == item.Sector) &&
						(x.Block == item.Block)
					).Any())
					continue;

				// Autenticar el sector si no lo está ya
				if (authorizedSector != item.Sector)
				{
					list.Add(await MifareAutenticateOperation.CreateAsync(item.Sector, MifareKeyType.A));
					authorizedSector = item.Sector;
				}
				list.Add(MifareWriteOperation.Check(item.Sector, item.Block, item.OldValue.ToHexadecimal()));
			}
			#endregion Hacer check de los writes

			#region Añadir los writes explicitos
			authorizedSector = null;
			foreach (var item in Response)
			{
				// Comprobar que no esté ya en la lista
				if (list
					.Where(x => (x is IMifareRWOperation))
					.Cast<IMifareRWOperation>()
					.Where(x =>
						(x.Operation == item.Operation) &&
						(x.Sector == item.Sector) &&
						(x.Block == item.Block)
					).Any()
				)
					continue;

                if (item.Operation != MifareOperationType.Success)
                {
                    // Comprueba que se haya modificado
                    if (!modifieds
                        .Where(x =>
                            x.Sector == item.Sector &&
                            x.Block == item.Block
                        )
                        .Any()
                    )
                        continue;

                    // Autenticar el sector si no lo está ya
                    if (authorizedSector != item.Sector)
                    {
                        list.Add(await MifareAutenticateOperation.CreateAsync(item.Sector, MifareKeyType.B));
                        authorizedSector = item.Sector;
                    }
                }

				list.Add(item);
			}
			#endregion Añadir los writes explicitos

			#region Añadir los writes explicitos
			foreach (var item in modifieds)
			{
				// Comprobar que no esté ya en la lista
				if (list
					.Where(x => (x is IMifareRWOperation))
					.Cast<IMifareRWOperation>()
					.Where(x =>
						(x.Operation == MifareOperationType.WriteBlock) &&
						(x.Sector == item.Sector) &&
						(x.Block == item.Block)
					).Any()
				)
					continue;

				// Autenticar el sector si no lo está ya
				if (authorizedSector != item.Sector)
				{
					list.Add(await MifareAutenticateOperation.CreateAsync(item.Sector, MifareKeyType.B));
					authorizedSector = item.Sector;
				}
				list.Add(MifareWriteOperation.Create(item.Sector, item.Block, item.Value.ToHexadecimal()));
			}
			#endregion Añadir los writes explicitos

			return list;
		}
		#endregion GetResponseAsync

		#region Load
		public void Load(IEnumerable<MifareOperationResultArguments> values)
		{
			var blocks = values
			   .Where(x => x.Operation == MifareOperationType.ReadBlock)
			   .Cast<IMifareReadOperationsArguments>();
			foreach (var item in blocks)
				Card.Sectors[item.Sector].Blocks[item.Block].Value = item.Data.FromHexadecimal();
		}

		public void Load(MifareClassicScript<EigeCard> values)
		{
			var blocks = values.Request
				.Where(x => x.Operation == MifareOperationType.ReadBlock)
				.Cast<IMifareReadOperationsArguments>();
			foreach (var item in blocks)
				Card.Sectors[item.Sector].Blocks[item.Block].Value = item.Data.FromHexadecimal();
		}
		#endregion Load

		#region ToString
		public override string ToString()
		{
			var json = this.ToJson();
			return json;
		}
		#endregion ToString

		public object GetValueFromCode(string code)
		{
			foreach (var itemProperty in Card.GetType().GetProperties())
			{
				var item = itemProperty.GetValue(Card);
				foreach (var fieldProperty in item.GetType().GetProperties())
				{
					if (code.StartsWith("BLO"))
					{
						var splitted = code.Split('O');
						int sector = 0;
						int block = 0;
						var limit = Convert.ToInt32(splitted[1]);

						for (int i = 0; i < limit; i++)
						{
							block++;
							if (block == 4)
							{
								block = 0;
								sector++;
							}
						}

						return Card.Sectors[sector].Blocks[block];
					}

					var attr = fieldProperty.GetCustomAttribute<MifareClassicMemoryAttribute>();
					if (attr == null)
						continue;
					if (attr.Name == code)
					{
						if (attr.Name == code)
							return fieldProperty.GetValue(item);
					}
				}
			}

			return null;
		}
		public async void SetValueFromCode(string code, string value, GreyList.ActionType operation, long uid)
		{
			await Task.Run(() =>
			{
				var exit = false;

				foreach (var itemProperty in Card.GetType().GetProperties())
				{
					var item = itemProperty.GetValue(Card);
					foreach (var property in item.GetType().GetProperties())
					{
						if (code.StartsWith("BLO") && operation == GreyList.ActionType.ModifyBlock)
						{
							var splitted = code.Split('O');
							byte sector = 0;
							byte block = 0;
							var limit = Convert.ToInt32(splitted[1]);

							for (int i = 0; i < limit; i++)
							{
								block++;
								if (block == 4)
								{
									block = 0;
									sector++;
								}
							}

							if (Card.Sectors[sector].Blocks[block].OldValue == null)
								Card.Sectors[sector].Blocks[block].OldValue = (byte[])Card.Sectors[sector].Blocks[block].Value.Clone();
							Card.Sectors[sector].Blocks[block].Set(value.FromHexadecimal());

							//Card.SetCrc(sector, block);
							Card.BlockBackup(sector, block);

							exit = true;
							break;
						}

						var attr = property.GetCustomAttribute<MifareClassicMemoryAttribute>();
						if (attr == null)
							continue;
						if (attr.Name == code)
						{
							SetValue(item, property, attr, value);

							exit = true;
							break;
						}
					}
					if (exit)
						break;
				}
			});
		}
		public async Task AddValueFromCodeAsync(string code, decimal value)
		{
			await Task.Run(() =>
			{
				foreach (var itemProperty in Card.GetType().GetProperties())
				{
					var item = itemProperty.GetValue(Card);
					foreach (var property in item.GetType().GetProperties())
					{
						var attr = property.GetCustomAttribute<MifareClassicMemoryAttribute>();
						if (attr == null)
							continue;

						if (attr.Name == code)
						{
							var val = property.GetValue(item);
							if (val == null)
								SetValue(item, property, attr, null, value);
							else
								SetValue(item, property, attr, val.GetPropertyValue("Value"), value);

							return;
						}
					}
				}
			});
		}

		public DateTime ConvertToEigeDateTime(object value)
		{
			string anyo = value.ToString().Substring(0, 4);
			string mes = value.ToString().Substring(4, 2);
			string dia = value.ToString().Substring(6, 2);
			string hora = value.ToString().Substring(8, 2);
			string minuto = value.ToString().Substring(10, 2);

			return new DateTime(Convert.ToInt32(anyo), Convert.ToInt32(mes), Convert.ToInt32(dia), Convert.ToInt32(hora), Convert.ToInt32(minuto), 59);
		}

		private void SetValue(object item, PropertyInfo property, MifareClassicMemoryAttribute attr, object value, decimal? valueToAdd = null)
		{
			if (property.PropertyType == typeof(EigeInt8))
				property.SetValue(item, new EigeInt8(Convert.ToInt32(value) + Convert.ToInt32(valueToAdd ?? 0)));

			else if (property.PropertyType == typeof(EigeInt16))
				property.SetValue(item, new EigeInt16(Convert.ToInt32(value) + Convert.ToInt32(valueToAdd ?? 0)));

			else if (property.PropertyType == typeof(EigeInt24))
				property.SetValue(item, new EigeInt24(Convert.ToInt32(value) + Convert.ToInt32(valueToAdd ?? 0)));

			else if (property.PropertyType == typeof(EigeInt32))
				property.SetValue(item, new EigeInt32(Convert.ToInt32(value) + Convert.ToInt32(valueToAdd ?? 0)));

			else if (property.PropertyType == typeof(EigeBool))
				property.SetValue(item, new EigeBool(Convert.ToBoolean(Convert.ToInt32(value))));

			else if (property.PropertyType == typeof(EigeDate))
				property.SetValue(item, new EigeDate(Convert.ToDateTime(value)));
			else if (property.PropertyType == typeof(EigeDateTime))
			{
				if (value != null)
				{
					dynamic eigeDate;

					if (value.GetType() == typeof(DateTime))
						eigeDate = new EigeDateTime(Convert.ToDateTime(value));
					else
						eigeDate = new EigeDateTime(Convert.ToDateTime(ConvertToEigeDateTime(value)));
					property.SetValue(item, eigeDate);
				}
			}
			else if (property.PropertyType == typeof(EigeString))
			{
				var byteArray = (byte[])value;
				property.SetValue(item, new EigeString(byteArray, byteArray.Length));
			}
			else if (property.PropertyType == typeof(EigeCurrency))
			{
				var byteArray = (byte[])value;
				property.SetValue(item, new EigeCurrency(byteArray, byteArray.Length));
			}
			else if (property.PropertyType == typeof(EigeMesDia))
			{
				var byteArray = (byte[])value;
				property.SetValue(item, new EigeMesDia(byteArray, byteArray.Length));
			}
			else if (property.PropertyType == typeof(EigeBytes))
			{
				var byteArray = (byte[])value;
				property.SetValue(item, new EigeBytes(byteArray, byteArray.Length));
			}
			else if (property.PropertyType == typeof(EigeBcd))
			{
				var byteArray = (byte[])value;
				property.SetValue(item, new EigeBcd(byteArray, byteArray.Length));
			}
			else if (property.PropertyType == typeof(GenericEnum<>))
			{
				var val = Activator.CreateInstance(property.PropertyType, value, attr.EndBit - attr.StartBit + 1);
				property.SetValue(item, val);
			}
			//Cambios
		}

		#region GetHexadecimal
		public string GetHexadecimal()
		{
			var script = new string[16];
			foreach (var item in Response)
				if (item.Operation == MifareOperationType.ReadBlock)
					script[item.Sector * 4 + item.Block] = item.Data;
			var result = script.JoinString("\n");

			return result;
		}
		#endregion GetHexadecimal
	}
}