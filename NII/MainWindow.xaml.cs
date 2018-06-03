using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NII   // Программа ведения базы данных "Сотрудники" научного учреждения "Прогресс"
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			GetFullDate();
			GetRealTime();
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			DragMove();
		}

		private void Application_Shutdown_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		#region Application Full Format Date & Time
		public void GetFullDate()
		{
			ApplicationFullDate.Content = DateTime.Now.ToLongDateString();
			// In ru-RU culture : ApplicationFullDate.Content = DateTime.Now.DayOfWeek.ToString() + ", " + DateTime.Now.ToLongDateString();
		}

		public void GetRealTime()
		{
			System.Timers.Timer Timer = new System.Timers.Timer
			{
				Interval = 1000
			};
			Timer.Elapsed += Timer_Elapsed;
			Timer.Start();
		}

		private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			try
			{
				Dispatcher.Invoke(() => ApplicationRealTime.Content = DateTime.Now.ToLongTimeString());
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error : " + ex.Message + ", " + ex.Source);
			}

			// In ru-RU culture : ApplicationRealTime.Content = DateTime.Now.ToLongTimeString() + " " + DateTime.Now.AddHours(12).ToString("tt", CultureInfo.InvariantCulture)
		}
		#endregion
	}
}
