using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Puzzle15.Core.Arrays;
using Puzzle15.GameField;

namespace Puzzle15
{
	public interface IGame<TCell>
	{
		Size FieldSize { get; }
		int Turns { get; }
		bool Finished { get; }
		
		bool Shift(TCell value);
		bool Shift(CellLocation valueLocation);

		bool Undo();

		TCell this[CellLocation location] { get; }
		CellLocation GetLocation(TCell value);
		IEnumerable<CellLocation> GetLocations(TCell value);

		IEnumerable<CellInfo<TCell>> EnumerateField();
	}

	public class ClassicGame : IGame<int>
	{
		public Size FieldSize => CurrentGameField.Size;
		public int Turns => CurrentGameData.Turns;
		public bool Finished => isFinished(CurrentGameField);
		private readonly Predicate<IGameField<int>> isFinished;

		private readonly Stack<GameData> history;
		private GameData CurrentGameData => history.Peek();
		private IGameField<int> CurrentGameField => CurrentGameData.GameField;

		public ClassicGame(IGameField<int> startingGameField, Predicate<IGameField<int>> finishedCondition)
		{
			isFinished = finishedCondition;

			history = new Stack<GameData>();
			history.Push(new GameData(0, startingGameField.Clone()));
		}

		public bool Shift(int value)
		{
			return Shift(GetLocation(value));
		}

		public bool Shift(CellLocation valueLocation)
		{
			if (Finished)
				return false;

			var newField = CurrentGameField.Shift(valueLocation);
			if (newField != null)
			{
				history.Push(new GameData(Turns + 1, newField));
				return true;
			}
			return false;
		}

		public bool Undo()
		{
			if (!CurrentGameField.Immutable)
				throw new InvalidOperationException("Operation is allowed only for immutable fields");

			if (history.Count > 1)
			{
				history.Pop();
				return true;
			}
			return false;
		}

		public int this[CellLocation location] => CurrentGameField[location];
		public CellLocation GetLocation(int value) => GetLocations(value).Single();
		public IEnumerable<CellLocation> GetLocations(int value) => CurrentGameField.GetLocations(value);

		public IEnumerable<CellInfo<int>> EnumerateField() => CurrentGameField;

		private class GameData
		{
			public int Turns { get; }
			public IGameField<int> GameField { get; }

			public GameData(int turns, IGameField<int> gameField)
			{
				Turns = turns;
				GameField = gameField;
			}
		}
	}
}