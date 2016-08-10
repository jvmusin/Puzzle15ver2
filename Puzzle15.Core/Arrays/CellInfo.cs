using System;

namespace Puzzle15.Core.Arrays
{
	public class CellInfo<T> : IEquatable<CellInfo<T>>
	{
		public CellLocation Location { get; }
		public virtual T Value { get; }

		public CellInfo(CellLocation location, T value)
		{
			Location = location;
			Value = value;
		}

		protected CellInfo(CellLocation location)
		{
			Location = location;
		}

		public bool Equals(CellInfo<T> other)
		{
			return other != null &&
			       Equals(Location, other.Location) &&
			       Equals(Value, other.Value);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as CellInfo<T>);
		}

		public override int GetHashCode()
		{
			return Location.GetHashCode() ^ Value.GetHashCode();
		}
	}

	public class LazyCellInfo<T> : CellInfo<T>
	{
		private readonly Lazy<T> valueHolder;

		public override T Value => valueHolder.Value;

		public LazyCellInfo(CellLocation location, Func<T> getValue) : base(location)
		{
			valueHolder = new Lazy<T>(getValue);
		}
	}
}