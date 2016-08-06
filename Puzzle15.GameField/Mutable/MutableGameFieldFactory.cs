using System;
using System.Drawing;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GameField.Mutable
{
	public class MutableGameFieldFactory<T> : IGameFieldFactory<T>
	{
		public IGameField<T> CreateGameField(Size size, Func<CellLocation, T> getValue)
		{
			return new MutableGameField<T>(size, getValue);
		}
	}
}