using System;
using System.Drawing;
using FluentAssertions;
using NUnit.Framework;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GameField.Tests
{
	[TestFixture]
	public partial class GameField_Should
	{
		[Test]
		public void ShiftSingleTime_UsingValue(
			[ValueSource(nameof(GameFieldFactories))] IGameFieldFactory<int> gameFieldFactory)
			=> ShiftSingleTime(gameFieldFactory, field => field.Shift(7));

		[Test]
		public void ShiftSingleTime_UsingLocation(
			[ValueSource(nameof(GameFieldFactories))] IGameFieldFactory<int> gameFieldFactory)
			=> ShiftSingleTime(gameFieldFactory, field => field.Shift(new CellLocation(2, 2)));

		private static void ShiftSingleTime(IGameFieldFactory<int> gameFieldFactory,
			Func<IGameField<int>, IGameField<int>> shift)
		{
			var fieldSize = new Size(3, 3);
			var fieldData = new[]
			{
				new[] {1, 4, 5},
				new[] {2, 8, 0},
				new[] {3, 6, 7}
			};
			var expectedFieldData = new[]
			{
				new[] {1, 4, 5},
				new[] {2, 8, 7},
				new[] {3, 6, 0}
			};

			var field = gameFieldFactory.CreateGameField(fieldSize, loc => fieldData.GetValue(loc));
			var expectedField = gameFieldFactory.CreateGameField(fieldSize, loc => expectedFieldData.GetValue(loc));

			shift(field).ShouldBeEquivalentTo(expectedField);
		}
	}
}