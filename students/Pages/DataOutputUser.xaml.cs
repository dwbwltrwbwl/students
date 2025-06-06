using students.ApplicationData;
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

namespace students.Pages
{
    /// <summary>
    /// Логика взаимодействия для DataOutputUser.xaml
    /// </summary>
    public partial class DataOutputUser : Page
    {
        private List<attendance> allAttendance;
        private List<attendance> filteredAttendance;
        private List<groups> allGroups;
        private attendance selectedAttendance;
        public DataOutputUser()
        {
            InitializeComponent();
            allAttendance = AppConnect.model01.attendance.ToList();
            filteredAttendance = allAttendance;
            listAttendance.ItemsSource = filteredAttendance;
            LoadGroups();
            LoadAttendance();
        }
        private void LoadAttendance()
        {
            allAttendance = AppConnect.model01.attendance.ToList();
            filteredAttendance = allAttendance;
            listAttendance.ItemsSource = filteredAttendance;
        }
        private void LoadGroups()
        {
            try
            {
                allGroups = AppConnect.model01.groups
                    .OrderBy(g => g.group_number)
                    .ToList();
                ComboGroup.Items.Add(new ComboBoxItem
                {
                    Content = "Все группы",
                    Tag = null
                });
                foreach (var group in allGroups)
                {
                    ComboGroup.Items.Add(new ComboBoxItem
                    {
                        Content = group.group_number,
                        Tag = group
                    });
                }
                ComboGroup.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки групп: {ex.Message}");
            }
        }
        private void TextSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAttendanceList();
        }
        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAttendanceList();
        }

        private void ComboFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAttendanceList();
        }
        private void UpdateAttendanceList()
        {
            if (listAttendance == null || ComboSort == null || TextSearch == null || ComboGroup == null)
                return;
            string searchText = TextSearch.Text?.ToLower() ?? "";
            groups selectedGroup = null;
            if (ComboGroup.SelectedItem is ComboBoxItem selectedItem)
            {
                selectedGroup = selectedItem.Tag as groups;
            }

            filteredAttendance = allAttendance?
                .Where(a =>
                    (selectedGroup == null || a.students?.groups?.id_group == selectedGroup.id_group) &&
                    (a.students?.last_name?.ToLower()?.Contains(searchText) == true ||
                     a.students?.first_name?.ToLower()?.Contains(searchText) == true ||
                     a.students?.middle_name?.ToLower()?.Contains(searchText) == true ||
                     a.subjects?.subject_name?.ToLower()?.Contains(searchText) == true ||
                     a.date.ToString("dd.MM.yyyy")?.Contains(searchText) == true))
                .ToList() ?? new List<attendance>();
            switch (ComboSort.SelectedIndex)
            {
                case 1:
                    filteredAttendance = filteredAttendance
                        .OrderByDescending(a => a.date)
                        .ToList();
                    break;
                case 2:
                    filteredAttendance = filteredAttendance
                        .OrderBy(a => a.date)
                        .ToList();
                    break;
                case 3:
                    filteredAttendance = filteredAttendance
                        .OrderBy(a => a.students?.last_name)
                        .ThenBy(a => a.students?.first_name)
                        .ThenBy(a => a.students?.middle_name)
                        .ToList();
                    break;
                case 4:
                    filteredAttendance = filteredAttendance
                        .OrderByDescending(a => a.students?.last_name)
                        .ThenByDescending(a => a.students?.first_name)
                        .ThenByDescending(a => a.students?.middle_name)
                        .ToList();
                    break;
                case 5:
                    filteredAttendance = filteredAttendance
                        .OrderBy(a => a.subjects?.subject_name)
                        .ToList();
                    break;
            }
            listAttendance.ItemsSource = filteredAttendance;
        }
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var addEditPage = new AddEditPage();
            addEditPage.AttendanceUpdated += LoadAttendance;
            NavigationService.Navigate(addEditPage);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (listAttendance.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите запись для редактирования",
                                "Запись не выбрана",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            selectedAttendance = listAttendance.SelectedItem as attendance;
            var addEditPage = new AddEditPage(selectedAttendance);
            addEditPage.AttendanceUpdated += LoadAttendance;
            NavigationService.Navigate(addEditPage);
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Autorization());
        }
    }
}
