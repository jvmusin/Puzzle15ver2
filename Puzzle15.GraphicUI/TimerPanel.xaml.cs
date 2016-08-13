using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Puzzle15.GraphicUI
{
	public partial class TimerPanel : UserControl
	{
		public static readonly DependencyProperty TimeProperty =
			DependencyProperty.Register(nameof(Time), typeof(string), typeof(TimerPanel));

		private readonly DispatcherTimer timer;
		private int elapsedTimeInSeconds;

		public TimerPanel()
		{
			InitializeComponent();

			timer = new DispatcherTimer(
				TimeSpan.FromSeconds(1),
				DispatcherPriority.Normal,
				(s, a) => OnTick(++elapsedTimeInSeconds),
				Dispatcher.CurrentDispatcher);
			timer.Stop();
		}

		public void Start()
		{
			if (timer.IsEnabled)
				timer.Stop();
			
			timer.Start();
			OnTick(elapsedTimeInSeconds = 0);
		}

		public void Stop()
		{
			timer.Stop();
		}

		private void OnTick(int seconds)
		{
			Time = FromSeconds(seconds);
		}

		private static string FromSeconds(int seconds)
		{
			var ts = TimeSpan.FromSeconds(seconds);
			return string.Format($@"{ts:mm\:ss}");
		}

		public string Time
		{
			get { return (string) GetValue(TimeProperty); }
			private set { SetValue(TimeProperty, value); }
		}
	}
}