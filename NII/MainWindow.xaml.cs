using NII.Database_Classes;
using NII.Support_Classes;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                    Projects_DataGrid.ItemsSource = db.Projects.Include("Scientists").Include("Technicians").Include("Samples").Include("Equipment").ToList();      
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

        #region Find Control
        /// <summary>
        /// Helper function for searching all controls of the specified type.
        /// </summary>
        /// <typeparam name="T">Type of control.</typeparam>
        /// <param name="depObj">Where to look for controls.</param>
        /// <returns>Enumerable list of controls.</returns>
        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
        #endregion

        #region Window ClearFocus()
        /// <summary>
        /// Button to move the application window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{			
            Keyboard.ClearFocus();
        }
        #endregion

        #region RootGrid : DragMove(), UnselectAll()
        private void Grid_DragMove_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }

        private void Grid_DataGrid_UnselectAll_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Grid rootGrid = sender as Grid;
            if (e.RightButton == MouseButtonState.Released)
            {
                foreach (DataGrid dataGrid in FindVisualChildren<DataGrid>(RootGrid))
                {
                    dataGrid.UnselectAll();
                }
            }
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

        #region About Application Button
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private void About_Application_Button_Click(object sender, RoutedEventArgs e)
        {
            InvisibleGrids();
            Grid_About_Application.Visibility = Visibility.Visible;
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
			Grid_About_Application.Visibility = Visibility.Collapsed;
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

        #region Clear all input fields
        // Clear all textBoxes, comboBoxes, datePickers at 'Create New' panels
        private void ClearInputFields()
        {
            foreach (TextBox textBox in FindVisualChildren<TextBox>(RootGrid))
            {
                textBox.Clear();
            }
            foreach (ComboBox comboBox in FindVisualChildren<ComboBox>(RootGrid))
            {
                comboBox.SelectedIndex = -1;
            }
            foreach (DatePicker datePicker in FindVisualChildren<DatePicker>(RootGrid))
            {
                datePicker.SelectedDate = null;
            }

			UnceslectAllListBoxes_and_CloseAllExpanders();
		}
		#endregion

		#region Unselect All ListBoxes
		private void UnceslectAllListBoxes_and_CloseAllExpanders()
		{
			ListBox_Projects_Technicians.UnselectAll();
			ListBox_Projects_Scientists.UnselectAll();
			ListBox_Projects_Samples.UnselectAll();
			ListBox_Projects_Equipment.UnselectAll();

			Expander_Projects_Equipment.IsExpanded = false;
			Expander_Projects_Samples.IsExpanded = false;
			Expander_Projects_Scientists.IsExpanded = false;
			Expander_Projects_Technicians.IsExpanded = false;
		}
		#endregion

		#region Is Input Fields Are Not Empty
		// Check is all input fields are filled up
		private bool IsInputFieldsAreNotEmpty_Project(DependencyObject obj)
        {
            List<bool> txt = new List<bool>(7);
            List<bool> lstbx = new List<bool>(4);
            List<bool> dtp = new List<bool>(1);

            foreach (TextBox child in FindVisualChildren<TextBox>(obj))
            {
                if (!string.IsNullOrEmpty(child.Text) & !string.IsNullOrWhiteSpace(child.Text)) txt.Add(true);
            }
            foreach (ListBox child in FindVisualChildren<ListBox>(obj))
            {
                if (child.SelectedItems != null & child.SelectedIndex != -1) lstbx.Add(true);
            }
            foreach (DatePicker child in FindVisualChildren<DatePicker>(obj))
            {
                if (child.SelectedDate.Value != null) dtp.Add(true);
            }

            if ((txt.Any(a => a == true) & txt.Count == 7) & (lstbx.Any(a => a == true) & lstbx.Count == 4) & (dtp.Any(a => a == true) & dtp.Count == 1)) return true;
            else return false;
        }

        private bool IsInputFieldsAreNotEmpty_Scientists_Technicians(DependencyObject obj)
        {          
            List<bool> txt = new List<bool>(5);
            List<bool> cmb = new List<bool>(2);
            List<bool> dtp = new List<bool>(1);

            foreach (TextBox child in FindVisualChildren<TextBox>(obj))
            {
                if (!string.IsNullOrEmpty(child.Text) & !string.IsNullOrWhiteSpace(child.Text)) txt.Add(true);
            }
            foreach (ComboBox child in FindVisualChildren<ComboBox>(obj))
            {
                if (child.SelectedItem != null & child.SelectedIndex != -1) cmb.Add(true);
            }
            foreach (DatePicker child in FindVisualChildren<DatePicker>(obj))
            {
                if (child.SelectedDate.Value != null) dtp.Add(true);
            }

            if ((txt.Any(a => a == true) & txt.Count == 5) & (cmb.Any(a => a == true) & cmb.Count == 2) & (dtp.Any(a => a == true) & dtp.Count == 1)) return true;
            else return false;            
        }

        private bool IsInputFieldsAreNotEmpty_Samples_Equipment(DependencyObject obj)
        {
            List<bool> txt = new List<bool>(3);
            foreach (TextBox child in FindVisualChildren<TextBox>(obj))
            {
                if (!string.IsNullOrEmpty(child.Text) & !string.IsNullOrWhiteSpace(child.Text)) txt.Add(true);
            }

            if(txt.Any(a => a == true) & txt.Count == 3) return true;
            else return false;
        }
        #endregion

        #region Number TextBox Validation
        /// <summary>
        /// Check is string represents a number
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool IsNumber(string text)
        {
            return int.TryParse(text, out int output);
        }

        private void InputValidation_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsNumber(e.Text) == false) e.Handled = true;            
        }
        #endregion

        // Modify / Save button event
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
						}
						else
						{
							TglBtnSample.IsChecked = false;
							TglBtnSample.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
							Samples_DataGrid.IsEnabled = true;							
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
                                Sample sample = new Sample();
                                int id = (Samples_DataGrid.SelectedItem as Sample).Id;
                                sample = db.Samples.Find(id);

                                TextBox_Samples_Title.Text = sample.Title;
                                TextBox_Samples_Quantity.Text = sample.Quantity.ToString();
                                TextBox_Samples_Description.Text = sample.Description; 
                            }
						}           
						else if (Grid_Samples_Modify_Button.Content.ToString() == "Save")   // Save MODIFIED Sample
                        {
							Grid_Samples_Modify_Button.Content = "Modify";
							Grid_Samples_Delete_Button.IsEnabled = true;
							Grid_Samples_CreateNewSample_Button.IsEnabled = true;

							Grid_Samples_Cancel_Button.Visibility = Visibility.Collapsed;
							Grid_Samples_Cancel_Button.IsEnabled = false;

                            if (!IsInputFieldsAreNotEmpty_Samples_Equipment(Grid_Samples))
                            {
                                MessageBox.Show("All fields must be filled!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                using (NIIDbContext db = new NIIDbContext())
                                {
                                    Sample sample = new Sample();
                                    int id = (Samples_DataGrid.SelectedItem as Sample).Id;
                                    sample = db.Samples.Find(id);
                                    db.Entry(sample).State = EntityState.Modified;

                                    sample.Title = TextBox_Samples_Title.Text;
                                    sample.Quantity = Convert.ToInt32(TextBox_Samples_Quantity.Text);
                                    sample.Description = TextBox_Samples_Description.Text;
                                    
                                    db.SaveChanges();

                                    LoadDB();
                                }
                            }
						}
					}                                                               // Save NEW Sample
					else if (Samples_DataGrid.SelectedItem == null & Grid_Samples_CreateNewSample_Button.IsEnabled == false & TglBtnSample.IsChecked == true)
					{
						
						TglBtnSample.IsChecked = false;
						TglBtnSample.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
						Samples_DataGrid.IsEnabled = true;										

						Grid_Samples_Modify_Button.Content = "Modify";
						Grid_Samples_Delete_Button.IsEnabled = true;
						Grid_Samples_CreateNewSample_Button.IsEnabled = true;

						Grid_Samples_Cancel_Button.Visibility = Visibility.Collapsed;
						Grid_Samples_Cancel_Button.IsEnabled = false;

                        if (!IsInputFieldsAreNotEmpty_Samples_Equipment(Grid_Samples))
                        {
                            MessageBox.Show("All fields must be filled!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            using (NIIDbContext db = new NIIDbContext())
                            {
                                Sample sample = new Sample
                                {
                                    Title = TextBox_Samples_Title.Text,
                                    Quantity = Convert.ToInt32(TextBox_Samples_Quantity.Text),
                                    Description = TextBox_Samples_Description.Text
                                };
                                db.Samples.Add(sample);
                                db.SaveChanges();

                                LoadDB();
                            }
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
						}
						else
						{
							TglBtnEquipment.IsChecked = false;
							TglBtnEquipment.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
							Equipment_DataGrid.IsEnabled = true;
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
                                Equipment equipment = new Equipment();
                                int id = (Equipment_DataGrid.SelectedItem as Equipment).Id;
                                equipment = db.Equipment.Find(id);

                                TextBox_Equipment_Title.Text = equipment.Title;
                                TextBox_Equipment_Quantity.Text = equipment.Quantity.ToString();
                                TextBox_Equipment_Description.Text = equipment.Description;
                            }
						}
						else if (Grid_Equipment_Modify_Button.Content.ToString() == "Save")     // Save MODIFIED Equipment
                        {
							Grid_Equipment_Modify_Button.Content = "Modify";
							Grid_Equipment_Delete_Button.IsEnabled = true;
							Grid_Equipment_CreateNewPieceOfEquipment_Button.IsEnabled = true;

							Grid_Equipment_Cancel_Button.Visibility = Visibility.Collapsed;
							Grid_Equipment_Cancel_Button.IsEnabled = false;

                            if (!IsInputFieldsAreNotEmpty_Samples_Equipment(Grid_Equipment))
                            {
                                MessageBox.Show("All fields must be filled!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                using (NIIDbContext db = new NIIDbContext())
                                {
                                    Equipment equipment = new Equipment();
                                    int id = (Equipment_DataGrid.SelectedItem as Equipment).Id;
                                    equipment = db.Equipment.Find(id);
                                    db.Entry(equipment).State = EntityState.Modified;

                                    equipment.Title = TextBox_Equipment_Title.Text;
                                    equipment.Quantity = Convert.ToInt32(TextBox_Equipment_Quantity.Text);
                                    equipment.Description = TextBox_Equipment_Description.Text;

                                    db.SaveChanges();

                                    LoadDB();
                                }
                            }
                        }
					}                                               // Save NEW Equipment
					else if (Equipment_DataGrid.SelectedItem == null & Grid_Equipment_CreateNewPieceOfEquipment_Button.IsEnabled == false & TglBtnEquipment.IsChecked == true)
					{
						
						TglBtnEquipment.IsChecked = false;
						TglBtnEquipment.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
						Equipment_DataGrid.IsEnabled = true;

						Grid_Equipment_Modify_Button.Content = "Modify";
						Grid_Equipment_Delete_Button.IsEnabled = true;
						Grid_Equipment_CreateNewPieceOfEquipment_Button.IsEnabled = true;

						Grid_Equipment_Cancel_Button.Visibility = Visibility.Collapsed;
						Grid_Equipment_Cancel_Button.IsEnabled = false;

                        if (!IsInputFieldsAreNotEmpty_Samples_Equipment(Grid_Equipment))
                        {
                            MessageBox.Show("All fields must be filled!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            using (NIIDbContext db = new NIIDbContext())
                            {
                                Equipment equipment = new Equipment
                                {
                                    Title = TextBox_Equipment_Title.Text,
                                    Quantity = Convert.ToInt32(TextBox_Equipment_Quantity.Text),
                                    Description = TextBox_Equipment_Description.Text
                                };
                                db.Equipment.Add(equipment);
                                db.SaveChanges();

                                LoadDB();
                            }
                        }
                    }
					else MessageBox.Show("Please select target record!", "Modify piece of equipment", MessageBoxButton.OK, MessageBoxImage.Warning);
					break;

				case "Grid_Technicians_Modify_Button":
					if (Technicians_DataGrid.SelectedItem != null)
					{
						if (TglBtnTechnicians.IsChecked == false)
						{
							TglBtnTechnicians.IsChecked = true;
							TglBtnTechnicians.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
							Technicians_DataGrid.IsEnabled = false;
						}
						else
						{
							TglBtnTechnicians.IsChecked = false;
							TglBtnTechnicians.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
							Technicians_DataGrid.IsEnabled = true;
						}

						if (Grid_Technicians_Modify_Button.Content.ToString() == "Modify")
						{
							Grid_Technicians_Modify_Button.Content = "Save";
							Grid_Technicians_Delete_Button.IsEnabled = false;
							Grid_Technicians_CreateNewTechnician_Button.IsEnabled = false;

							Grid_Technicians_Cancel_Button.Visibility = Visibility.Visible;
							Grid_Technicians_Cancel_Button.IsEnabled = true;

                            using (NIIDbContext db = new NIIDbContext())
                            {
                                Technician technician = new Technician();
                                int id = (Technicians_DataGrid.SelectedItem as Technician).Id;
                                technician = db.Technicians.Find(id);

                                TextBox_Technicians_Name.Text = technician.Name;
                                TextBox_Technicians_Age.Text = technician.Age.ToString();
                                TextBox_Technicians_Personal_Id.Text = technician.Personal_Identification_Number;
                                ComboBox_Technicians_Position.SelectedValue = technician.Position;
                                ComboBox_Technicians_Qualification.SelectedValue = technician.Qualification;
                                TextBox_Technicians_EducationalBackground.Text = technician.EducationalBackground;
                                DatePicker_Technicians_DateOfEmployment.SelectedDate = technician.DateOfEmployment;
                            }
                        }
						else if (Grid_Technicians_Modify_Button.Content.ToString() == "Save")       // Save MODIFIED Technician
                        {
							Grid_Technicians_Modify_Button.Content = "Modify";
							Grid_Technicians_Delete_Button.IsEnabled = true;
							Grid_Technicians_CreateNewTechnician_Button.IsEnabled = true;

							Grid_Technicians_Cancel_Button.Visibility = Visibility.Collapsed;
							Grid_Technicians_Cancel_Button.IsEnabled = false;

                            if (!IsInputFieldsAreNotEmpty_Scientists_Technicians(Grid_Technicians))
                            {
                                MessageBox.Show("All fields must be filled!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                using (NIIDbContext db = new NIIDbContext())
                                {
                                    Technician technician = new Technician();
                                    int id = (Technicians_DataGrid.SelectedItem as Technician).Id;
                                    technician = db.Technicians.Find(id);
                                    db.Entry(technician).State = EntityState.Modified;

                                    technician.Name = TextBox_Technicians_Name.Text;
                                    technician.Age = Convert.ToInt32(TextBox_Technicians_Age.Text);
                                    technician.Personal_Identification_Number = TextBox_Technicians_Personal_Id.Text;
                                    technician.Position = ComboBox_Technicians_Position.SelectedValue.ToString();
                                    technician.Qualification = ComboBox_Technicians_Qualification.SelectedValue.ToString();
                                    technician.EducationalBackground = TextBox_Technicians_EducationalBackground.Text;
                                    technician.DateOfEmployment = DatePicker_Technicians_DateOfEmployment.SelectedDate.Value;
                                        
                                    db.SaveChanges();

                                    LoadDB();
                                }
                            }
                        }
					}                                   // Save NEW Technician
					else if (Technicians_DataGrid.SelectedItem == null & Grid_Technicians_CreateNewTechnician_Button.IsEnabled == false & TglBtnTechnicians.IsChecked == true)
					{
						
						TglBtnTechnicians.IsChecked = false;
						TglBtnTechnicians.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
						Technicians_DataGrid.IsEnabled = true;

						Grid_Technicians_Modify_Button.Content = "Modify";
						Grid_Technicians_Delete_Button.IsEnabled = true;
						Grid_Technicians_CreateNewTechnician_Button.IsEnabled = true;

						Grid_Technicians_Cancel_Button.Visibility = Visibility.Collapsed;
						Grid_Technicians_Cancel_Button.IsEnabled = false;

                        if (!IsInputFieldsAreNotEmpty_Scientists_Technicians(Grid_Technicians))
                        {
                            MessageBox.Show("All fields must be filled!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            using (NIIDbContext db = new NIIDbContext())
                            {
                                Technician technician = new Technician
                                {
                                    Name = TextBox_Technicians_Name.Text,
                                    Age = Convert.ToInt32(TextBox_Technicians_Age.Text),
                                    Personal_Identification_Number = TextBox_Technicians_Personal_Id.Text,
                                    Position = ComboBox_Technicians_Position.SelectedValue.ToString(),
                                    Qualification = ComboBox_Technicians_Qualification.SelectedValue.ToString(),
                                    EducationalBackground = TextBox_Technicians_EducationalBackground.Text,
                                    DateOfEmployment = DatePicker_Technicians_DateOfEmployment.SelectedDate.Value
                                };
                                db.Technicians.Add(technician);
                                db.SaveChanges();

                                LoadDB();
                            }
                        }
                    }
					else MessageBox.Show("Please select target record!", "Modify a technician", MessageBoxButton.OK, MessageBoxImage.Warning);
					break;

				case "Grid_Scientists_Modify_Button":
					if (Scientists_DataGrid.SelectedItem != null)
					{
						if (TglBtnScientists.IsChecked == false)
						{
							TglBtnScientists.IsChecked = true;
							TglBtnScientists.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
							Scientists_DataGrid.IsEnabled = false;
						}
						else
						{
							TglBtnScientists.IsChecked = false;
							TglBtnScientists.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
							Scientists_DataGrid.IsEnabled = true;
						}

						if (Grid_Scientists_Modify_Button.Content.ToString() == "Modify")
						{
							Grid_Scientists_Modify_Button.Content = "Save";
							Grid_Scientists_Delete_Button.IsEnabled = false;
							Grid_Scientists_CreateNewScientist_Button.IsEnabled = false;

							Grid_Scientists_Cancel_Button.Visibility = Visibility.Visible;
							Grid_Scientists_Cancel_Button.IsEnabled = true;

                            using (NIIDbContext db = new NIIDbContext())
                            {
                                Scientist scientist = new Scientist();
                                int id = (Scientists_DataGrid.SelectedItem as Scientist).Id;
                                scientist = db.Scientists.Find(id);

                                TextBox_Scientists_Name.Text = scientist.Name;
                                TextBox_Scientists_Age.Text = scientist.Age.ToString();
                                TextBox_Scientists_Personal_Id.Text = scientist.Personal_Identification_Number;
                                ComboBox_Scientists_Position.SelectedValue = scientist.Position;
                                ComboBox_Scientists_Qualification.SelectedValue = scientist.Qualification;
                                TextBox_Scientists_EducationalBackground.Text = scientist.EducationalBackground;
                                DatePicker_Scientists_DateOfEmployment.SelectedDate = scientist.DateOfEmployment;
                            }
                        }
						else if (Grid_Scientists_Modify_Button.Content.ToString() == "Save")    // Save MODIFIED Scientist
                        {
							Grid_Scientists_Modify_Button.Content = "Modify";
							Grid_Scientists_Delete_Button.IsEnabled = true;
							Grid_Scientists_CreateNewScientist_Button.IsEnabled = true;

							Grid_Scientists_Cancel_Button.Visibility = Visibility.Collapsed;
							Grid_Scientists_Cancel_Button.IsEnabled = false;

                            if (!IsInputFieldsAreNotEmpty_Scientists_Technicians(Grid_Scientists))
                            {
                                MessageBox.Show("All fields must be filled!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                using (NIIDbContext db = new NIIDbContext())
                                {
                                    Scientist scientist = new Scientist();
                                    int id = (Scientists_DataGrid.SelectedItem as Scientist).Id;
                                    scientist = db.Scientists.Find(id);
                                    db.Entry(scientist).State = EntityState.Modified;

                                    scientist.Name = TextBox_Scientists_Name.Text;
                                    scientist.Age = Convert.ToInt32(TextBox_Scientists_Age.Text);
                                    scientist.Personal_Identification_Number = TextBox_Scientists_Personal_Id.Text;
                                    scientist.Position = ComboBox_Scientists_Position.SelectedValue.ToString();
                                    scientist.Qualification = ComboBox_Scientists_Qualification.SelectedValue.ToString();
                                    scientist.EducationalBackground = TextBox_Scientists_EducationalBackground.Text;
                                    scientist.DateOfEmployment = DatePicker_Scientists_DateOfEmployment.SelectedDate.Value;

                                    db.SaveChanges();

                                    LoadDB();
                                }
                            }
                        }
                    }                                                        // Save NEW Scientist
                    else if (Scientists_DataGrid.SelectedItem == null & Grid_Scientists_CreateNewScientist_Button.IsEnabled == false & TglBtnScientists.IsChecked == true)
					{

						TglBtnScientists.IsChecked = false;
						TglBtnScientists.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
						Scientists_DataGrid.IsEnabled = true;

						Grid_Scientists_Modify_Button.Content = "Modify";
						Grid_Scientists_Delete_Button.IsEnabled = true;
						Grid_Scientists_CreateNewScientist_Button.IsEnabled = true;

						Grid_Scientists_Cancel_Button.Visibility = Visibility.Collapsed;
						Grid_Scientists_Cancel_Button.IsEnabled = false;

                        if (!IsInputFieldsAreNotEmpty_Scientists_Technicians(Grid_Scientists))
                        {
                            MessageBox.Show("All fields must be filled!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            using (NIIDbContext db = new NIIDbContext())
                            {
                                Scientist scientist = new Scientist
                                {
                                    Name = TextBox_Scientists_Name.Text,
                                    Age = Convert.ToInt32(TextBox_Scientists_Age.Text),
                                    Personal_Identification_Number = TextBox_Scientists_Personal_Id.Text,
                                    Position = ComboBox_Scientists_Position.SelectedValue.ToString(),
                                    Qualification = ComboBox_Scientists_Qualification.SelectedValue.ToString(),
                                    EducationalBackground = TextBox_Scientists_EducationalBackground.Text,
                                    DateOfEmployment = DatePicker_Scientists_DateOfEmployment.SelectedDate.Value
                                };
                                db.Scientists.Add(scientist);
                                db.SaveChanges();

                                LoadDB();
                            }
                        }
                    }
					else MessageBox.Show("Please select target record!", "Modify a scientist", MessageBoxButton.OK, MessageBoxImage.Warning);
					break;

				case "Grid_Projects_Modify_Button":
					if (Projects_DataGrid.SelectedItem != null)
					{
						if (TglBtnProjects.IsChecked == false)
						{
							TglBtnProjects.IsChecked = true;
							TglBtnProjects.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
							Projects_DataGrid.IsEnabled = false;
						}
						else
						{
							TglBtnProjects.IsChecked = false;
							TglBtnProjects.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
							Projects_DataGrid.IsEnabled = true;
						}

						if (Grid_Projects_Modify_Button.Content.ToString() == "Modify")
						{
							Grid_Projects_Modify_Button.Content = "Save";
							Grid_Projects_Delete_Button.IsEnabled = false;
							Grid_Projects_CreateNewProject_Button.IsEnabled = false;

							Grid_Projects_Cancel_Button.Visibility = Visibility.Visible;
							Grid_Projects_Cancel_Button.IsEnabled = true;													

							using (NIIDbContext db = new NIIDbContext())
							{
								Project project = new Project();
								int id = (Projects_DataGrid.SelectedItem as Project).Id;
								project = db.Projects.Find(id);

								TextBox_Projects_Name.Text = project.Name;
								TextBox_Projects_CodeName.Text = project.CodeName;
								TextBox_Projects_Location.Text = project.Location;
								TextBox_Projects_Term.Text = project.Term.ToString();
								TextBox_Projects_Cost.Text = project.Cost.ToString();
								DatePicker_Projects_DateOfBeginning.SelectedDate = project.DateOfBeginning;
								TextBox_Projects_Description.Text = project.Description;
								
								List<Sample> samplesFromProject = db.Samples.Where(s => s.Projects.Any(p => p.Id == id)).ToList();
								List<string> samplesTitles = new List<string>();
								foreach (Sample smp in samplesFromProject)
								{
									samplesTitles.Add(smp.Title);
								}
								foreach (string title in samplesTitles)
								{
									ListBox_Projects_Samples.SelectedItems.Add(title);
								}

								List<Equipment> equipmentFromProject = db.Equipment.Where(eq => eq.Projects.Any(p => p.Id == id)).ToList();
								List<string> equipmentTitles = new List<string>();
								foreach (Equipment eq in equipmentFromProject)
								{
									equipmentTitles.Add(eq.Title);
								}
								foreach (string title in equipmentTitles)
								{
									ListBox_Projects_Equipment.SelectedItems.Add(title);
								}

								List<Scientist> scientistsFromProject = db.Scientists.Where(sc => sc.Projects.Any(p => p.Id == id)).ToList();
								List<string> scientistsNames = new List<string>();
								foreach (Scientist sc in scientistsFromProject)
								{
									scientistsNames.Add(sc.Name);
								}
								foreach(string name in scientistsNames)
								{
									ListBox_Projects_Scientists.SelectedItems.Add(name);
								}

								List<Technician> techniciansFromProject = db.Technicians.Where(th => th.Projects.Any(p => p.Id == id)).ToList();
								List<string> techniciansNames = new List<string>();
								foreach(Technician th in techniciansFromProject)
								{
									techniciansNames.Add(th.Name);
								}
								foreach(string name in techniciansNames)
								{
									ListBox_Projects_Technicians.SelectedItems.Add(name);
								}								
							}
						}
						else if (Grid_Projects_Modify_Button.Content.ToString() == "Save")  // Save MODIFIED Project
                        {
							Grid_Projects_Modify_Button.Content = "Modify";
							Grid_Projects_Delete_Button.IsEnabled = true;
							Grid_Projects_CreateNewProject_Button.IsEnabled = true;

							Grid_Projects_Cancel_Button.Visibility = Visibility.Collapsed;
							Grid_Projects_Cancel_Button.IsEnabled = false;

                            if (!IsInputFieldsAreNotEmpty_Project(Grid_Projects))
                            {
                                MessageBox.Show("All fields must be filled!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                            else
                            {
                                using (NIIDbContext db = new NIIDbContext())
                                {
                                    Project project = new Project();
                                    int id = (Projects_DataGrid.SelectedItem as Project).Id;
                                    project = db.Projects.Find(id);
                                    db.Entry(project).State = EntityState.Modified;

                                    project.Name = TextBox_Projects_Name.Text;
                                    project.CodeName = TextBox_Projects_CodeName.Text;
                                    project.Location = TextBox_Projects_Location.Text;
                                    project.Term = Convert.ToInt32(TextBox_Projects_Term.Text);
                                    project.Cost = Convert.ToDecimal(TextBox_Projects_Cost.Text);
                                    project.DateOfBeginning = DatePicker_Projects_DateOfBeginning.SelectedDate.Value;
									project.Description = TextBox_Projects_Description.Text;

									List<string> scientistsFromListBox = new List<string>();
									foreach(string scientist in ListBox_Projects_Scientists.SelectedItems)
									{
										scientistsFromListBox.Add(scientist);
									}
									List<Scientist> scientstsFromDb = new List<Scientist>();
									foreach(string scientist in scientistsFromListBox)
									{
										scientstsFromDb.Add(db.Scientists.Where(scs => scs.Name == scientist).FirstOrDefault());
									}
									project.Scientists = scientstsFromDb;

									
									List <string> techniciansFromListBox = new List<string>();
									foreach(string technician in ListBox_Projects_Technicians.SelectedItems)
									{
										techniciansFromListBox.Add(technician);
									}
									List<Technician> techniciansFromDb = new List<Technician>();
									foreach(string technician in techniciansFromListBox)
									{
										techniciansFromDb.Add(db.Technicians.Where(thc => thc.Name == technician).FirstOrDefault());
									}
									project.Technicians = techniciansFromDb;

									
									List<string> samplesFromListBox = new List<string>();
									foreach(string sample in ListBox_Projects_Samples.SelectedItems)
									{
										samplesFromListBox.Add(sample);
									}
									List<Sample> samplesFromDb = new List<Sample>();
									foreach(string sample in samplesFromListBox)
									{
										samplesFromDb.Add(db.Samples.Where(smp => smp.Title == sample).FirstOrDefault());
									}
									project.Samples = samplesFromDb;


									List<string> equipmentFromListBox = new List<string>();
									foreach(string pieceOfEquipment in ListBox_Projects_Equipment.SelectedItems)
									{
										equipmentFromListBox.Add(pieceOfEquipment);
									}
									List<Equipment> equipmentFromDb = new List<Equipment>();
									foreach(string pieceOfEquipment in equipmentFromListBox)
									{
										equipmentFromDb.Add(db.Equipment.Where(eqp => eqp.Title == pieceOfEquipment).FirstOrDefault());
									}
									project.Equipment = equipmentFromDb;																	
                                    
                                    db.SaveChanges();

                                    LoadDB();
                                }
                            }
							UnceslectAllListBoxes_and_CloseAllExpanders();
						}
					}                                               // Save NEW Project
					else if (Projects_DataGrid.SelectedItem == null & Grid_Projects_CreateNewProject_Button.IsEnabled == false & TglBtnProjects.IsChecked == true)
					{

						TglBtnProjects.IsChecked = false;
						TglBtnProjects.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
						Projects_DataGrid.IsEnabled = true;

						Grid_Projects_Modify_Button.Content = "Modify";
						Grid_Projects_Delete_Button.IsEnabled = true;
						Grid_Projects_CreateNewProject_Button.IsEnabled = true;

						Grid_Projects_Cancel_Button.Visibility = Visibility.Collapsed;
						Grid_Projects_Cancel_Button.IsEnabled = false;

                        if (!IsInputFieldsAreNotEmpty_Project(Grid_Projects))
                        {
                            MessageBox.Show("All fields must be filled!", "Message", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            using (NIIDbContext db = new NIIDbContext())
                            {
								List<string> scientistsFromListBox = new List<string>();
								foreach (string scientist in ListBox_Projects_Scientists.SelectedItems)
								{
									scientistsFromListBox.Add(scientist);
								}
								List<Scientist> scientstsFromDb = new List<Scientist>();
								foreach (string scientist in scientistsFromListBox)
								{
									scientstsFromDb.Add(db.Scientists.Where(scs => scs.Name == scientist).FirstOrDefault());
								}
								
								List<string> techniciansFromListBox = new List<string>();
								foreach (string technician in ListBox_Projects_Technicians.SelectedItems)
								{
									techniciansFromListBox.Add(technician);
								}
								List<Technician> techniciansFromDb = new List<Technician>();
								foreach (string technician in techniciansFromListBox)
								{
									techniciansFromDb.Add(db.Technicians.Where(thc => thc.Name == technician).FirstOrDefault());
								}

								List<string> samplesFromListBox = new List<string>();
								foreach (string sample in ListBox_Projects_Samples.SelectedItems)
								{
									samplesFromListBox.Add(sample);
								}
								List<Sample> samplesFromDb = new List<Sample>();
								foreach (string sample in samplesFromListBox)
								{
									samplesFromDb.Add(db.Samples.Where(smp => smp.Title == sample).FirstOrDefault());
								}
								
								List<string> equipmentFromListBox = new List<string>();
								foreach (string pieceOfEquipment in ListBox_Projects_Equipment.SelectedItems)
								{
									equipmentFromListBox.Add(pieceOfEquipment);
								}
								List<Equipment> equipmentFromDb = new List<Equipment>();
								foreach (string pieceOfEquipment in equipmentFromListBox)
								{
									equipmentFromDb.Add(db.Equipment.Where(eqp => eqp.Title == pieceOfEquipment).FirstOrDefault());
								}								

								Project project = new Project
                                {
                                    Name = TextBox_Projects_Name.Text,
                                    CodeName = TextBox_Projects_CodeName.Text,
                                    Location = TextBox_Projects_Location.Text,
                                    Term = Convert.ToInt32(TextBox_Projects_Term.Text),
                                    Cost = Convert.ToDecimal(TextBox_Projects_Cost.Text),
                                    DateOfBeginning = DatePicker_Projects_DateOfBeginning.SelectedDate.Value,
                                    Scientists = scientstsFromDb,
									Technicians = techniciansFromDb,
									Samples = samplesFromDb,
									Equipment = equipmentFromDb,
									Description = TextBox_Projects_Description.Text
                                };
                                db.Projects.Add(project);
                                db.SaveChanges();

                                LoadDB();
                            }
                        }
						UnceslectAllListBoxes_and_CloseAllExpanders();
					}
					else MessageBox.Show("Please select target record!", "Modify a project", MessageBoxButton.OK, MessageBoxImage.Warning);
					break;

				default:
					break;
			}
		}

		// Cancel button event
		private void Cancel_Button_Click(object sender, RoutedEventArgs e)
		{
			switch ((sender as Button).Name)
			{
				case "Grid_Samples_Cancel_Button":
					TglBtnSample.IsChecked = false;
					TglBtnSample.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

					Samples_DataGrid.IsEnabled = true;

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

					Grid_Equipment_Modify_Button.Content = "Modify";
					Grid_Equipment_Delete_Button.IsEnabled =true;
					Grid_Equipment_CreateNewPieceOfEquipment_Button.IsEnabled = true;

					Grid_Equipment_Cancel_Button.Visibility = Visibility.Collapsed;
					Grid_Equipment_Cancel_Button.IsEnabled = false;
					break;

				case "Grid_Technicians_Cancel_Button":
					TglBtnTechnicians.IsChecked = false;
					TglBtnTechnicians.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

					Technicians_DataGrid.IsEnabled = true;

					Grid_Technicians_Modify_Button.Content = "Modify";
					Grid_Technicians_Delete_Button.IsEnabled = true;
					Grid_Technicians_CreateNewTechnician_Button.IsEnabled = true;

					Grid_Technicians_Cancel_Button.Visibility = Visibility.Collapsed;
					Grid_Technicians_Cancel_Button.IsEnabled = false;
					break;

				case "Grid_Scientists_Cancel_Button":
					TglBtnScientists.IsChecked = false;
					TglBtnScientists.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

					Scientists_DataGrid.IsEnabled = true;

					Grid_Scientists_Modify_Button.Content = "Modify";
					Grid_Scientists_Delete_Button.IsEnabled = true;
					Grid_Scientists_CreateNewScientist_Button.IsEnabled = true;

					Grid_Scientists_Cancel_Button.Visibility = Visibility.Collapsed;
					Grid_Scientists_Cancel_Button.IsEnabled = false;
					break;

				case "Grid_Projects_Cancel_Button":
					TglBtnProjects.IsChecked = false;
					TglBtnProjects.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

					Projects_DataGrid.IsEnabled = true;

					Grid_Projects_Modify_Button.Content = "Modify";
					Grid_Projects_Delete_Button.IsEnabled = true;
					Grid_Projects_CreateNewProject_Button.IsEnabled = true;

					Grid_Projects_Cancel_Button.Visibility = Visibility.Collapsed;
					Grid_Projects_Cancel_Button.IsEnabled = false;

					UnceslectAllListBoxes_and_CloseAllExpanders();
					break;

				default:
					break;
			}
		}

		// Create new record button event
		private void CreateNew_Button_Click(object sender, RoutedEventArgs e)
		{
            // Clear all input fields
            ClearInputFields();

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
					break;

				case "Grid_Technicians_CreateNewTechnician_Button":
					TglBtnTechnicians.IsChecked = true;
					TglBtnTechnicians.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

					Grid_Technicians_Modify_Button.Content = "Save";

					Grid_Technicians_CreateNewTechnician_Button.IsEnabled = false;
					Grid_Technicians_Delete_Button.IsEnabled = false;
					Technicians_DataGrid.IsEnabled = false;

					Grid_Technicians_Cancel_Button.Visibility = Visibility.Visible;
					Grid_Technicians_Cancel_Button.IsEnabled = true;
					break;

				case "Grid_Scientists_CreateNewScientist_Button":
					TglBtnScientists.IsChecked = true;
					TglBtnScientists.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

					Grid_Scientists_Modify_Button.Content = "Save";

					Grid_Scientists_CreateNewScientist_Button.IsEnabled = false;
					Grid_Scientists_Delete_Button.IsEnabled = false;
					Scientists_DataGrid.IsEnabled = false;

					Grid_Scientists_Cancel_Button.Visibility = Visibility.Visible;
					Grid_Scientists_Cancel_Button.IsEnabled = true;
					break;

				case "Grid_Projects_CreateNewProject_Button":
					TglBtnProjects.IsChecked = true;
					TglBtnProjects.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));

					Grid_Projects_Modify_Button.Content = "Save";

					Grid_Projects_CreateNewProject_Button.IsEnabled = false;
					Grid_Projects_Delete_Button.IsEnabled = false;
					Projects_DataGrid.IsEnabled = false;

					Grid_Projects_Cancel_Button.Visibility = Visibility.Visible;
					Grid_Projects_Cancel_Button.IsEnabled = true;
					break;

				default:
					break;
			}
		}

		// Delete button event
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
                                Sample sample;
                                int id = (Samples_DataGrid.SelectedItem as Sample).Id;
                                sample = db.Samples.Find(id);
                                db.Samples.Remove(sample);

                                db.SaveChanges();
                                LoadDB();
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
                                Equipment equipment;
                                int id = (Equipment_DataGrid.SelectedItem as Equipment).Id;
                                equipment = db.Equipment.Find(id);
                                db.Equipment.Remove(equipment);

                                db.SaveChanges();
                                LoadDB();
                            }
						}
					}
					else MessageBox.Show("Please select a record you want to delete!", "Deleting the piece of equipment", MessageBoxButton.OK, MessageBoxImage.Warning);
					break;

				case "Grid_Technicians_Delete_Button":
					if (Technicians_DataGrid.SelectedItem != null)
					{
						var messageResult = MessageBox.Show("Are you sure?", "Deleting a technician", MessageBoxButton.YesNo, MessageBoxImage.Question);

						if (messageResult == MessageBoxResult.Yes)
						{
							using (NIIDbContext db = new NIIDbContext())
							{
                                Technician technician;
                                int id = (Technicians_DataGrid.SelectedItem as Technician).Id;
                                technician = db.Technicians.Find(id);
                                db.Technicians.Remove(technician);

                                db.SaveChanges();
                                LoadDB();
                            }
						}
					}
					else MessageBox.Show("Please select a record you want to delete!", "Deleting a technician", MessageBoxButton.OK, MessageBoxImage.Warning);
					break;

				case "Grid_Scientists_Delete_Button":
					if (Scientists_DataGrid.SelectedItem != null)
					{
						var messageResult = MessageBox.Show("Are you sure?", "Deleting a scientist", MessageBoxButton.YesNo, MessageBoxImage.Question);

						if (messageResult == MessageBoxResult.Yes)
						{
							using (NIIDbContext db = new NIIDbContext())
							{
                                Scientist scientist;
                                int id = (Scientists_DataGrid.SelectedItem as Scientist).Id;
                                scientist = db.Scientists.Find(id);
                                db.Scientists.Remove(scientist);

                                db.SaveChanges();
                                LoadDB();
                            }
						}
					}
					else MessageBox.Show("Please select a record you want to delete!", "Deleting a scientist", MessageBoxButton.OK, MessageBoxImage.Warning);
					break;

				case "Grid_Projects_Delete_Button":
					if (Projects_DataGrid.SelectedItem != null)
					{
						var messageResult = MessageBox.Show("Are you sure?", "Deleting a project", MessageBoxButton.YesNo, MessageBoxImage.Question);

						if (messageResult == MessageBoxResult.Yes)
						{
							using (NIIDbContext db = new NIIDbContext())
							{
                                Project project;
                                int id = (Projects_DataGrid.SelectedItem as Project).Id;
                                project = db.Projects.Find(id);
                                db.Projects.Remove(project);

                                db.SaveChanges();
                                LoadDB();
                            }
						}
					}
					else MessageBox.Show("Please select a record you want to delete!", "Deleting a project", MessageBoxButton.OK, MessageBoxImage.Warning);
					break;

				default:
					break;
			}
		}
        #endregion

        
    }
}
// Chart : https://code.msdn.microsoft.com/Chart-Control-in-WPF-c9727c28  