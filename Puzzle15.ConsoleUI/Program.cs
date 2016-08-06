using System;
using Ninject;
using Puzzle15.Core.Arrays;
using Puzzle15.GameField;
using Puzzle15.GameField.Immutable;

namespace Puzzle15.ConsoleUI
{
	public class Program
	{
		private readonly IGameFactory<int> gameFactory;
		private IGame<int> game;

		public Program(IGameFactory<int> gameFactory)
		{
			this.gameFactory = gameFactory;
		}

		public static void Main()
		{
			var kernel = new StandardKernel();
			kernel.Bind<IGameFieldFactory<int>>().To<ImmutableGameFieldFactory<int>>();
			kernel.Bind<IGameFieldShuffler<int>>().To<GameFieldShuffler<int>>();
			kernel.Bind<IGameFactory<int>>().To<ClassicGameFactory>();
			kernel.Get<Program>().Run();
		}

		private void Run()
		{
			game = gameFactory.CreateGame(InputSideLength(), InputDifficulty());
			while (!game.Finished)
			{
				PrintGame(game);
				while (!DoAction(game)) ;
				Console.WriteLine("\n----------------------\n");
			}
			PrintCongratulations(game);
		}

		private static void PrintGame(IGame<int> game)
		{
			PrintGameTitle(game);
			PrintGameField(game);
		}

		private static void PrintGameTitle(IGame<int> game)
		{
			Console.WriteLine("Turns: " + game.Turns);
		}

		private static void PrintGameField(IGame<int> game)
		{
			var height = game.FieldSize.Height;
			var width = game.FieldSize.Width;

			for (var row = 0; row < height; row++)
			{
				for (var column = 0; column < width; column++)
				{
					Console.Write(game[new CellLocation(row, column)] + " ");
				}
				Console.WriteLine();
			}
		}

		private static void PrintCongratulations(IGame<int> game)
		{
			Console.WriteLine($"YAY! {game.Turns} turns done!");
		}

		private static bool DoAction(IGame<int> game)
		{
			Console.Write("Input action: ");
			var parts = Console.ReadLine().Split(' ');
			switch (parts[0])
			{
				case "move":
					var row = int.Parse(parts[1]);
					var column = int.Parse(parts[2]);
					return game.Shift(new CellLocation(row, column));
				case "undo":
					return game.Undo();
				default:
					return false;
			}
		}

		private static int InputSideLength() => InputData("Input side length: ");

		private static int InputDifficulty() => InputData("Input difficulty: ");

		private static int InputData(string message)
		{
			Console.Write(message);
			return ReadInt();
		}

		private static int ReadInt() => int.Parse(Console.ReadLine());
	}
}
