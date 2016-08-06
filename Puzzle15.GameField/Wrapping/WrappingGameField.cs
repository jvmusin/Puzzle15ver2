using System;
using System.Collections.Generic;
using System.Linq;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GameField.Wrapping
{
	public class WrappingGameField<T> : GameFieldBase<T>
	{
		public override bool Immutable => true;

		private readonly IGameField<T> parent;
		private readonly CellInfo<T> changedCell;

		public WrappingGameField(IGameField<T> source) : base(source.Size)
		{
			parent = source;
		}

		private WrappingGameField(IGameField<T> source, CellInfo<T> changedCell) : this(source)
		{
			this.changedCell = changedCell;
		}

		public override IGameField<T> Shift(T value)
		{
			return Shift(() => value, GetLocation(value));
		}

		public override IGameField<T> Shift(CellLocation valueLocation)
		{
			return Shift(() => this[valueLocation], valueLocation);
		}

		private IGameField<T> Shift(Func<T> getValue, CellLocation valueLocation)
		{
			var emptyCellLocation = GetLocation(EmptyCellValue);

			if (emptyCellLocation.GetByEdgeNeighbours().Contains(valueLocation))
			{
				var newValueCell = new CellInfo<T>(emptyCellLocation, getValue());
				var newEmptyCell = new CellInfo<T>(valueLocation, EmptyCellValue);

				var result = new WrappingGameField<T>(this, newValueCell);
				return new WrappingGameField<T>(result, newEmptyCell);
			}

			return null;
		}

		public override IEnumerable<CellLocation> GetLocations(T value)
		{
			return this
				.Where(x => x != null && Equals(x.Value, value))
				.Select(x => x.Location);
		}

		public override CellLocation GetLocation(T value)
		{
			return changedCell != null && Equals(changedCell.Value, value)
				? changedCell.Location
				: parent.GetLocation(value);
		}

		public override T this[CellLocation location]
		{
			get
			{
				return changedCell != null && Equals(changedCell.Location, location)
					? changedCell.Value
					: parent[location];
			}
		}

		public override IEnumerator<CellInfo<T>> GetEnumerator()
		{
			return ArrayExtensions.EnumerateLocations(Size)
				.Select(loc => new LazyCellInfo<T>(loc, () => this[loc]))
				.GetEnumerator();
		}

		public override IGameField<T> Clone()
		{
			return new WrappingGameField<T>(parent);
		}
	}
}