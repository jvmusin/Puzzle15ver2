using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Puzzle15.GraphicUI
{
	public partial class ButtonPanel : UserControl
	{
		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register(nameof(Title), typeof(string), typeof(ButtonPanel),
				new FrameworkPropertyMetadata("Click me"));

		public static readonly DependencyProperty TitleForegroundProperty =
			DependencyProperty.Register(nameof(TitleForeground), typeof(Brush), typeof(ButtonPanel),
				new PropertyMetadata(Brushes.Gray));

		public static readonly DependencyProperty TitleSizeProperty =
			DependencyProperty.Register(nameof(TitleSize), typeof(int), typeof(ButtonPanel),
				new FrameworkPropertyMetadata(30));

		public static readonly DependencyProperty BackgroundColorProperty =
			DependencyProperty.Register(nameof(BackgroundColor), typeof(Brush), typeof(ButtonPanel),
				new FrameworkPropertyMetadata(Brushes.Black));

		public static readonly DependencyProperty BorderColorProperty =
			DependencyProperty.Register(nameof(BorderColor), typeof(Brush), typeof(ButtonPanel),
				new FrameworkPropertyMetadata(Brushes.Gray));

		public ButtonPanel()
		{
			InitializeComponent();
		}

		public string Title
		{
			get { return (string)GetValue(TitleProperty); }
			set { SetValue(TitleProperty, value); }
		}

		public Brush TitleForeground
		{
			get { return (Brush)GetValue(TitleForegroundProperty); }
			set { SetValue(TitleForegroundProperty, value); }
		}

		public int TitleSize
		{
			get { return (int)GetValue(TitleSizeProperty); }
			set { SetValue(TitleSizeProperty, value); }
		}

		public Brush BackgroundColor
		{
			get { return (Brush)GetValue(BackgroundColorProperty); }
			set { SetValue(BackgroundColorProperty, value); }
		}

		public Brush BorderColor
		{
			get { return (Brush)GetValue(BorderColorProperty); }
			set { SetValue(BorderColorProperty, value); }
		}
	}
}