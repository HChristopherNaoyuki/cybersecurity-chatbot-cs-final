using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace cybersecurity_chatbot_cs_final
{
    public partial class TaskWindow : Window
    {
        public class CybersecurityTask
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Status { get; set; }
            public bool IsCompleted { get; set; }

            public CybersecurityTask()
            {
                Title = "New Task";
                Description = string.Empty;
                Status = "Pending";
                IsCompleted = false;
            }
        }

        private readonly List<CybersecurityTask> _tasks = new List<CybersecurityTask>();
        private readonly ChatBot _chatBot;

        public TaskWindow(ChatBot chatBot)
        {
            InitializeComponent();
            _chatBot = chatBot ?? throw new ArgumentNullException(nameof(chatBot));
            LoadTasks();
            TasksList.ItemsSource = _tasks;
        }

        private void LoadTasks()
        {
            try
            {
                var taskStrings = _chatBot.MemoryManager.GetTasks();
                foreach (var taskString in taskStrings)
                {
                    if (!string.IsNullOrWhiteSpace(taskString))
                    {
                        _tasks.Add(new CybersecurityTask
                        {
                            Title = taskString,
                            Description = "Cybersecurity task",
                            Status = "Pending",
                            IsCompleted = false
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TaskTitle.Text))
            {
                MessageBox.Show("Please enter a task title.", "Info",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var newTask = new CybersecurityTask
            {
                Title = TaskTitle.Text,
                Description = "Cybersecurity task",
                Status = "Pending",
                IsCompleted = false
            };

            _tasks.Add(newTask);
            _chatBot.MemoryManager.AddTask(newTask.Title);
            _chatBot.MemoryManager.LogActivity($"Added task: {newTask.Title}");
            RefreshTaskList();
            TaskTitle.Clear();
            TaskTitle.Focus();
        }

        private void CompleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (TasksList.SelectedItem is CybersecurityTask task)
            {
                task.Status = "Completed";
                task.IsCompleted = true;
                _chatBot.MemoryManager.LogActivity($"Completed task: {task.Title}");
                _chatBot.MemoryManager.RemoveTask(task.Title);
                RefreshTaskList();
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (TasksList.SelectedItem is CybersecurityTask task)
            {
                _tasks.Remove(task);
                _chatBot.MemoryManager.LogActivity($"Deleted task: {task.Title}");
                _chatBot.MemoryManager.RemoveTask(task.Title);
                RefreshTaskList();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RefreshTaskList()
        {
            var incompleteTasks = _tasks.Where(t => !t.IsCompleted).ToList();
            TasksList.ItemsSource = incompleteTasks;
            TasksList.Items.Refresh();
        }
    }
}