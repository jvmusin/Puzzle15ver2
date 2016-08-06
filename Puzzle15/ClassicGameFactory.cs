using System.Drawing;
using Puzzle15.Core.Arrays;
using Puzzle15.GameField;

namespace Puzzle15
{
	public interface IGameFactory<TCell>
	{
		IGame<TCell> CreateGame(int fieldSideLength, int difficulty);
	}

	public class ClassicGameFactory : IGameFactory<int>
	{
		private readonly IGameFieldFactory<int> gameFieldFactory;
		private readonly IGameFieldShuffler<int> gameFieldShuffler;

		public ClassicGameFactory(IGameFieldFactory<int> gameFieldFactory, IGameFieldShuffler<int> gameFieldShuffler)
		{
			this.gameFieldFactory = gameFieldFactory;
			this.gameFieldShuffler = gameFieldShuffler;
		}

		public IGame<int> CreateGame(int fieldSideLength, int difficulty)
		{
			var gameField = gameFieldShuffler.Shuffle(GetTarget(fieldSideLength), difficulty);
			var target = GetTarget(fieldSideLength);
			return new ClassicGame(gameField, target);
		}

		private IGameField<int> GetTarget(int fieldSideLength)
		{
			var fieldSize = new Size(fieldSideLength, fieldSideLength);
			var cellsCount = fieldSideLength*fieldSideLength;
			return gameFieldFactory.CreateGameField(fieldSize, loc => GetIndex(loc, fieldSideLength)%cellsCount);
		}

		private static int GetIndex(CellLocation location, int fieldWidth)
		{
			return location.Row*fieldWidth + location.Column + 1;
		}
	}
}