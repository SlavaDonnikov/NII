using NII.Database_Classes;
using NII.Support_Classes;
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

			InvisibleGrids();

			Grid_Home_Page.Visibility = Visibility.Visible;

			LoadDB();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			GetFullDate();
			GetRealTime();

            DataContext = new ComboBoxViewModel();
        }

		private void LoadDB()
		{
			using(NIIDbContext db = new NIIDbContext())
			{
				Equipment_DataGrid.ItemsSource = Samples_DataGrid.ItemsSource = null;
				try
				{
					Samples_DataGrid.ItemsSource = db.Samples.ToList();
					Equipment_DataGrid.ItemsSource = db.Equipment.ToList();
				}
				catch (Exception e)
				{
					MessageBox.Show("DB loading error : " + e.Message + ", " + e.Source);
				}
			}
		}
		/// <summary>
		/// Button to move the application window 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			DragMove();
		}

		/// <summary>
		/// Application "Power" button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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

		private void InvisibleGrids()
		{
			Grid_Home_Page.Visibility = Visibility.Collapsed;
			Grid_Samples.Visibility = Visibility.Collapsed;
			Grid_Equipment.Visibility = Visibility.Collapsed;
		}

		private void Menu_Samples_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			InvisibleGrids();
			Grid_Samples.Visibility = Visibility.Visible;
		}

		private void Menu_Home_Page_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			InvisibleGrids();
			Grid_Home_Page.Visibility = Visibility.Visible;
		}

		private void Menu_Equipment_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			InvisibleGrids();
			Grid_Equipment.Visibility = Visibility.Visible;
		}
	}
}
