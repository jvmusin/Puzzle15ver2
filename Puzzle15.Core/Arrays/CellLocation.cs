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

		public static readonly CellLocation DeltaUp = new CellLocation(-1, 0);
		public static readonly CellLocation DeltaDown = new CellLocation(1, 0);
		public static readonly CellLocation DeltaLeft = new CellLocation(0, -1);
		public static readonly CellLocation DeltaRight = new CellLocation(0, 1);
		public static IEnumerable<CellLocation> Deltas = new[] {DeltaUp, DeltaRight, DeltaDown, DeltaLeft};

		public CellLocation(int row, int column)
		{
			Row = row;
			Column = column;
		}

		public CellLocation Mirror() => new CellLocation(-Row, -Column);

		public bool IsInsideField(Size fieldSize)
		{
			return InInRange(Row, 0, fieldSize.Height) &&
			       InInRange(Column, 0, fieldSize.Width);
		}

		private static bool InInRange(int value, int from, int to)
		{
			return from <= value && value < to;
		}

		public IEnumerable<CellLocation> GetByEdgeNeighbours()
		{
			return Deltas.Select(delta => this + delta);
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

		public override string ToString()
		{
			return $"Row: {Row}, Column: {Column}";
		}
	}
}