namespace System.Collections.Generic
{
	public static class DictionaryExtension
	{
		#region GetOrDefault
		public static V GetOrDefault<K, V>(this Dictionary<K, V> that, K key)
		{
			return that.ContainsKey(key) ?
				that[key] :
				default(V);
		}
		#endregion GetOrDefault
	}
}