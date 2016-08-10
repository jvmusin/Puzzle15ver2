using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoreLinq;
using Puzzle15.Core;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GameField.Mutable
{
	public class MutableGameField<TCell> : GameFieldBase<TCell>
	{
		public override bool Immutable => false;
		
		private readonly TCell[][] field;
		private readonly Dictionary<TCell, List<CellLocation>> locations;
		private readonly List<CellLocation> emptyValueLocations;

		public MutableGameField(Size size, Func<CellLocation, TCell> getValue) : base(size)
		{
			field = ArrayExtensions.CreateField<TCell>(size);

			locations = new Dictionary<TCell, List<CellLocation>>();
			emptyValueLocations = field.EnumerateLocations().ToList();

			field.EnumerateLocations().ForEach(loc => UpdateCell(loc, getValue(loc)));
			CheckDefaultValuesCount();
		}

		public override IGameField<TCell> Shift(TCell value)
		{
			return Shift(GetLocation(value));
		}

		public override IGameField<TCell> Shift(CellLocation valueLocation)
		{
			CheckLocation(valueLocation);

			var emptyLocation = emptyValueLocations.Single();
			if (emptyLocation.GetByEdgeNeighbours().Contains(valueLocation))
			{
				Swap(emptyLocation, valueLocation);
				return this;
			}

			throw new InvalidLocationException($"The is no empty cell around {valueLocation}");
		}

		private void Swap(CellLocation x, CellLocation y)
		{
			var xValue = this[x];
			var yValue = this[y];

			UpdateCell(x, yValue);
			UpdateCell(y, xValue);
		}

		public override IEnumerable<CellLocation> GetLocations(TCell value)
		{
			return GetLocationsInternal(value);
		}

		private List<CellLocation> GetLocationsInternal(TCell value)
		{
			return IsEmptyValue(value) 
				? emptyValueLocations
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

		private void UpdateCell(CellLocation location, TCell newValue)
		{
			GetLocationsInternal(this[location]).Remove(location);
			field.SetValue(newValue, location);
			GetLocationsInternal(newValue).Add(location);
		}
		
		public override IEnumerator<CellInfo<TCell>> GetEnumerator() => field.Enumerate().GetEnumerator();

		public override IGameField<TCell> Clone() => new MutableGameField<TCell>(Size, loc => this[loc]);
	}
}