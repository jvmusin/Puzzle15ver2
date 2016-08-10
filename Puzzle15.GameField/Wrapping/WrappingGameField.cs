using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GameField.Wrapping
{
	public class WrappingGameField<TCell> : GameFieldBase<TCell>
	{
		public override bool Immutable => true;

		private readonly IGameField<TCell> parent;
		private readonly CellInfo<TCell> changedCell;
		
		public WrappingGameField(IGameField<TCell> source) : this(source.Size)
		{
			parent = source.Clone();
			CheckDefaultValuesCount();
		}

		public WrappingGameField(Size size, Func<CellLocation, TCell> getValue) : this(size)
		{
			foreach (var location in this.Select(x => x.Location))
			{
				var cell = new CellInfo<TCell>(location, getValue(location));
				parent = new WrappingGameField<TCell>(parent ?? new WrappingGameField<TCell>(size), cell);
			}
			CheckDefaultValuesCount();
		}

		private WrappingGameField(Size size) : base(size) { }

		private WrappingGameField(IGameField<TCell> source, CellInfo<TCell> changedCell) : this(source.Size)
		{
			parent = source;
			this.changedCell = changedCell;
		}

		public override IGameField<TCell> Shift(TCell value)
		{
			return Shift(() => value, GetLocation(value));
		}

		public override IGameField<TCell> Shift(CellLocation valueLocation)
		{
			CheckLocation(valueLocation);
			return Shift(() => this[valueLocation], valueLocation);
		}

		private IGameField<TCell> Shift(Func<TCell> getValue, CellLocation valueLocation)
		{
			var emptyCellLocation = GetLocation(EmptyCellValue);
			if (emptyCellLocation.GetByEdgeNeighbours().Contains(valueLocation))
			{
				return ApplyChanges(
					new LazyCellInfo<TCell>(emptyCellLocation, getValue),
					new CellInfo<TCell>(valueLocation, EmptyCellValue));
			}

			throw new InvalidLocationException($"The is no empty cell around {valueLocation}");
		}

		private WrappingGameField<TCell> ApplyChanges(params CellInfo<TCell>[] changedCells)
		{
			return changedCells.Aggregate(this, (field, cell) => new WrappingGameField<TCell>(field, cell));
		}

		public override IEnumerable<CellLocation> GetLocations(TCell value)
		{
			return this
				.Where(x => x != null && Equals(x.Value, value))
				.Select(x => x.Location);
		}

		public override CellLocation GetLocation(TCell value)
		{
			if (changedCell != null && Equals(changedCell.Value, value))
				return changedCell.Location;
			if (parent != null)
				return parent.GetLocation(value);
			throw new ArgumentException($"There is no {value} on the field", nameof(value));
		}

		public override TCell this[CellLocation location]
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

		public override IEnumerator<CellInfo<TCell>> GetEnumerator()
		{
			return ArrayExtensions.EnumerateLocations(Size)
				.Select(loc => new LazyCellInfo<TCell>(loc, () => this[loc]))
				.GetEnumerator();
		}

		public override IGameField<TCell> Clone()
		{
			return new WrappingGameField<TCell>(parent, null);
		}
	}
}