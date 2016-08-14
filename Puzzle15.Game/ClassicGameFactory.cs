using System.Drawing;
using Puzzle15.GameField;

namespace Puzzle15.Game
{
	public interface IGameFactory<TCell>
	{
		IGame<TCell> CreateGame(Size fieldSize, int difficulty);
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
			return CreateGame(new Size(fieldSideLength, fieldSideLength), difficulty);
		}

		public IGame<int> CreateGame(Size fieldSize, int difficulty)
		{
			var target = CreateTargetField(fieldSize);
			var startingGameField = gameFieldShuffler.Shuffle(target.Clone(), difficulty);
			return new Game<int>(startingGameField, target.Equals);
		}

		private IGameField<int> CreateTargetField(Size fieldSize)
		{
			var cellsCount = fieldSize.Height*fieldSize.Width;
			return gameFieldFactory.CreateGameField(fieldSize, loc =>
			{
				var value = loc.Row*fieldSize.Width + loc.Column + 1;
				return value%cellsCount;
			});
		}
	}
}