using System.Collections.Generic;

namespace PayIn.Application.Bus.Services.Dijkstra
{
	class PrioQueue : LinkedList<Node>
	{
		public void AddNodeWithPriority(Node node)
		{
			if (this.Count == 0)
			{
				this.AddFirst(node);
			}
			else
			{
				if (node.Value >= this.Last.Value.Value)
				{
					this.AddLast(node);
				}
				else
				{
					for (LinkedListNode<Node> it = this.First; it != null; it = it.Next)
					{
						if (node.Value <= it.Value.Value)
						{
							this.AddBefore(it, node);
							break;
						}
					}
				}
			}
		}

		public bool HasLetter(string letter)
		{
			for (LinkedListNode<Node> it = this.First; it != null; it = it.Next)
			{
				if (it.Value.Code == letter) { return true; }
			}
			return false;
		}
	}
}
