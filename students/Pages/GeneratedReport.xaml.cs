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
    /// Логика взаимодействия для GeneratedReport.xaml
    /// </summary>
    public partial class GeneratedReport : Page
    {
        private readonly dynamic _student;

        public GeneratedReport(dynamic student)
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
