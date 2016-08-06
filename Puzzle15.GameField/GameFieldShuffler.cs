using System;
using System.Linq;
using Puzzle15.Core;

namespace Puzzle15.GameField
{
	public interface IGameFieldShuffler<T>
	{
		IGameField<T> Shuffle(IGameField<T> gameField, int quality);
	}

	public class GameFieldShuffler<T> : IGameFieldShuffler<T>
	{
		public IGameField<T> Shuffle(IGameField<T> gameField, int quality)
		{
			var shiftsCount = 1 << quality;
			for (var i = 0; i < shiftsCount; i++)
				gameField = ShiftRandom(gameField);
			return gameField;
		}

		private static IGameField<T> ShiftRandom(IGameField<T> gameField)
		{
			var neighbours = gameField.GetLocations(gameField.EmptyCellValue).Single().GetByEdgeNeighbours();
			foreach (var neighbour in neighbours.Shuffle())
				if (gameField.IsInside(neighbour))
					return gameField.Shift(neighbour);
			throw new InvalidOperationException("Unable to shift an empty cell");
		}
	}
}