using System;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GameField.Tests
{
	[TestFixture]
	public class GameFieldShiftTests : GameFieldTestsBase
	{
		[Test]
		public void ShiftSingleTime_UsingValue(
			[ValueSource(nameof(GameFieldFactories))] IGameFieldFactory<int> gameFieldFactory)
		{
			ShiftTest(gameFieldFactory,
				SingleTimeShiftResult,
				f => f.Shift(7));
		}

		[Test]
		public void ShiftSingleTime_UsingLocation(
			[ValueSource(nameof(GameFieldFactories))] IGameFieldFactory<int> gameFieldFactory)
		{
			ShiftTest(gameFieldFactory,
				SingleTimeShiftResult,
				f => f.Shift(new CellLocation(2, 2)));
		}

		[Test]
		public static void ShiftSeveralTimes_UsingValue(
			[ValueSource(nameof(GameFieldFactories))] IGameFieldFactory<int> gameFieldFactory)
		{
			ShiftTest(gameFieldFactory,
				SeveralTimesShiftResult,
				f => f.Shift(7),
				f => f.Shift(6),
				f => f.Shift(6),
				f => f.Shift(7),
				f => f.Shift(8),
				f => f.Shift(4));
		}

		[Test]
		public static void ShiftSeveralTimes_UsingLocation(
			[ValueSource(nameof(GameFieldFactories))] IGameFieldFactory<int> gameFieldFactory)
		{
			ShiftTest(gameFieldFactory,
				SeveralTimesShiftResult,
				f => f.Shift(new CellLocation(2, 2)),
				f => f.Shift(new CellLocation(2, 1)),
				f => f.Shift(new CellLocation(2, 2)),
				f => f.Shift(new CellLocation(1, 2)),
				f => f.Shift(new CellLocation(1, 1)),
				f => f.Shift(new CellLocation(0, 1)));
		}

		[Test]
		public static void FailShift_UsingIncorrectValue(
			[ValueSource(nameof(GameFieldFactories))] IGameFieldFactory<int> gameFieldFactory)
		{
			Action shift = () => ShiftTest(gameFieldFactory, null, f => f.Shift(10));
			shift.ShouldThrow<Exception>();
		}

		[Test]
		public static void FailShift_UsingIncorrectLocation(
			[ValueSource(nameof(GameFieldFactories))] IGameFieldFactory<int> gameFieldFactory)
		{
			Action shift = () => ShiftTest(gameFieldFactory, null, f => f.Shift(new CellLocation(2, -1)));
			shift.ShouldThrow<Exception>();
		}

		private static void ShiftTest(IGameFieldFactory<int> gameFieldFactory,
			int[][] expectedFieldData,
			params Func<IGameField<int>, IGameField<int>>[] shifts)
		{
			var startField = gameFieldFactory.CreateGameField(FieldSize, StartField.GetValue);
			var expectedField = expectedFieldData == null 
				? null 
				: gameFieldFactory.CreateGameField(FieldSize, expectedFieldData.GetValue);

			var result = shifts.Aggregate(startField, (existingField, shift) => shift(existingField));
			result.ShouldBeEquivalentTo(expectedField);
		}

		private static readonly Size FieldSize = new Size(3, 3);

		private static readonly int[][] StartField =
		{
			new[] {1, 4, 5},
			new[] {2, 8, 0},
			new[] {3, 6, 7}
		};

		private static readonly int[][] SingleTimeShiftResult =
		{
			new[] {1, 4, 5},
			new[] {2, 8, 7},
			new[] {3, 6, 0}
		};


		private static readonly int[][] SeveralTimesShiftResult =
		{
			new[] {1, 0, 5},
			new[] {2, 4, 8},
			new[] {3, 6, 7}
		};
	}
}