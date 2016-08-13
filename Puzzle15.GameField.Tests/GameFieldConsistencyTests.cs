using System;
using System.Drawing;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GameField.Tests
{
	[TestFixture]
	public class GameFieldConsistencyTests : GameFieldTestsBase
	{
		[Test]
		public void FieldModifiesAfterShift_IfNotImmutable(
			[ValueSource(nameof(GameFieldFactories))] IGameFieldFactory<int> gameFieldFactory)
		{
			var gameField = gameFieldFactory.CreateGameField(new Size(3, 3), loc => loc.Row*3 + loc.Column);
			var clonedGameField = gameField.Clone();

			var shiftResult = gameField.Shift(1);

			var equalsToCloned = gameField.Equals(clonedGameField);
			var equalsToResult = gameField.Equals(shiftResult);

			if (gameField.Immutable)
			{
				equalsToCloned.Should().BeTrue();
				equalsToResult.Should().BeFalse();
			}
			else
			{
				equalsToCloned.Should().BeFalse();
				equalsToResult.Should().BeTrue();
			}
		}

		[Test]
		public void FieldHasCorrectValueLocations(
			[ValueSource(nameof(GameFieldFactories))] IGameFieldFactory<int> gameFieldFactory)
		{
			var fieldData = new[]
			{
				new[] {1, 0, 4},
				new[] {4, 2, 9},
				new[] {9, 100, 8}
			};
			var gameField = gameFieldFactory.CreateGameField(new Size(3, 3), fieldData.GetValue);

			gameField.GetLocations(4).Should().BeEquivalentTo(
				new CellLocation(0, 2),
				new CellLocation(1, 0));
			gameField.GetLocations(0).Single().Should().Be(new CellLocation(0, 1));
			gameField.GetLocations(5).Should().BeEmpty();
		}

		[Test]
		public void FieldHasCorrectValueLocation(
			[ValueSource(nameof(GameFieldFactories))] IGameFieldFactory<int> gameFieldFactory)
		{
			var fieldData = new[]
			{
				new[] {1, 0, 4},
				new[] {4, 2, 9},
				new[] {9, 100, 8}
			};
			var gameField = gameFieldFactory.CreateGameField(new Size(3, 3), fieldData.GetValue);

			gameField.GetLocation(2).Should().Be(new CellLocation(1, 1));
		}

		[Test]
		public void FieldThrowsExceptionOnGetLocation_IfThereIsNotOnlyOneCorrectValue(
			[ValueSource(nameof(ClassicGameFieldFactories))] IGameFieldFactory<int> gameFieldFactory,
			[Values(2, 10)] int valueToFind)
		{
			var fieldData = new[]
			{
				new[] {1, 0, 2},
				new[] {4, 2, 9},
				new[] {9, 100, 8}
			};
			var gameField = gameFieldFactory.CreateGameField(new Size(3, 3), fieldData.GetValue);

			Assert.Throws<ArgumentException>(() => gameField.GetLocation(valueToFind));
		}

		[Test]
		public void FieldThrowsException_IfTwoDefaultValues(
			[ValueSource(nameof(GameFieldFactories))] IGameFieldFactory<int> gameFieldFactory)
		{
			var fieldData = new[]
			{
				new[] {1, 0, 4},
				new[] {4, 2, 9},
				new[] {9, 0, 8}
			};
			TestIncorrectDefaultValuesCount(gameFieldFactory, new Size(3, 3), fieldData);
		}

		[Test]
		public void FieldThrowsException_IfZeroDefaultValues(
			[ValueSource(nameof(GameFieldFactories))] IGameFieldFactory<int> gameFieldFactory)
		{
			var fieldData = new[]
			{
				new[] {1, -2, 4},
				new[] {4, 2, 9},
				new[] {9, 88, 8}
			};
			TestIncorrectDefaultValuesCount(gameFieldFactory, new Size(3, 3), fieldData);
		}

		private static void TestIncorrectDefaultValuesCount(IGameFieldFactory<int> gameFieldFactory, Size fieldSize,
			int[][] fieldData)
		{
			var fakeField = GetFakeGameField(fieldSize, fieldData);

			Assert.Throws<InvalidOperationException>(
				() => gameFieldFactory.CreateGameField(new Size(3, 3), fieldData.GetValue));
			Assert.Throws<InvalidOperationException>(
				() => gameFieldFactory.CreateGameField(fakeField));
		}

		[Test]
		public void TestIndexer(
			[ValueSource(nameof(GameFieldFactories))] IGameFieldFactory<int> gameFieldFactory)
		{
			var fieldData = new[]
			{
				new[] {1, -2, 4},
				new[] {4, 2, 0},
				new[] {9, 88, 8}
			};
			var gameField = gameFieldFactory.CreateGameField(new Size(3, 3), fieldData.GetValue);

			fieldData.Enumerate()
				.Select(x => Equals(x.Value, gameField[x.Location]))
				.ShouldAllBeEquivalentTo(true);
		}

		[Test]
		public void FieldNotChangesIfImmutable_AfterShiftingClonedField(
			[ValueSource(nameof(GameFieldFactories))] IGameFieldFactory<int> gameFieldFactory)
		{
			var fieldData = new[]
			{
				new[] {1, -2, 4},
				new[] {4, 2, 0},
				new[] {9, 88, 8}
			};
			var gameField = gameFieldFactory.CreateGameField(new Size(3, 3), fieldData.GetValue);
			var clonedField = gameField.Clone();

			clonedField.Shift(2);

			gameField.SequenceEqual(fieldData.Enumerate()).Should().BeTrue();
			clonedField.Equals(gameField).Should().Be(gameField.Immutable);
		}

		private static IGameField<int> GetFakeGameField(Size fieldSize, int[][] fieldData)
		{
			var fakeField = A.Fake<IGameField<int>>(x => x.Strict());

			A.CallTo(() => fakeField[A<CellLocation>._])
				.ReturnsLazily(x => fieldData.GetValue((CellLocation) x.Arguments[0]));
			A.CallTo(() => fakeField.GetEnumerator()).ReturnsLazily(x => fieldData.Enumerate().GetEnumerator());
			A.CallTo(() => fakeField.Size).Returns(fieldSize);
			A.CallTo(() => fakeField.Clone()).ReturnsLazily(x => GetFakeGameField(fieldSize, fieldData));

			return fakeField;
		}
	}
}