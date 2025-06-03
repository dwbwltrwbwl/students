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
        public DataOutput()
        {
            InitializeComponent();
            allAttendance = AppConnect.model01.attendance.ToList();
            filteredAttendance = allAttendance;
            listAttendance.ItemsSource = filteredAttendance;
        }
        private void TextSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
        private void ComboSort_SelectionChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void ComboFilter_SelectionChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
