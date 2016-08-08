using System.Drawing;
using NUnit.Framework;

namespace Puzzle15.GameField.Tests
{
	[TestFixture]
	public class GameFieldConsistencyTests : GameFieldTestsBase
	{
		[Test]
		public void ModifyFieldIfNotImmutable(
			[ValueSource(nameof(GameFieldFactories))] IGameFieldFactory<int> gameFieldFactory)
		{
			gameFieldFactory.CreateGameField(new Size(3, 3), loc => loc.Row*3 + loc.Column);
		}
	}
}