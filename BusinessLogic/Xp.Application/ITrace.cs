using System.Collections.Generic;
using Xp.Common;

namespace Xp.Application
{
	public interface ITrace<TItem>
		where TItem : IPosition
	{
		int                Id         { get; set; }
		int                WorkerId   { get; set; }
		string             WorkerName { get; set; }
		int                ItemId     { get; set; }
		string             ItemName   { get; set; }
		TItem              Since      { get; set; }
		TItem              Until      { get; set; }
		IEnumerable<TItem> Items      { get; }
	}
}
