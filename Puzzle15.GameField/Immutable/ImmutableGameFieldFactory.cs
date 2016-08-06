﻿using System;
using System.Drawing;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GameField.Immutable
{
	public class ImmutableGameFieldFactory<TCell> : IGameFieldFactory<TCell>
	{
		public IGameField<TCell> CreateGameField(Size size, Func<CellLocation, TCell> getValue)
		{
			return new ImmutableGameField<TCell>(size, getValue);
		}
	}
}