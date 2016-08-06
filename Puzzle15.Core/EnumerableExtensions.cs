using System;
using System.Collections.Generic;
using System.Linq;

namespace Puzzle15.Core
{
	public static class EnumerableExtensions
	{
		private static readonly Random Random = new Random();

		public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
		{
			return source.OrderBy(x => Random.Next());
		}
	}
}