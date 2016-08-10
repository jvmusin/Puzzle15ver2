using System;
using System.Drawing;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GameField.Mutable
{
	public class MutableGameFieldFactory<TCell> : IGameFieldFactory<TCell>
	{
		public IGameField<TCell> CreateGameField(Size size, Func<CellLocation, TCell> getValue)
			=> new MutableGameField<TCell>(size, getValue);

		public IGameField<TCell> CreateGameField(IGameField<TCell> source) 
			=> CreateGameField(source.Size, loc => source[loc]);
	}
}