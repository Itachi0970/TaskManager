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
using System.Diagnostics;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TaskManager
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Task> tasks = new ObservableCollection<Task>();
        public MainWindow()
        {
            DispatcherTimer dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(DispatcherTimerUpdate);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            InitializeComponent();
        }
        private void DispatcherTimerUpdate(object sender, EventArgs e)
        {
            lblDate.Content = DateTime.Now;
            CommandManager.InvalidateRequerySuggested();
        }
        private void NewTaskButtonClicked(object sender, RoutedEventArgs e)
        {
            NewTask(newTaskName.Text);
        }
        private void NewTask(String newTaskName)
        {
            Task task;
            if(newTaskName == "")
            {
                task = new Task();
            } 
            else
            {
                task = new Task(newTaskName);
            }
            tasks.Add(task);
            taskList.ItemsSource = tasks;
        }
        private void DeleteTask(object sender, RoutedEventArgs e)
        {
            if(taskList.SelectedItems != null)
            {
                tasks.Remove((Task)taskList.SelectedItem);
            }
        }
        private void RenameTask(object sender, RoutedEventArgs e)
        {
            Task toRename = tasks.Where(x=>x == taskList.SelectedItem).FirstOrDefault();
            if (toRename != null)
            {
                toRename.taskName = newTaskName.Text;
                toRename.OnPropertyChanged();
            }
            
        }
    }

    public class Task : INotifyPropertyChanged
    {
        public String taskName { get; set; }
        public bool complete { get; set; }
        public Task()
        {
            taskName = "New Task";
            complete = false;
        }
        public Task(String taskName)
        {
            this.taskName = taskName;
            complete = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}

