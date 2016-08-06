using System;
using System.Collections.Generic;

namespace Puzzle15.Core
{
	public static class DictionaryExtensions
	{
		public static TValue ComputeIfAbsent<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, Func<TKey, TValue> getValue)
		{
			TValue result;
			if (source.TryGetValue(key, out result))
				return result;

			return source[key] = getValue(key);
		}
	}
}