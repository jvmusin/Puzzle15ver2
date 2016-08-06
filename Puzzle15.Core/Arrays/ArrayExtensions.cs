using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Puzzle15.Core.Arrays
{
	public static class ArrayExtensions
	{
		public static T[][] CreateField<T>(Size size)
		{
			return Enumerable.Range(0, size.Height)
				.Select(x => new T[size.Width])
				.ToArray();
		}

		public static void Fill<T>(this T[][] field, Func<CellLocation, T> getValue)
		{
			foreach (var location in field.EnumerateLocations())
			{
				var newValue = getValue(location);
				field.SetValue(newValue, location);
			}
		}

		public static void Swap<T>(this T[][] field, CellLocation x, CellLocation y)
		{
			var xValue = field.GetValue(x);
			var yValue = field.GetValue(y);

			field.SetValue(xValue, y);
			field.SetValue(yValue, x);
		}

		public static IEnumerable<CellInfo<T>> Enumerate<T>(this T[][] field)
		{
			return field.EnumerateLocations()
				.Select(loc => new CellInfo<T>(loc, field.GetValue(loc)));
		}

		public static IEnumerable<CellLocation> EnumerateLocations<T>(this T[][] field)
		{
			return EnumerateLocations(field.GetSize());
		}

		public static IEnumerable<CellLocation> EnumerateLocations(Size fieldSize)
		{
			var height = fieldSize.Height;
			var width = fieldSize.Width;

			for (var row = 0; row < height; row++)
				for (var column = 0; column < width; column++)
					yield return new CellLocation(row, column);
		}

		public static Size GetSize<T>(this T[][] field)
		{
			var height = field.GetHeight();
			var width = field.GetWidth();
			return new Size(height, width);
		}

		public static int GetHeight<T>(this T[][] field)
		{
			return field.Length;
		}

		public static int GetWidth<T>(this T[][] field)
		{
			return field.Any() ? field[0].Length : 0;
		}

		public static T GetValue<T>(this T[][] field, CellLocation location)
		{
			return field[location.Row][location.Column];
		}

		public static void SetValue<T>(this T[][] field, T value, CellLocation location)
		{
			field[location.Row][location.Column] = value;
		}
	}
}