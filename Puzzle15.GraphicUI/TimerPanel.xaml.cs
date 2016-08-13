using System.Windows;
using System.Windows.Controls;

namespace Puzzle15.GraphicUI
{
	public partial class TimerPanel : UserControl
	{
		public static readonly DependencyProperty TimeProperty =
			DependencyProperty.Register(nameof(Time), typeof(string), typeof(TimerPanel),
				new FrameworkPropertyMetadata("17 : 03"));

		public TimerPanel()
		{
			InitializeComponent();
		}

		public string Time
		{
			get { return (string) GetValue(TimeProperty); }
			set { SetValue(TimeProperty, value); }
		}
	}
}