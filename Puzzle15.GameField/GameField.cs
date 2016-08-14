using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MoreLinq;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GameField
{
	public class GameField<TCell> : GameFieldBase<TCell>
	{
		private readonly TCell[][] field;
		private readonly ConcurrentDictionary<TCell, List<CellLocation>> locations;
		private readonly List<CellLocation> emptyValueLocations;

		public GameField(Size size, Func<CellLocation, TCell> getValue, bool immutable) : base(size, immutable)
		{
			field = ArrayExtensions.CreateField<TCell>(size);

			locations = new ConcurrentDictionary<TCell, List<CellLocation>>();
			emptyValueLocations = field.EnumerateLocations().ToList();

			field.EnumerateLocations().ForEach(loc => UpdateCell(loc, getValue(loc)));
			CheckDefaultValuesCount();
		}

		private void UpdateCell(CellLocation location, TCell newValue)
		{
			GetLocationsInternal(this[location]).Remove(location);
			field.SetValue(newValue, location);
			GetLocationsInternal(newValue).Add(location);
		}

		public override IGameField<TCell> Shift(TCell value)
		{
			return Shift(GetLocation(value));
		}

		public override IGameField<TCell> Shift(CellLocation valueLocation)
		{
			CheckLocation(valueLocation);

			var emptyValueLocation = emptyValueLocations.Single();
			if (emptyValueLocation.GetByEdgeNeighbours().Contains(valueLocation))
			{
				var resultingField = (GameField<TCell>) (Immutable ? Clone() : this);
				resultingField.Swap(emptyValueLocation, valueLocation);
				return resultingField;
			}

			throw new InvalidLocationException($"There is no empty cell around {valueLocation}");
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
				: locations.GetOrAdd(value, x => new List<CellLocation>());
		}

		public override TCell this[CellLocation location]
		{
			get
			{
				CheckLocation(location);
				return field.GetValue(location);
			}
		}

		public override IEnumerator<CellInfo<TCell>> GetEnumerator()
		{
			return field.Enumerate().GetEnumerator();
		}

		public override IGameField<TCell> Clone()
		{
			return new GameField<TCell>(Size, loc => this[loc], Immutable);
		}
	}
}