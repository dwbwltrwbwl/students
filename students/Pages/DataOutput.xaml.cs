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
    /// Логика взаимодействия для DataOutput.xaml
    /// </summary>
    public partial class DataOutput : Page
    {
        private List<attendance> allAttendance;
        private List<attendance> filteredAttendance;
        private List<groups> allGroups;
        private attendance selectedAttendance;
        public DataOutput()
        {
            InitializeComponent();
            allAttendance = AppConnect.model01.attendance.ToList();
            filteredAttendance = allAttendance;
            listAttendance.ItemsSource = filteredAttendance;
            LoadGroups();
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
            NavigationService.Navigate(new AddEditPage());
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
            NavigationService.Navigate(new AddEditPage(selectedAttendance));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (listAttendance.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите запись для удаления",
                                "Внимание",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }
            var confirmResult = MessageBox.Show("Вы уверены, что хотите удалить запись?",
                                                "Подтверждение удаления",
                                                MessageBoxButton.YesNo,
                                                MessageBoxImage.Question);
            if (confirmResult != MessageBoxResult.Yes)
                return;

            try
            {
                var selectedAttendance = (attendance)listAttendance.SelectedItem;
                AppConnect.model01.attendance.Remove(selectedAttendance);
                AppConnect.model01.SaveChanges();
                allAttendance.Remove(selectedAttendance);
                UpdateAttendanceList();
                MessageBox.Show("Запись успешно удалена!",
                                "Успех",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}\n\n" +
                                "Подробности: " + ex.InnerException?.Message,
                                "Ошибка базы данных",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Autorization());
        }

        private void GenerateReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (listAttendance.SelectedItem == null)
            {
                MessageBox.Show("Выберите студента из списка",
                                "Внимание",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            try
            {
                dynamic selectedRecord = listAttendance.SelectedItem;
                if (selectedRecord.students == null)
                {
                    MessageBox.Show("В выбранной записи отсутствует информация о студенте",
                                    "Ошибка",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    return;
                }
                NavigationService.Navigate(new GeneratedReport(selectedRecord.students));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при формировании отчета: {ex.Message}",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }
    }
}
