using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using Ninject;
using Puzzle15.Core.Arrays;
using Puzzle15.Game;
using Puzzle15.GraphicUI.Utils;

namespace Puzzle15.GraphicUI
{
	public partial class MainWindow : Window
	{
		private readonly IGameFactory<int> gameFactory;

		private static readonly int difficulty = 3;
		private static readonly int fieldSideLength = 4;
		private static readonly Dictionary<Key, CellLocation> Deltas;

		private event Action<IGame<int>> GameUpdated;
		
		private IGame<int> Game { get; set; }

		static MainWindow()
		{
			Deltas = new Dictionary<Key, CellLocation>
			{
				[Key.Up] = CellLocation.DeltaUp,
				[Key.Right] = CellLocation.DeltaRight,
				[Key.Down] = CellLocation.DeltaDown,
				[Key.Left] = CellLocation.DeltaLeft
			};
		}

		public MainWindow()
		{
			InitializeComponent();

			var kernel = new StandardKernel(new ClassicGameNinjectModule());
			gameFactory = kernel.Get<IGameFactory<int>>();

			GameUpdated += GameTable.UpdateTable;
			GameUpdated += game => Difficulty.Value = difficulty.ToString();
			GameUpdated += game => Turns.Value = Game.Turns.ToString();
			GameUpdated += game =>
			{
				if (game.Finished)
				{
					Timer.Stop();
					MessageBox.Show(this, "You win!");
				}
			};
		}

		private void OnGameUpdated()
		{
			GameUpdated?.Invoke(Game);
		}

		private void BackButtonHandle(object sender, MouseButtonEventArgs e)
		{
			if (Game.Undo())
				OnGameUpdated();
		}

		private void NewGameButtonHandle(object sender, MouseButtonEventArgs e)
		{
			Game = gameFactory.CreateGame(fieldSideLength, difficulty);
			Timer.Start();
			OnGameUpdated();
		}

		private void KeyPressedHandle(object sender, KeyEventArgs e)
		{
			if (Game == null || Game.Finished)
				return;

			CellLocation delta;
			if (!Deltas.TryGetValue(e.Key, out delta))
				return;

			try
			{
				var numberToShiftLocation = Game.GetLocation(0) + delta.Mirror();
				if (Game.Shift(numberToShiftLocation))
					OnGameUpdated();
			}
			catch (InvalidLocationException)
			{
				MessageBox.Show(this, "Invalid turn");
			}
		}
	}
}