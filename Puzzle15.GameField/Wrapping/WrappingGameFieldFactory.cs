using System;
using System.Drawing;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GameField.Wrapping
{
	public class WrappingGameFieldFactory<T> : IGameFieldFactory<T>
	{
		public IGameField<T> CreateGameField(Size size, Func<CellLocation, T> getValue)
			=> new WrappingGameField<T>(size, getValue);

		public IGameField<T> CreateGameField(IGameField<T> source)
			=> new WrappingGameField<T>(source);
	}
}