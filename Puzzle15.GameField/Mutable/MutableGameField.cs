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
		private readonly List<CellLocation> emptyLocations;

		public MutableGameField(Size size, Func<CellLocation, TCell> getValue) : base(size)
		{
			field = ArrayExtensions.CreateField<TCell>(size);

			locations = new Dictionary<TCell, List<CellLocation>>();
			emptyLocations = field.EnumerateLocations().ToList();

			field.EnumerateLocations().ForEach(loc => UpdateCell(loc, getValue(loc)));
			if (field.Enumerate().Select(x => x.Value).Count(IsEmptyValue) != 1)
				throw new InvalidOperationException("Field should contain only one default value");
		}

		public override IGameField<TCell> Shift(TCell value)
		{
			return Shift(GetLocations(value).Single());
		}

		public override IGameField<TCell> Shift(CellLocation valueLocation)
		{
			var emptyLocation = emptyLocations.Single();
			
			if (emptyLocation.GetByEdgeNeighbours().Contains(valueLocation))
			{
				var value = this[valueLocation];
				UpdateCell(emptyLocation, value);
				UpdateCell(valueLocation, EmptyCellValue);
				return this;
			}

			return null;
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

		public override TCell this[CellLocation location] => field.GetValue(location);
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