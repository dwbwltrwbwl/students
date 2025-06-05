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
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Page
    {
        public Autorization()
        {
            InitializeComponent();
        }
        private void ButtonVhod_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string login = TBLogin.Text;
                string password = PBPassword.Password;
                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var userObj = ApplicationData.AppConnect.model01.teachers
                    .FirstOrDefault(x => x.login == login && x.password == password);

                if (userObj == null)
                {
                    MessageBox.Show("Такого пользователя нет", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    string fullName = $"{userObj.first_name} {userObj.last_name}";
                    MessageBox.Show($"Здравствуйте, {fullName}", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
                    AppConnect.id_instructor = userObj.id_teacher;
                    switch (userObj.id_role)
                    {
                        case 1:
                            NavigationService.Navigate(new DataOutput());
                            break;
                        case 2:
                            NavigationService.Navigate(new DataOutputUser());
                            break;
                        default:
                            MessageBox.Show("Недопустимая роль пользователя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Критическая ошибка приложения: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ButtonRegistr_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Registration());
        }
    }
}
