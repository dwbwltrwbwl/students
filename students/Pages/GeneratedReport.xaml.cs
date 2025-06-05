using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using WordDocument = Microsoft.Office.Interop.Word.Document;
using WordTable = Microsoft.Office.Interop.Word.Table;
using WordRange = Microsoft.Office.Interop.Word.Range;
using SystemPath = System.IO.Path;

namespace students.Pages
{
    public class RecommendationEngine
    {
        public string GenerateRecommendations(double totalAverage, double attendancePercent)
        {
            StringBuilder sb = new StringBuilder();
            if (totalAverage < 3.0)
            {
                sb.AppendLine("⚠️ Студент в группе риска! Рекомендуем:");
                sb.AppendLine("- Организовать дополнительные консультации");
                sb.AppendLine("- Проверить наличие задолженностей");
            }
            else if (totalAverage > 4.5)
            {
                sb.AppendLine("⭐ Одаренный студент! Предложите:");
                sb.AppendLine("- Участие в олимпиадах");
                sb.AppendLine("- Углубленные материалы по предмету");
            }
            if (attendancePercent < 70)
            {
                sb.AppendLine($"\n⏱ Низкая посещаемость ({attendancePercent:0}%):");
                sb.AppendLine("- Обсудить причины отсутствий");
                sb.AppendLine("- Ввести балльную систему за посещение");
            }
            return sb.Length > 0 ? sb.ToString() : "✅ Студент показывает стабильные результаты. Рекомендации не требуются";
        }
    }
    public partial class GeneratedReport : System.Windows.Controls.Page
    {
        private readonly ApplicationData.students _student;

        public GeneratedReport(dynamic student)
        {
            _student = student as ApplicationData.students;
            InitializeComponent();
            LoadStudentData();
        }

        private void LoadStudentData()
        {
            try
            {
                tbStudentName.Text = _student.FullName;
                tbGroupInfo.Text = $"Группа: {_student.groups?.group_number}";
                var gradesList = _student.attendance
                    .Where(a => a.grade.HasValue)
                    .ToList();
                if (gradesList.Any())
                {
                    var gradesBySubject = gradesList
                        .GroupBy(a => a.subjects?.subject_name)
                        .Select(g => new
                        {
                            Subject = g.Key ?? "Без названия",
                            Grades = string.Join(", ", g.Select(x => x.grade.Value)),
                            Average = g.Average(x => x.grade.Value).ToString("0.00")
                        })
                        .ToList();
                    lvGrades.ItemsSource = gradesBySubject;
                    double totalAverage = gradesList.Average(a => a.grade.Value);
                    tbTotalAverage.Text = totalAverage.ToString("0.00");
                }
                else
                {
                    lvGrades.ItemsSource = null;
                    tbTotalAverage.Text = "нет оценок";
                }
                var attendanceList = _student.attendance.ToList();
                if (attendanceList.Any())
                {
                    var attendanceDisplayList = attendanceList
                        .Select(a => new
                        {
                            Date = a.date,
                            Subject = a.subjects?.subject_name ?? "Без названия",
                            VisitStatus = a.id_visit,
                            IsPresent = a.id_visit == 1 || a.id_visit == 3
                        })
                        .ToList();
                    lvAttendance.ItemsSource = attendanceDisplayList;
                    int presentCount = attendanceList.Count(a => a.id_visit == 1 || a.id_visit == 3);
                    double percent = (double)presentCount / attendanceList.Count * 100;
                    tbAttendancePercent.Text = $"{percent:0}%";
                }
                else
                {
                    lvAttendance.ItemsSource = null;
                    tbAttendancePercent.Text = "100%";
                }
                double totalAverageValue = 0;
                if (gradesList.Any())
                {
                    totalAverageValue = gradesList.Average(a => a.grade.Value);
                }
                double attendancePercentValue = 100;
                if (attendanceList.Any())
                {
                    int presentCount = attendanceList.Count(a => a.id_visit == 1 || a.id_visit == 3);
                    attendancePercentValue = (double)presentCount / attendanceList.Count * 100;
                }
                var engine = new RecommendationEngine();
                string recommendations = engine.GenerateRecommendations(totalAverageValue, attendancePercentValue);
                tbRecommendations.Text = recommendations;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void WordDocument_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string templatePath = @"D:\StudentReportTemplate.docx";
                string outputDirectory = @"D:\Reports";
                string outputPath = SystemPath.Combine(outputDirectory,
                                                      $"Отчет_{_student.last_name}_{DateTime.Now:ddMMyyyyHHmmss}.docx");
                if (!File.Exists(templatePath))
                {
                    MessageBox.Show($"Шаблон документа не найден:\n{templatePath}",
                                    "Ошибка",
                                    MessageBoxButton.OK,
                                    MessageBoxImage.Error);
                    return;
                }
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                Word.Application wordApp = new Word.Application();
                WordDocument wordDoc = wordApp.Documents.Open(templatePath);
                ReplaceBookmark(wordDoc, "StudentName", _student.FullName);
                ReplaceBookmark(wordDoc, "GroupInfo", _student.groups?.group_number ?? "Не указана");
                ReplaceBookmark(wordDoc, "TotalAverage", tbTotalAverage.Text);
                ReplaceBookmark(wordDoc, "AttendancePercent", tbAttendancePercent.Text);
                if (lvGrades.ItemsSource is IEnumerable<dynamic> grades)
                {
                    AddGradesTable(wordDoc, "GradesTable", grades);
                }
                if (lvAttendance.ItemsSource is IEnumerable<dynamic> attendance)
                {
                    AddAttendanceTable(wordDoc, "AttendanceTable", attendance);
                }
                wordDoc.SaveAs2(outputPath);
                wordDoc.Close();
                wordApp.Quit();

                MessageBox.Show($"Документ успешно создан:\n{outputPath}",
                                "Успех",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
                NavigationService.Navigate(new PageQRCode());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании документа: {ex.Message}\n{ex.StackTrace}",
                                "Ошибка",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        private void ReplaceBookmark(WordDocument doc, string bookmarkName, string text)
        {
            if (doc.Bookmarks.Exists(bookmarkName))
            {
                Word.Bookmark bookmark = doc.Bookmarks[bookmarkName];
                bookmark.Range.Text = text;
            }
        }

        private void AddGradesTable(WordDocument doc, string bookmarkName, IEnumerable<dynamic> grades)
        {
            if (!doc.Bookmarks.Exists(bookmarkName)) return;

            Word.Bookmark bookmark = doc.Bookmarks[bookmarkName];
            WordRange range = bookmark.Range;
            int start = range.Start;
            int end = range.End;
            range = doc.Range(end, end);
            WordTable table = doc.Tables.Add(range, grades.Count() + 1, 3);
            table.Borders.Enable = 1;
            table.Range.ParagraphFormat.SpaceAfter = 0;
            table.Cell(1, 1).Range.Text = "Предмет";
            table.Cell(1, 2).Range.Text = "Оценки";
            table.Cell(1, 3).Range.Text = "Средний балл";
            int row = 2;
            foreach (var grade in grades)
            {
                table.Cell(row, 1).Range.Text = grade.Subject.ToString();
                table.Cell(row, 2).Range.Text = grade.Grades.ToString();
                table.Cell(row, 3).Range.Text = grade.Average.ToString();
                row++;
            }
            if (doc.Bookmarks.Exists(bookmarkName))
            {
                doc.Bookmarks[bookmarkName].Delete();
            }
        }

        private void AddAttendanceTable(WordDocument doc, string bookmarkName, IEnumerable<dynamic> attendance)
        {
            if (!doc.Bookmarks.Exists(bookmarkName)) return;

            Word.Bookmark bookmark = doc.Bookmarks[bookmarkName];
            WordRange range = bookmark.Range;
            int start = range.Start;
            int end = range.End;
            range = doc.Range(end, end);
            WordTable table = doc.Tables.Add(range, attendance.Count() + 1, 3);
            table.Borders.Enable = 1;
            table.Range.ParagraphFormat.SpaceAfter = 0;
            table.Cell(1, 1).Range.Text = "Дата";
            table.Cell(1, 2).Range.Text = "Предмет";
            table.Cell(1, 3).Range.Text = "Статус";

            int row = 2;
            foreach (var record in attendance)
            {
                table.Cell(row, 1).Range.Text = record.Date.ToString("dd.MM.yyyy");
                table.Cell(row, 2).Range.Text = record.Subject.ToString();
                table.Cell(row, 3).Range.Text = GetStatusText(record.VisitStatus);
                row++;
            }

            if (doc.Bookmarks.Exists(bookmarkName))
            {
                doc.Bookmarks[bookmarkName].Delete();
            }
        }

        private string GetStatusText(int visitStatus)
        {
            switch (visitStatus)
            {
                case 1: return "Присутствовал";
                case 2: return "Отсутствовал";
                case 3: return "Опоздание";
                default: return "Неизвестно";
            }
        }
    }
}
