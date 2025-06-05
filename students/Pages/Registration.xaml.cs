using students.ApplicationData;
using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace students.Pages
{
    public partial class Registration : Page
    {
        public bool IsPhoneNumberComplete { get; private set; }
        public Registration()
        {
            InitializeComponent();
            LoadQualifications();
        }
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            try
            {
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }
        private void TBEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            ValidateEmail();
        }
        private bool ValidateEmail()
        {
            string email = TBEmail.Text.Trim();
            bool isValid = IsValidEmail(email);
            Border border = (Border)TBEmail.Template.FindName("BorderElement", TBEmail);
            if (border != null)
            {
                border.BorderBrush = isValid
                    ? (SolidColorBrush)new BrushConverter().ConvertFrom("#FF5427D4")
                    : Brushes.Red;
            }
            EmailErrorText.Visibility = isValid
                ? Visibility.Collapsed
                : Visibility.Visible;

            return isValid;
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            foreach (char c in e.Text)
            {
                if (!char.IsLetter(c))
                {
                    e.Handled = true;
                    return;
                }
            }
        }
        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                foreach (char c in text)
                {
                    if (!char.IsLetter(c))
                    {
                        e.CancelCommand();
                        return;
                    }
                }
            }
            else
            {
                e.CancelCommand();
            }
        }
        private void TBTelephone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0))
            {
                e.Handled = true;
                return;
            }
            string currentText = TBTelephone.Text.Replace("+7 (", "")
                                                .Replace(")", "")
                                                .Replace("-", "")
                                                .Replace(" ", "");
            if (currentText.Length >= 11)
            {
                e.Handled = true;
            }
        }

        private void TBTelephone_TextChanged(object sender, TextChangedEventArgs e)
        {
            string digitsOnly = new string(TBTelephone.Text.Where(char.IsDigit).ToArray());

            if (digitsOnly.Length > 0 && digitsOnly[0] != '7' && digitsOnly[0] != '8')
            {
                digitsOnly = "7" + digitsOnly;
            }

            IsPhoneNumberComplete = digitsOnly.Length == 11;

            string formattedPhone = "+7 (";
            if (digitsOnly.Length > 1)
                formattedPhone += digitsOnly.Substring(1, Math.Min(3, digitsOnly.Length - 1));
            if (digitsOnly.Length > 4)
                formattedPhone += ") " + digitsOnly.Substring(4, Math.Min(3, digitsOnly.Length - 4));
            if (digitsOnly.Length > 7)
                formattedPhone += "-" + digitsOnly.Substring(7, Math.Min(2, digitsOnly.Length - 7));
            if (digitsOnly.Length > 9)
                formattedPhone += "-" + digitsOnly.Substring(9, Math.Min(2, digitsOnly.Length - 9));

            TBTelephone.TextChanged -= TBTelephone_TextChanged; // Отключаем обработчик
            TBTelephone.Text = formattedPhone; // Устанавливаем отформатированный текст
            TBTelephone.CaretIndex = formattedPhone.Length; // Устанавливаем курсор в конец
            TBTelephone.TextChanged += TBTelephone_TextChanged; // Включаем обработчик обратно

            UpdatePhoneValidationIndicator();
        }

        private void UpdatePhoneValidationIndicator()
        {
            if (IsPhoneNumberComplete)
            {
                TBTelephone.BorderBrush = Brushes.Gray;
                TBTelephone.ToolTip = null;
            }
            else
            {
                TBTelephone.BorderBrush = Brushes.Red;
                TBTelephone.ToolTip = "Введите полный номер телефона (11 цифр)";
            }
        }
        private void TBTelephone_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!IsPhoneNumberComplete && TBTelephone.Text.Length > 0)
            {
                MessageBox.Show("Введите полный номер телефона из 11 цифр",
                                "Неполный номер",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
            }
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
            if (!ValidateEmail())
            {
                MessageBox.Show("Пожалуйста, введите корректный email");
                TBEmail.Focus();
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