using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GameField.Wrapping
{
	public class WrappingGameField<T> : GameFieldBase<T>
	{
		public override bool Immutable => true;

		private readonly IGameField<T> parent;
		private readonly CellInfo<T> changedCell;
		
		public WrappingGameField(IGameField<T> source) : this(source.Size)
		{
			parent = source.Clone();
		}

		public WrappingGameField(Size size, Func<CellLocation, T> getValue) : this(size)
		{
			foreach (var location in this.Select(x => x.Location))
			{
				var cell = new CellInfo<T>(location, getValue(location));
				parent = new WrappingGameField<T>(parent ?? new WrappingGameField<T>(size), cell);
			}
		}

		private WrappingGameField(Size size) : base(size) { }

		private WrappingGameField(IGameField<T> source, CellInfo<T> changedCell) : this(source.Size)
		{
			parent = source;
			this.changedCell = changedCell;
		}

		public override IGameField<T> Shift(T value)
		{
			return Shift(() => value, GetLocation(value));
		}

		public override IGameField<T> Shift(CellLocation valueLocation)
		{
			CheckLocation(valueLocation);
			return Shift(() => this[valueLocation], valueLocation);
		}

		private IGameField<T> Shift(Func<T> getValue, CellLocation valueLocation)
		{
			var emptyCellLocation = GetLocation(EmptyCellValue);
			if (emptyCellLocation.GetByEdgeNeighbours().Contains(valueLocation))
			{
				return ApplyChanges(
					new LazyCellInfo<T>(emptyCellLocation, getValue),
					new CellInfo<T>(valueLocation, EmptyCellValue));
			}

			return null;
		}

		private WrappingGameField<T> ApplyChanges(params CellInfo<T>[] newCells)
		{
			return newCells.Aggregate(this, (field, cell) => new WrappingGameField<T>(field, cell));
		}

		public override IEnumerable<CellLocation> GetLocations(T value)
		{
			return this
				.Where(x => x != null && Equals(x.Value, value))
				.Select(x => x.Location);
		}

		public override CellLocation GetLocation(T value)
		{
			if (changedCell != null && Equals(changedCell.Value, value))
				return changedCell.Location;
			if (parent != null)
				return parent.GetLocation(value);
			throw new ArgumentException($"There is no {value} on the field", nameof(value));
		}

		public override T this[CellLocation location]
		{
			get
			{
				if (changedCell != null && Equals(changedCell.Location, location))
					return changedCell.Value;
				if (parent != null)
					return parent[location];
				throw new ArgumentException($"Location {location} is invalid", nameof(location));
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
			return new WrappingGameField<T>(parent, null);
		}
	}
}