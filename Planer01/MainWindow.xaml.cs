using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Planer01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public class Subject
    {
        public string Name { get; set; }
        public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
        public List<Assessment> Assessments { get; set; } = new List<Assessment>();
        public override string ToString()
        {
            return Name;
        }
    }
    public class Assessment
    {
        public string Title { get; set; }      // np. "Kolokwium 1"
        public DateTime Date { get; set; }
        public string Type { get; set; }       // Kolokwium / Test / Egzamin / Projekt nie wiem czy to wykorzystam ale może się przydać
    }
    public class TaskItem
    {
        public string Title { get; set; }
        public DateTime? Deadline { get; set; }
        public bool IsDone { get; set; }
        public Subject Subject  { get; set; }
        public TaskItem(Subject subject)
        {
            Subject = subject;
        }


        public override string ToString()
        {
            if (Deadline.HasValue)
            {
                return $"{Title} - {Subject.Name} (Deadline: {Deadline.Value.ToShortDateString()})";
            }
            else
            {
                if(Subject != null)
                    return $"{Title} - {Subject.Name}";

                return Title;
            }
          
        }
       
    }
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
        }
        //FUNKCJE ZADAŃ
        //
        //
        //
        private List<TaskItem> taskItems = new List<TaskItem>();
        private List<Subject> subjects = new List<Subject>();
        private void AddTask(string title, string Subcject = "", DateTime? dateTime = null) // dodaje zadanie do listy, można podać tytuł, przedmiot i termin wykonania, ale tylko tytuł jest wymagany
        {

            if (!string.IsNullOrWhiteSpace(Title))
            {
                //Subject selectedSubject = (Subject)SubjectComboBox.SelectedItem;
                
                   

                if (SubjectComboBox.SelectedItem is Subject selectedSubject)
                {
                    TaskItem task = new TaskItem(selectedSubject)
                    {
                        Title = title,
                        //Subject = new Subject(), //? poprawialam moze jest błędnie
                        Deadline = dateTime,
                        IsDone = false
                    };

                    taskItems.Add(task);
                    selectedSubject.Tasks.Add(task);
                    TasksListBox.Items.Add(task);
                }
                else
                {
                    MessageBox.Show("wybierz przedmiot do zadania");
                    return;
                }
            }


        }

        private void AddTask_Click(object sender, RoutedEventArgs e) // dodaje zadanie do listy po kliknięciu przycisku, pobierając tytuł z TextBoxa
        {
            if (!string.IsNullOrWhiteSpace(TaskTextBox.Text))
            {
                AddTask(TaskTextBox.Text);
                TaskTextBox.Clear();
            }
        }
        private void RemoveTask_Click(object sender, RoutedEventArgs e) //usuwa ostatnie zadanie z listy
        {
            if (TasksListBox.Items.Count > 0)
            {
                TaskItem lastTask = taskItems.Last();
                TasksListBox.Items.Remove(lastTask);

            }
        }
        private void RemoveSelectedTask_Click(object sender, RoutedEventArgs e) //usuwa zaznaczone zadanie z listy
        {
            if (TasksListBox.SelectedItem != null)
            {
                TaskItem selectedTask = (TaskItem)TasksListBox.SelectedItem;
                taskItems.Remove(selectedTask);
                TasksListBox.Items.Remove(selectedTask);

            }
        }

        private void TasksListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e) //usuwa zaznaczone zadanie z listy po podwójnym kliknięciu
        {
            if (TasksListBox.SelectedItem != null)
            {
                TaskItem selectedTask = (TaskItem)TasksListBox.SelectedItem;
                taskItems.Remove(selectedTask);
                TasksListBox.Items.Remove(selectedTask);
               

            }
        }
        private void TaskTextBox_KeyDown(object sender, KeyEventArgs e) //dodaje zadanie do listy po naciśnięciu klawisza Enter, pobierając tytuł z TextBoxa
        {
            if (e.Key == Key.Enter)
            {
                if (!string.IsNullOrWhiteSpace(TaskTextBox.Text))
                {
                    AddTask(TaskTextBox.Text);
                    TaskTextBox.Clear();
                }
            }
        }
        private void ShowTasks_Click(object sender, RoutedEventArgs e)
        {

            SubjectsView.Visibility = Visibility.Collapsed;
            TasksView.Visibility = Visibility.Visible;
            AbsenceCounterView.Visibility = Visibility.Collapsed;
        }
        // KONIEC FUNKCJI ZADAŃ
        //...
        //...
        //...




        //FUNKCJE PRZEDMIOTÓW
        private void ShowSubjects_Click(object sender, RoutedEventArgs e)
        {

            SubjectsView.Visibility = Visibility.Visible;
            TasksView.Visibility = Visibility.Collapsed;
            AbsenceCounterView.Visibility = Visibility.Collapsed;
        }

        private void AddSubject(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                Subject subject = new Subject
                {
                    Name = name
                };

                subjects.Add(subject);
                SubjectListBox.Items.Add(subject);
                SubjectComboBox.Items.Add(subject);
                AbsenceSubjectListBox.Items.Add(subject);
            }
        }

        private void AddSubject_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(SubjectTextBox.Text))
            {
                AddSubject(SubjectTextBox.Text);
                SubjectTextBox.Clear();
            }
        }

        private void SubjectTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (!string.IsNullOrWhiteSpace(SubjectTextBox.Text))
                {
                    AddSubject(SubjectTextBox.Text);
                    SubjectTextBox.Clear();
                }
            }
        }
        private void SubjectsListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(SubjectListBox.SelectedItem != null)
            {
                Subject selectedSubject = (Subject)SubjectListBox.SelectedItem;
                foreach(var task in selectedSubject.Tasks.ToList())
                {
                    taskItems.Remove(task);
                    TasksListBox.Items.Remove(task);
                }
                subjects.Remove(selectedSubject);
                SubjectListBox.Items.Remove(selectedSubject);
                SubjectComboBox.Items.Remove(selectedSubject);
                AbsenceSubjectListBox.Items.Remove(selectedSubject);
            }
        }

        // KONIEC FUNKCJI PRZEDMIOTÓW





        // FUNKCJE LICZNIKA NIEOBECNOSCI

        private void ShowAbsenceCounter_Click(object sender, RoutedEventArgs e)
        {
            AbsenceCounterView.Visibility = Visibility.Visible;
            SubjectsView.Visibility = Visibility.Collapsed;
            TasksView.Visibility = Visibility.Collapsed;
        }






    }
}