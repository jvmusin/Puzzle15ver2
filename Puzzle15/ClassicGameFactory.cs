using System.Drawing;
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
			var target = CreateTargetField(fieldSideLength);
			var startingGameField = gameFieldShuffler.Shuffle(target.Clone(), difficulty);
			return new ClassicGame(startingGameField, f => f.Equals(target));
		}

		private IGameField<int> CreateTargetField(int fieldSideLength)
		{
			var fieldSize = new Size(fieldSideLength, fieldSideLength);
			var cellsCount = fieldSideLength*fieldSideLength;
			return gameFieldFactory.CreateGameField(fieldSize, loc =>
			{
				var value = loc.Row*fieldSideLength + loc.Column + 1;
				return value%cellsCount;
			});
		}
	}
}