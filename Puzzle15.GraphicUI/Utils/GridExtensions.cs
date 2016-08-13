using System.Windows;
using System.Windows.Controls;
using Puzzle15.Core.Arrays;

namespace Puzzle15.GraphicUI.Utils
{
	public static class GridExtensions
	{
		public static void AddChild(this Grid grid, UIElement element, CellLocation location)
		{
			grid.Children.Add(element);
			Grid.SetRow(element, location.Row);
			Grid.SetColumn(element, location.Column);
		}
	}
}