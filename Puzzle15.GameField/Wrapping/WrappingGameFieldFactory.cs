using System;
using System.Drawing;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GameField.Wrapping
{
	public class WrappingGameFieldFactory<TCell> : IGameFieldFactory<TCell>
	{
		public IGameField<TCell> CreateGameField(Size size, Func<CellLocation, TCell> getValue)
			=> new WrappingGameField<TCell>(size, getValue);

		public IGameField<TCell> CreateGameField(IGameField<TCell> source)
			=> new WrappingGameField<TCell>(source);
	}
}