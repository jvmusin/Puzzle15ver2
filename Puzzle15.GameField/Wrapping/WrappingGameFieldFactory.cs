using System;
using System.Drawing;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GameField.Wrapping
{
	public class WrappingGameFieldFactory<T> : IGameFieldFactory<T>
	{
		private readonly IGameFieldFactory<T> backingFieldFactory;

		public WrappingGameFieldFactory(IGameFieldFactory<T> backingFieldFactory)
		{
			this.backingFieldFactory = backingFieldFactory;
		}

		public IGameField<T> CreateGameField(Size size, Func<CellLocation, T> getValue)
		{
			var backingField = backingFieldFactory.CreateGameField(size, getValue);
			return new WrappingGameField<T>(backingField);
		}
	}
}