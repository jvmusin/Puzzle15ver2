using System;
using System.Drawing;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GameField
{
	public class SingleGameFieldFactory<T> : IGameFieldFactory<T>
	{
		private readonly IGameField<T> gameFieldInstance;

		public SingleGameFieldFactory(IGameField<T> gameFieldInstance)
		{
			this.gameFieldInstance = gameFieldInstance;
		}

		public IGameField<T> CreateGameField(Size size, Func<CellLocation, T> getValue) => gameFieldInstance;

		public IGameField<T> CreateGameField(IGameField<T> source) => gameFieldInstance;
	}
}