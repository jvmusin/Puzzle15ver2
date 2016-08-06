using System;
using System.Collections.Generic;
using System.Drawing;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GameField
{
	public interface IGameField<TCell> : IEquatable<IGameField<TCell>>, IEnumerable<CellInfo<TCell>>
	{
		Size Size { get; }
		bool Immutable { get; }
		TCell EmptyCellValue { get; }

		IGameField<TCell> Shift(TCell value);
		IGameField<TCell> Shift(CellLocation valueLocation);

		bool IsInside(CellLocation location);
		IEnumerable<CellLocation> GetLocations(TCell value);
		CellLocation GetLocation(TCell value);
		TCell this[CellLocation location] { get; }
		
		IGameField<TCell> Clone();
	}
}