using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace cybersecurity_chatbot_cs_final
{
    /// <summary>
    /// Task management window for cybersecurity tasks
    /// </summary>
    public partial class TaskWindow : Window
    {
        /// <summary>
        /// Represents a cybersecurity task
        /// </summary>
        public class CybersecurityTask
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string Status { get; set; }
            public bool IsCompleted { get; set; }

            /// <summary>
            /// Creates a new cybersecurity task with default values
            /// </summary>
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

        /// <summary>
        /// Initializes task window
        /// </summary>
        public TaskWindow(ChatBot chatBot)
        {
            InitializeComponent();
            _chatBot = chatBot ?? throw new ArgumentNullException(nameof(chatBot));
            LoadTasks();
            TasksList.ItemsSource = _tasks;
        }

        /// <summary>
        /// Loads tasks from memory manager
        /// </summary>
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

        /// <summary>
        /// Handles Add Task button click
        /// </summary>
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

        /// <summary>
        /// Handles Complete Task button click
        /// </summary>
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

        /// <summary>
        /// Handles Delete Task button click
        /// </summary>
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

        /// <summary>
        /// Handles Close button click
        /// </summary>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Refreshes task list display
        /// </summary>
        private void RefreshTaskList()
        {
            var incompleteTasks = _tasks.Where(t => !t.IsCompleted).ToList();
            TasksList.ItemsSource = incompleteTasks;
            TasksList.Items.Refresh();
        }
    }
}