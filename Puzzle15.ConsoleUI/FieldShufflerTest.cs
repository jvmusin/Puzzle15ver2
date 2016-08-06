using System;
using System.Drawing;
using Puzzle15.Core.Arrays;
using Puzzle15.GameField;

namespace Puzzle15.ConsoleUI
{
	public class FieldShufflerTest
	{
		private readonly IGameFieldFactory<int> gameFieldFactory;
		private readonly IGameFieldShuffler<int> gameFieldShuffler;

		public FieldShufflerTest(IGameFieldFactory<int> gameFieldFactory, IGameFieldShuffler<int> gameFieldShuffler)
		{
			this.gameFieldFactory = gameFieldFactory;
			this.gameFieldShuffler = gameFieldShuffler;
		}

		public void Run()
		{
			var field = GetField();
			Console.WriteLine(field);
			const string prefix = "\n----------------------------------------------\n";
			for (var i = 0; i < 1000; i++)
				Console.WriteLine(prefix + (field = gameFieldShuffler.Shuffle(field, 0)));
		}

		private IGameField<int> GetField()
		{
			var fieldSize = new Size(4, 4);
			var fieldArea = fieldSize.Width*fieldSize.Height;
			Func<CellLocation, int> indexer = loc => (loc.Row*fieldSize.Width + loc.Column + 1) % fieldArea;
			return gameFieldFactory.CreateGameField(fieldSize, indexer);
		}
	}
}