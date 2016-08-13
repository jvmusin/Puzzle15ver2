using System.Windows;
using System.Windows.Controls;

namespace Puzzle15.GraphicUI
{
	public partial class InfoPanel : UserControl
	{
		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register(nameof(Title), typeof(string), typeof(InfoPanel),
				new FrameworkPropertyMetadata("Happiness"));

		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register(nameof(Value), typeof(string), typeof(InfoPanel),
				new FrameworkPropertyMetadata("42"));

		public static readonly DependencyProperty TitleSizeProperty =
			DependencyProperty.Register(nameof(TitleSize), typeof(int), typeof(InfoPanel),
				new FrameworkPropertyMetadata(25));

		public static readonly DependencyProperty ValueSizeProperty =
			DependencyProperty.Register(nameof(ValueSize), typeof(int), typeof(InfoPanel),
				new FrameworkPropertyMetadata(30));

		public InfoPanel()
		{
			InitializeComponent();
		}

		public string Title
		{
			get { return (string) GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public string Value
		{
			get { return (string) GetValue(ValueProperty); }
			set { SetValue(ValueProperty, value); }
		}

		public int TitleSize
		{
			get { return (int) GetValue(TitleSizeProperty); }
			set { SetValue(TitleSizeProperty, value); }
		}

		public int ValueSize
		{
			get { return (int) GetValue(ValueSizeProperty); }
			set { SetValue(ValueSizeProperty, value); }
		}
	}
}