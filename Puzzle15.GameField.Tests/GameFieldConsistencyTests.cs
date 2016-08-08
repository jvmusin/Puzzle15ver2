using System.Drawing;
using FluentAssertions;
using NUnit.Framework;

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
	}
}