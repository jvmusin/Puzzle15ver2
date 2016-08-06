using System;
using System.Drawing;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GameField
{
	public interface IGameFieldFactory<T>
	{
		IGameField<T> CreateGameField(Size size, Func<CellLocation, T> getValue);
		IGameField<T> CreateGameField(IGameField<T> source);
	}
}