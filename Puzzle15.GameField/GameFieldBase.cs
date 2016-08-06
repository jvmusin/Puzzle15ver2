using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GameField
{
	public abstract class GameFieldBase<TCell> : IGameField<TCell>
	{
		public Size Size { get; }
		public abstract bool Immutable { get; }
		public TCell EmptyCellValue => default(TCell);

		protected GameFieldBase(Size size)
		{
			Size = size;
		}

		public abstract IGameField<TCell> Shift(TCell value);
		public abstract IGameField<TCell> Shift(CellLocation valueLocation);

		public bool IsInside(CellLocation location)
		{
			var row = location.Row;
			var col = location.Column;

			return 0 <= row && row < Size.Height &&
			       0 <= col && col < Size.Width;
		}

		public abstract IEnumerable<CellLocation> GetLocations(TCell value);

		public virtual CellLocation GetLocation(TCell value) => GetLocations(value).Single();

		public abstract TCell this[CellLocation location] { get; }

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public abstract IEnumerator<CellInfo<TCell>> GetEnumerator();

		public abstract IGameField<TCell> Clone();

		public bool Equals(IGameField<TCell> other)
		{
			return other != null &&
			       Equals(Size, other.Size) &&
			       this.SequenceEqual(other);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as IGameField<TCell>);
		}

		public override int GetHashCode()
		{
			return 0;
		}
	}
}