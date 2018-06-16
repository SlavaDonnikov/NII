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
		private void Menu_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
			InvisibleGrids();

			switch ((sender as Border).Name)
			{
				case "Menu_Home_Page":
					Grid_Home_Page.Visibility = Visibility.Visible;
					break;
				case "Menu_Projects":
					Grid_Projects.Visibility = Visibility.Visible;
					break;
				case "Menu_Scientists":
					Grid_Scientists.Visibility = Visibility.Visible;
					break;
				case "Menu_Technicians":
					Grid_Technicians.Visibility = Visibility.Visible;
					break;
				case "Menu_Samples":
					Grid_Samples.Visibility = Visibility.Visible;
					break;
				case "Menu_Equipment":
					Grid_Equipment.Visibility = Visibility.Visible;
					break;				

				default: 
					break;
			}
		}
		#endregion

		#region CRUD
		// Modify / Save button
		private void Modify_Button_Click(object sender, RoutedEventArgs e)
		{
			switch ((sender as Button).Name)
			{
				case "Grid_Samples_Modify_Button":
					if (Samples_DataGrid.SelectedItem != null)
					{
						if (TglBtnSample.IsChecked == false)
						{
							TglBtnSample.IsChecked = true;
							TglBtnSample.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
							Samples_DataGrid.IsEnabled = false;
							//Samples_DataGrid.Visibility = Visibility.Collapsed;
						}
						else
						{
							TglBtnSample.IsChecked = false;
							TglBtnSample.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
							Samples_DataGrid.IsEnabled = true;
							//Samples_DataGrid.Visibility = Visibility.Visible;
						}

						if (Grid_Samples_Modify_Button.Content.ToString() == "Modify")
						{
							Grid_Samples_Modify_Button.Content = "Save";
							Grid_Samples_Delete_Button.IsEnabled = false;
							Grid_Samples_CreateNewSample_Button.IsEnabled = false;

							Grid_Samples_Cancel_Button.Visibility = Visibility.Visible;
							Grid_Samples_Cancel_Button.IsEnabled = true;

							using (NIIDbContext db = new NIIDbContext())
							{
								//
							}
						}
						else if (Grid_Samples_Modify_Button.Content.ToString() == "Save")
						{
							Grid_Samples_Modify_Button.Content = "Modify";
							Grid_Samples_Delete_Button.IsEnabled = true;
							Grid_Samples_CreateNewSample_Button.IsEnabled = true;

							Grid_Samples_Cancel_Button.Visibility = Visibility.Collapsed;
							Grid_Samples_Cancel_Button.IsEnabled = false;

							using (NIIDbContext db = new NIIDbContext())
							{
								// Call Save Event or Save function??
							}
						}
					}               // Чтобы не нужно было выбирать строку датагрида при сохранении новой записи (нажата кнопка "Новый", а событие сохранения переадресовывается кнопке "Сохранить")
					else if (Samples_DataGrid.SelectedItem == null & Grid_Samples_CreateNewSample_Button.IsEnabled == false & TglBtnSample.IsChecked == true)
					{
						if (TglBtnSample.IsChecked == false)
						{
							TglBtnSample.IsChecked = true;
							TglBtnSample.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
							Samples_DataGrid.IsEnabled = false;
							//Samples_DataGrid.Visibility = Visibility.Collapsed;
						}
						else
						{
							TglBtnSample.IsChecked = false;
							TglBtnSample.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
							Samples_DataGrid.IsEnabled = true;
							//Samples_DataGrid.Visibility = Visibility.Visible;
						}

						Grid_Samples_Modify_Button.Content = "Modify";
						Grid_Samples_Delete_Button.IsEnabled = true;
						Grid_Samples_CreateNewSample_Button.IsEnabled = true;

						Grid_Samples_Cancel_Button.Visibility = Visibility.Collapsed;
						Grid_Samples_Cancel_Button.IsEnabled = false;

						using (NIIDbContext db = new NIIDbContext())
						{
							// Call Save Event or Save function??
						}
					}
					else MessageBox.Show("Please select target record!", "Modify sample", MessageBoxButton.OK, MessageBoxImage.Warning);
					break;

				case "Grid_Equipment_Modify_Button":
					if (Equipment_DataGrid.SelectedItem != null)
					{
						if (TglBtnEquipment.IsChecked == false)
						{
							TglBtnEquipment.IsChecked = true;
							TglBtnEquipment.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
							Equipment_DataGrid.IsEnabled = false;
							//Equipment_DataGrid.Visibility = Visibility.Collapsed;
						}
						else
						{
							TglBtnEquipment.IsChecked = false;
							TglBtnEquipment.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
							Equipment_DataGrid.IsEnabled = true;
							//Equipment_DataGrid.Visibility = Visibility.Visible;
						}

						if (Grid_Equipment_Modify_Button.Content.ToString() == "Modify")
						{
							Grid_Equipment_Modify_Button.Content = "Save";
							Grid_Equipment_Delete_Button.IsEnabled = false;
							Grid_Equipment_CreateNewPieceOfEquipment_Button.IsEnabled = false;

							Grid_Equipment_Cancel_Button.Visibility = Visibility.Visible;
							Grid_Equipment_Cancel_Button.IsEnabled = true;

							using (NIIDbContext db = new NIIDbContext())
							{
								//
							}
						}
						else if (Grid_Equipment_Modify_Button.Content.ToString() == "Save")
						{
							Grid_Equipment_Modify_Button.Content = "Modify";
							Grid_Equipment_Delete_Button.IsEnabled = true;
							Grid_Equipment_CreateNewPieceOfEquipment_Button.IsEnabled = true;

							Grid_Equipment_Cancel_Button.Visibility = Visibility.Collapsed;
							Grid_Equipment_Cancel_Button.IsEnabled = false;

							using (NIIDbContext db = new NIIDbContext())
							{
								// Call Save Event or Save function??
							}
						}
					}
					else if (Equipment_DataGrid.SelectedItem == null & Grid_Equipment_CreateNewPieceOfEquipment_Button.IsEnabled == false & TglBtnEquipment.IsChecked == true)
					{
						if (TglBtnEquipment.IsChecked == false)
						{
							TglBtnEquipment.IsChecked = true;
							TglBtnEquipment.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
							Equipment_DataGrid.IsEnabled = false;
							//Samples_DataGrid.Visibility = Visibility.Collapsed;
						}
						else
						{
							TglBtnEquipment.IsChecked = false;
							TglBtnEquipment.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
							Equipment_DataGrid.IsEnabled = true;
							//Samples_DataGrid.Visibility = Visibility.Visible;
						}

						Grid_Equipment_Modify_Button.Content = "Modify";
						Grid_Equipment_Delete_Button.IsEnabled = true;
						Grid_Equipment_CreateNewPieceOfEquipment_Button.IsEnabled = true;

						Grid_Equipment_Cancel_Button.Visibility = Visibility.Collapsed;
						Grid_Equipment_Cancel_Button.IsEnabled = false;

						using (NIIDbContext db = new NIIDbContext())
						{
							// Call Save Event or Save function??
						}
					}
					else MessageBox.Show("Please select target record!", "Modify piece of equipment", MessageBoxButton.OK, MessageBoxImage.Warning);
					break;

				default:
					break;
			}
		}

		// Cancel button
		private void Cancel_Button_Click(object sender, RoutedEventArgs e)
		{
			switch ((sender as Button).Name)
			{
				case "Grid_Samples_Cancel_Button":
					TglBtnSample.IsChecked = false;
					TglBtnSample.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

					Samples_DataGrid.IsEnabled = true;
					//Samples_DataGrid.Visibility = Visibility.Visible;

					Grid_Samples_Modify_Button.Content = "Modify";
					Grid_Samples_Delete_Button.IsEnabled = true;
					Grid_Samples_CreateNewSample_Button.IsEnabled = true;

					Grid_Samples_Cancel_Button.Visibility = Visibility.Collapsed;
					Grid_Samples_Cancel_Button.IsEnabled = false;
					break;

				case "Grid_Equipment_Cancel_Button":
					TglBtnEquipment.IsChecked = false;
					TglBtnEquipment.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

					Equipment_DataGrid.IsEnabled = true;
					//Equipment_DataGrid.Visibility = Visibility.Visible;

					Grid_Equipment_Modify_Button.Content = "Modify";
					Grid_Equipment_Delete_Button.IsEnabled =true;
					Grid_Equipment_CreateNewPieceOfEquipment_Button.IsEnabled = true;

					Grid_Equipment_Cancel_Button.Visibility = Visibility.Collapsed;
					Grid_Equipment_Cancel_Button.IsEnabled = false;
					break;

				default:
					break;
			}
		}

		// Create new record button
		private void CreateNew_Button_Click(object sender, RoutedEventArgs e)
		{
			switch ((sender as Button).Name)
			{
				case "Grid_Samples_CreateNewSample_Button":
					TglBtnSample.IsChecked = true;
					TglBtnSample.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

					Grid_Samples_Modify_Button.Content = "Save";

					Grid_Samples_CreateNewSample_Button.IsEnabled = false;
					Grid_Samples_Delete_Button.IsEnabled = false;
					Samples_DataGrid.IsEnabled = false;

					Grid_Samples_Cancel_Button.Visibility = Visibility.Visible;
					Grid_Samples_Cancel_Button.IsEnabled = true;

					// Очистка полей
					break;

				case "Grid_Equipment_CreateNewPieceOfEquipment_Button":
					TglBtnEquipment.IsChecked = true;
					TglBtnEquipment.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

					Grid_Equipment_Modify_Button.Content = "Save";

					Grid_Equipment_CreateNewPieceOfEquipment_Button.IsEnabled = false;
					Grid_Equipment_Delete_Button.IsEnabled = false;
					Equipment_DataGrid.IsEnabled = false;

					Grid_Equipment_Cancel_Button.Visibility = Visibility.Visible;
					Grid_Equipment_Cancel_Button.IsEnabled = true;

					// Очистка полей
					break;

				default:
					break;
			}
		}

		// Delete button
		private void Delete_Button_Click(object sender, RoutedEventArgs e)
		{
			switch ((sender as Button).Name)
			{
				case "Grid_Samples_Delete_Button":
					if (Samples_DataGrid.SelectedItem != null)
					{
						var messageResult = MessageBox.Show("Are you sure?", "Deleting the sample", MessageBoxButton.YesNo, MessageBoxImage.Question);

						if (messageResult == MessageBoxResult.Yes)
						{
							using (NIIDbContext db = new NIIDbContext())
							{
								// Delete functional
							}
						}
					}
					else MessageBox.Show("Please select a record you want to delete!", "Deleting the sample", MessageBoxButton.OK, MessageBoxImage.Warning);
					break;

				case "Grid_Equipment_Delete_Button":
					if (Equipment_DataGrid.SelectedItem != null)
					{
						var messageResult = MessageBox.Show("Are you sure?", "Deleting the piece of equipment", MessageBoxButton.YesNo, MessageBoxImage.Question);

						if (messageResult == MessageBoxResult.Yes)
						{
							using (NIIDbContext db = new NIIDbContext())
							{
								// Delete functional
							}
						}
					}
					else MessageBox.Show("Please select a record you want to delete!", "Deleting the piece of equipment", MessageBoxButton.OK, MessageBoxImage.Warning);
					break;

				default:
					break;
			}
		}
		#endregion

		// --------------------------------------------------------------------------------------------------------------------------------------->
		// Old Modify / Save button
		private void Grid_Samples_Modify_Button_Click(object sender, RoutedEventArgs e)
		{
			if (Samples_DataGrid.SelectedItem != null)
			{
				if (TglBtnSample.IsChecked == false)
				{
					TglBtnSample.IsChecked = true;
					TglBtnSample.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
					Samples_DataGrid.IsEnabled = false;
					//Samples_DataGrid.Visibility = Visibility.Collapsed;
				}
				else
				{
					TglBtnSample.IsChecked = false;
					TglBtnSample.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
					Samples_DataGrid.IsEnabled = true;
					//Samples_DataGrid.Visibility = Visibility.Visible;
				}

				if (Grid_Samples_Modify_Button.Content.ToString() == "Modify")
				{
					Grid_Samples_Modify_Button.Content = "Save";
					Grid_Samples_Delete_Button.IsEnabled = false;
					Grid_Samples_CreateNewSample_Button.IsEnabled = false;

					Grid_Samples_Cancel_Button.Visibility = Visibility.Visible;
					Grid_Samples_Cancel_Button.IsEnabled = true;

					using (NIIDbContext db = new NIIDbContext())
					{
						//
					}
				}
				else if (Grid_Samples_Modify_Button.Content.ToString() == "Save")
				{
					Grid_Samples_Modify_Button.Content = "Modify";
					Grid_Samples_Delete_Button.IsEnabled = true;
					Grid_Samples_CreateNewSample_Button.IsEnabled = true;

					Grid_Samples_Cancel_Button.Visibility = Visibility.Collapsed;
					Grid_Samples_Cancel_Button.IsEnabled = false;

					using (NIIDbContext db = new NIIDbContext())
					{
						// Call Save Event or Save function??
					}
				}
			}               // Чтобы не нужно было выбирать строку датагрида при сохранении новой записи (нажата кнопка "Новый", а событие сохранения переадресовывается кнопке "Сохранить")
			else if (Samples_DataGrid.SelectedItem == null & Grid_Samples_CreateNewSample_Button.IsEnabled == false & TglBtnSample.IsChecked == true)
			{
				if (TglBtnSample.IsChecked == false)
				{
					TglBtnSample.IsChecked = true;
					TglBtnSample.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
					Samples_DataGrid.IsEnabled = false;
					//Samples_DataGrid.Visibility = Visibility.Collapsed;
				}
				else
				{
					TglBtnSample.IsChecked = false;
					TglBtnSample.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
					Samples_DataGrid.IsEnabled = true;
					//Samples_DataGrid.Visibility = Visibility.Visible;
				}

				Grid_Samples_Modify_Button.Content = "Modify";
				Grid_Samples_Delete_Button.IsEnabled = true;
				Grid_Samples_CreateNewSample_Button.IsEnabled = true;

				Grid_Samples_Cancel_Button.Visibility = Visibility.Collapsed;
				Grid_Samples_Cancel_Button.IsEnabled = false;

				using (NIIDbContext db = new NIIDbContext())
				{
					// Call Save Event or Save function??
				}
			}
			else MessageBox.Show("Please select target record!", "Modify sample", MessageBoxButton.OK, MessageBoxImage.Warning);
		}

		// Old Cancel Button (Samples_Grid)
		private void Grid_Samples_Cancel_Button_Click(object sender, RoutedEventArgs e)
		{
			TglBtnSample.IsChecked = false;
			TglBtnSample.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

			Samples_DataGrid.IsEnabled = true;
			//Samples_DataGrid.Visibility = Visibility.Visible;

			Grid_Samples_Modify_Button.Content = "Modify";
			Grid_Samples_Delete_Button.IsEnabled = true;
			Grid_Samples_CreateNewSample_Button.IsEnabled = true;

			Grid_Samples_Cancel_Button.Visibility = Visibility.Collapsed;
			Grid_Samples_Cancel_Button.IsEnabled = false;
		}

		// Old Create new record button
		private void Grid_Samples_CreateNewSample_Button_Click(object sender, RoutedEventArgs e)
		{
			TglBtnSample.IsChecked = true;
			TglBtnSample.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

			Grid_Samples_Modify_Button.Content = "Save";

			Grid_Samples_CreateNewSample_Button.IsEnabled = false;
			Grid_Samples_Delete_Button.IsEnabled = false;
			Samples_DataGrid.IsEnabled = false;

			Grid_Samples_Cancel_Button.Visibility = Visibility.Visible;
			Grid_Samples_Cancel_Button.IsEnabled = true;

			// Очистка полей
		}

		// Old Delete button
		private void Grid_Samples_Delete_Button_Click(object sender, RoutedEventArgs e)
		{
			if (Samples_DataGrid.SelectedItem != null)
			{
				var messageResult = MessageBox.Show("Are you sure?", "Deleting the sample", MessageBoxButton.YesNo, MessageBoxImage.Question);

				if (messageResult == MessageBoxResult.Yes)
				{
					using (NIIDbContext db = new NIIDbContext())
					{
						// Delete functional
					}
				}
			}
			else MessageBox.Show("Please select a record you want to delete!", "Modify sample", MessageBoxButton.OK, MessageBoxImage.Warning);
		}
	}
}
// Chart : https://code.msdn.microsoft.com/Chart-Control-in-WPF-c9727c28  