using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoreLinq;
using Puzzle15.Core;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GameField.Immutable
{
	public class ImmutableGameField<TCell> : GameFieldBase<TCell>
	{
		public override bool Immutable => true;

		private readonly TCell[][] field;
		private readonly Dictionary<TCell, List<CellLocation>> locations;
		private readonly List<CellLocation> emptyLocations;

		public ImmutableGameField(Size size, Func<CellLocation, TCell> getValue) : base(size)
		{
			field = ArrayExtensions.CreateField<TCell>(size);

			locations = new Dictionary<TCell, List<CellLocation>>();
			emptyLocations = field.EnumerateLocations().ToList();

			field.EnumerateLocations().ForEach(loc => UpdateCell(loc, getValue(loc)));
			if (emptyLocations.Count != 1)
				throw new InvalidOperationException("Field should contain exactly one default value");
		}

		public override IGameField<TCell> Shift(TCell value)
		{
			return Shift(GetLocation(value));
		}

		public override IGameField<TCell> Shift(CellLocation valueLocation)
		{
			CheckLocation(valueLocation);

			var emptyLocation = emptyLocations.Single();
			if (emptyLocation.GetByEdgeNeighbours().Contains(valueLocation))
			{
				var newField = (ImmutableGameField<TCell>) Clone();
				newField.Swap(emptyLocation, valueLocation);
				return newField;
			}

			return null;
		}

		private void Swap(CellLocation x, CellLocation y)
		{
			var xValue = this[x];
			var yValue = this[y];

			UpdateCell(x, yValue);
			UpdateCell(y, xValue);
		}

		private void UpdateCell(CellLocation location, TCell newValue)
		{
			GetLocationsInternal(this[location]).Remove(location);
			field.SetValue(newValue, location);
			GetLocationsInternal(newValue).Add(location);
		}

		public override IEnumerable<CellLocation> GetLocations(TCell value)
		{
			return GetLocationsInternal(value);
		}

		private List<CellLocation> GetLocationsInternal(TCell value)
		{
			return IsEmptyValue(value)
				? emptyLocations
				: locations.ComputeIfAbsent(value, key => new List<CellLocation>());
		}

		private bool IsEmptyValue(TCell value) => Equals(value, EmptyCellValue);

		public override TCell this[CellLocation location]
		{
			get
			{
				CheckLocation(location);
				return field.GetValue(location);
			}
		}

		public override IEnumerator<CellInfo<TCell>> GetEnumerator() => field.Enumerate().GetEnumerator();

		public override IGameField<TCell> Clone() => new ImmutableGameField<TCell>(Size, loc => this[loc]);
	}
}