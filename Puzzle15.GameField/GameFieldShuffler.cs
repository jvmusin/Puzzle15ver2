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
			//TODO:	Add result checking
			if (quality < 0 || quality > 30)
				throw new ArgumentException("Quality should be ranged in [0,30]");

			return Enumerable.Range(0, 1 << quality)
				.Aggregate(gameField, (existingField, i) => ShiftRandom(existingField));
		}

		private static IGameField<T> ShiftRandom(IGameField<T> gameField)
		{
			var neighbours = gameField.GetLocation(gameField.EmptyCellValue).GetByEdgeNeighbours();
			var cellToShift = neighbours.Shuffle().FirstOrDefault(gameField.Contains);

			if (cellToShift == null)
				throw new InvalidOperationException("Unable to shift an empty cell");
			return gameField.Shift(cellToShift);
		}
	}
}