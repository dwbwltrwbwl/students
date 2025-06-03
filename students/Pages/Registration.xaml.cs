using students.ApplicationData;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace students.Pages
{
    public partial class Registration : Page
    {
        public Registration()
        {
            InitializeComponent();
            LoadQualifications();
        }

        private void LoadQualifications()
        {
            try
            {
                var qualifications = AppConnect.model01.qualifications.ToList();
                CBQualification.ItemsSource = qualifications;
                CBQualification.DisplayMemberPath = "qualification_name";
                CBQualification.SelectedValuePath = "id_qualification";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки квалификаций: {ex.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonRegist_Click(object sender, RoutedEventArgs e)
        {
            string first_name = TBName.Text;
            string last_name = TBSurname.Text;
            string middle_name = TBMiddleName.Text;
            string email = TBEmail.Text;
            string phone = TBTelephone.Text;
            string login = TBLogin.Text;
            string password = TBPassword.Text;
            string passwordRepeat = PBpassword.Password;
            if (string.IsNullOrWhiteSpace(first_name) ||
                string.IsNullOrWhiteSpace(last_name) ||
                string.IsNullOrWhiteSpace(middle_name) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phone) ||
                string.IsNullOrWhiteSpace(login) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(passwordRepeat))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Уведомление",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (CBQualification.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите квалификацию.", "Уведомление",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (password != passwordRepeat)
            {
                MessageBox.Show("Пароли не совпадают!", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (AppConnect.model01.teachers.Any(x => x.login == login))
            {
                MessageBox.Show("Пользователь с таким логином уже существует!", "Уведомление",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            try
            {
                var selectedQualification = (qualifications)CBQualification.SelectedItem;
                var teacherRole = AppConnect.model01.roles.FirstOrDefault(r => r.id_role == 1);
                if (teacherRole == null)
                {
                    MessageBox.Show("Роль преподавателя не найдена в системе!", "Ошибка",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                teachers userObj = new teachers
                {
                    first_name = first_name,
                    last_name = last_name,
                    middle_name = middle_name,
                    email = email,
                    phone = phone,
                    login = login,
                    password = password,
                    id_qualification = selectedQualification.id_qualification,
                    id_role = teacherRole.id_role
                };
                AppConnect.model01.teachers.Add(userObj);
                AppConnect.model01.SaveChanges();
                MessageBox.Show("Регистрация прошла успешно!", "Уведомление",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                TBName.Clear();
                TBSurname.Clear();
                TBMiddleName.Clear();
                TBEmail.Clear();
                TBTelephone.Clear();
                TBLogin.Clear();
                TBPassword.Clear();
                PBpassword.Clear();
                CBQualification.SelectedItem = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}\n\n" +
                                $"Inner exception: {ex.InnerException?.Message}", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Autorization());
        }
    }
}