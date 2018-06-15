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
		public List<string> Project_CodeName { get; set; }

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

        #region Load DB
        /// <summary>
        /// Loading data from DB with Context
        /// </summary>
        private void LoadDB()
		{
			using(NIIDbContext db = new NIIDbContext())
			{
				Equipment_DataGrid.ItemsSource = Samples_DataGrid.ItemsSource = null;
                try
                {                      
                    Projects_DataGrid.ItemsSource = db.Projects.Include("Scientists").Include("Technicians").Include("Samples").Include("Equipments").ToList();      
                    Scientists_DataGrid.ItemsSource = db.Scientists.Include("Projects").ToList();
                    Technicians_DataGrid.ItemsSource = db.Technicians.Include("Projects").ToList();
                    Samples_DataGrid.ItemsSource = db.Samples.Include("Projects").ToList();                    
					Equipment_DataGrid.ItemsSource = db.Equipment.Include("Projects").ToList();					
				}
				catch (Exception e)
				{
					MessageBox.Show("DB loading error : " + e.Message + ", " + e.Source);
				}
			}
		}
        #endregion

        #region Window DragMove()
        /// <summary>
        /// Button to move the application window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			DragMove();
		}
        #endregion

        #region Application Shutdown
        /// <summary>
        /// Application "Power" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_Shutdown_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}
        #endregion

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

        #region InvisibleGrids()
        private void InvisibleGrids()
		{
			Grid_Home_Page.Visibility = Visibility.Collapsed;
			Grid_Projects.Visibility = Visibility.Collapsed;
			Grid_Scientists.Visibility = Visibility.Collapsed;
			Grid_Technicians.Visibility = Visibility.Collapsed;
			Grid_Samples.Visibility = Visibility.Collapsed;
			Grid_Equipment.Visibility = Visibility.Collapsed;
		}
        #endregion

        #region Menu Buttons Clicks
        private void Menu_Home_Page_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			InvisibleGrids();
			Grid_Home_Page.Visibility = Visibility.Visible;
		}

		private void Menu_Projects_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			InvisibleGrids();
			Grid_Projects.Visibility = Visibility.Visible;
		}

		private void Menu_Scientists_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			InvisibleGrids();
			Grid_Scientists.Visibility = Visibility.Visible;
		}

		private void Menu_Technicians_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			InvisibleGrids();
			Grid_Technicians.Visibility = Visibility.Visible;
		}

		private void Menu_Samples_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			InvisibleGrids();
			Grid_Samples.Visibility = Visibility.Visible;
		}		

		private void Menu_Equipment_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			InvisibleGrids();
			Grid_Equipment.Visibility = Visibility.Visible;
		}
        #endregion
    }
}
// Chart : https://code.msdn.microsoft.com/Chart-Control-in-WPF-c9727c28  