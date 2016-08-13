using System;
using System.Windows;
using System.Windows.Controls;
using Puzzle15.Game;
using Brush = System.Windows.Media.Brush;
using Size = System.Drawing.Size;

namespace Puzzle15.GraphicUI
{
	public partial class ClassicGameTable : UserControl
	{
		private readonly ResourceDictionary resources = Application.Current.Resources;
		private static readonly Size TableSize = new Size(4, 4);

		public ClassicGameTable()
		{
			InitializeComponent();
		}

		public void UpdateTable(IGame<int> source)
		{
			if (source.FieldSize != TableSize)
				throw new ArgumentException($"Game field size should be {TableSize}", nameof(source.FieldSize));

			foreach (var cellInfo in source.EnumerateField())
			{
				var location = cellInfo.Location;
				var number = cellInfo.Value;
				var empty = number == 0;
				var cell = new ClassicGameCell
				{
					BackgroundColor = (Brush) resources[empty ? "EmptyCell" : "FilledCell"],
					Number = empty ? "" : number.ToString(),
					FontSize = 50,
					Margin = new Thickness(3)
				};
				Table.AddChild(cell, location);
			}
		}
	}
}