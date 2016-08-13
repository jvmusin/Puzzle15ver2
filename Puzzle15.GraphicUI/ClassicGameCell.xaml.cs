using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Puzzle15.GraphicUI
{
	public partial class ClassicGameCell : UserControl
	{
		public static readonly DependencyProperty NumberProperty =
			DependencyProperty.Register(nameof(Number), typeof(string), typeof(ClassicGameCell),
				new FrameworkPropertyMetadata("-1"));

		public static readonly DependencyProperty BackgroundColorProperty =
			DependencyProperty.Register(nameof(BackgroundColor), typeof(Brush), typeof(ClassicGameCell),
				new FrameworkPropertyMetadata(Brushes.RosyBrown));

		public ClassicGameCell()
		{
			InitializeComponent();
		}

		public string Number
		{
			get { return (string) GetValue(NumberProperty); }
			set { SetValue(NumberProperty, value); }
		}

		public Brush BackgroundColor
		{
			get { return (Brush) GetValue(BackgroundColorProperty); }
			set { SetValue(BackgroundColorProperty, value); }
		}
	}
}