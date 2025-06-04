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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private attendance currentAttendance;
        private bool isEditMode;
        public AddEditPage()
        {
            InitializeComponent();
            LoadGroups();
            LoadSubjects();
            LoadVisits();
            CBGroup.SelectedIndex = -1;
            LoadStudents(null);
            isEditMode = false;
            InitializePage();
        }
        public AddEditPage(attendance attendanceToEdit)
        {
            InitializeComponent();
            currentAttendance = attendanceToEdit;
            isEditMode = true;
            InitializePage();
        }
        private void InitializePage()
        {
            try
            {
                LoadGroups();
                LoadSubjects();
                LoadVisits();

                if (isEditMode)
                {
                    Title = "Редактирование записи";
                    CBGroup.SelectedValue = currentAttendance.students.groups.id_group;
                    CBStudent.SelectedValue = currentAttendance.id_student;
                    CBSubject.SelectedValue = currentAttendance.id_subject;
                    DPDate.SelectedDate = currentAttendance.date;
                    CBVisit.SelectedValue = currentAttendance.id_visit;
                    TBGrade.Text = currentAttendance.grade.ToString();
                }
                else
                {
                    Title = "Добавление новой записи";
                    CBGroup.SelectedIndex = -1;
                    CBStudent.SelectedIndex = -1;
                    CBSubject.SelectedIndex = -1;
                    DPDate.SelectedDate = DateTime.Today;
                    CBVisit.SelectedIndex = 0;
                    TBGrade.Text = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadGroups()
        {
            try
            {
                var groups = AppConnect.model01.groups
                    .OrderBy(g => g.group_number)
                    .ToList();
                CBGroup.ItemsSource = groups;
                CBGroup.DisplayMemberPath = "group_number";
                CBGroup.SelectedValuePath = "id_group";
                CBGroup.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки групп: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void LoadStudents(int? groupId = null)
        {
            try
            {
                var query = AppConnect.model01.students.AsQueryable();
                if (groupId.HasValue)
                {
                    query = query.Where(s => s.id_group == groupId.Value);
                }
                else
                {
                    CBStudent.ItemsSource = null;
                    return;
                }
                var students = query
                    .OrderBy(s => s.last_name)
                    .ThenBy(s => s.first_name)
                    .ToList();
                CBStudent.ItemsSource = students;
                CBStudent.DisplayMemberPath = "FullName";
                CBStudent.SelectedValuePath = "id_student";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки студентов: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void CBGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBGroup.SelectedItem != null)
            {
                int selectedGroupId = (int)CBGroup.SelectedValue;
                LoadStudents(selectedGroupId);
            }
            else
            {
                LoadStudents(null);
            }
        }
        private void LoadSubjects()
        {
            try
            {
                var subjects = AppConnect.model01.subjects
                    .OrderBy(s => s.subject_name)
                    .ToList();
                CBSubject.ItemsSource = subjects;
                CBSubject.DisplayMemberPath = "subject_name";
                CBSubject.SelectedValuePath = "id_subject";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки предметов: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadVisits()
        {
            try
            {
                var visits = AppConnect.model01.visits.ToList();
                CBVisit.ItemsSource = visits;
                CBVisit.DisplayMemberPath = "visit_name";
                CBVisit.SelectedValuePath = "id_visit";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки типов посещаемости: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CBStudent.SelectedItem == null ||
                    CBSubject.SelectedItem == null ||
                    DPDate.SelectedDate == null ||
                    CBVisit.SelectedItem == null ||
                    string.IsNullOrWhiteSpace(TBGrade.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (isEditMode)
                {
                    currentAttendance.id_student = (int)CBStudent.SelectedValue;
                    currentAttendance.id_subject = (int)CBSubject.SelectedValue;
                    currentAttendance.date = DPDate.SelectedDate.Value;
                    currentAttendance.id_visit = (int)CBVisit.SelectedValue;
                    currentAttendance.grade = int.Parse(TBGrade.Text);
                }
                else
                {
                    var newAttendance = new attendance
                    {
                        id_student = (int)CBStudent.SelectedValue,
                        id_subject = (int)CBSubject.SelectedValue,
                        date = DPDate.SelectedDate.Value,
                        id_visit = (int)CBVisit.SelectedValue,
                        grade = int.Parse(TBGrade.Text)
                    };
                    AppConnect.model01.attendance.Add(newAttendance);
                }
                AppConnect.model01.SaveChanges();
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
