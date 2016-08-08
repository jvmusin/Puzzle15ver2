using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Puzzle15.Core.Arrays
{
	public class CellLocation : IEquatable<CellLocation>
	{
		public int Row { get; }
		public int Column { get; }

		public CellLocation(int row, int column)
		{
			Row = row;
			Column = column;
		}

		public bool IsInsideField(Size fieldSize)
		{
			return InInRange(Row, 0, fieldSize.Height) &&
			       InInRange(Column, 0, fieldSize.Width);
		}

		private static bool InInRange(int value, int from, int to) => from <= value && value < to;

		public IEnumerable<CellLocation> GetByEdgeNeighbours()
		{
			return GetByEdgeDeltas().Select(delta => this + delta);
		}

		private static IEnumerable<CellLocation> GetByEdgeDeltas()
		{
			var dx = new[] {-1, 0, 1, 0};
			var dy = new[] {0, 1, 0, -1};
			for (var i = 0; i < 4; i++)
				yield return new CellLocation(dx[i], dy[i]);
		}

		public static CellLocation operator +(CellLocation location, CellLocation delta)
		{
			return new CellLocation(location.Row + delta.Row, location.Column + delta.Column);
		}

		public bool Equals(CellLocation other)
		{
			return other != null &&
			       Row == other.Row &&
			       Column == other.Column;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as CellLocation);
		}

		public override int GetHashCode()
		{
			return Row ^ Column;
		}
	}
}