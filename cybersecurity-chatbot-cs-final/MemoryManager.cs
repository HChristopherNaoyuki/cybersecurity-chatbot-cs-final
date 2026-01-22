using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace cybersecurity_chatbot_cs_final
{
    /// <summary>
    /// Handles user-specific data storage including tasks, history, and keywords
    /// </summary>
    public class MemoryManager
    {
        private const string UserDataDirectory = "UserData";
        private const string HistoryFileName = "history.log";
        private const string TasksFileName = "tasks.dat";
        private const string KeywordsFileName = "keywords.dat";

        private readonly List<string> _history = new List<string>();
        private readonly List<string> _tasks = new List<string>();
        private readonly Dictionary<string, int> _keywordCounts = new Dictionary<string, int>();
        private string _userName = "User";

        /// <summary>
        /// Gets or sets the user name
        /// </summary>
        public string UserName
        {
            get { return _userName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Username cannot be empty");
                }

                if (_userName != value)
                {
                    SaveUserData();
                    _userName = value.Trim();
                    LoadUserData();
                    LogActivity($"User changed to: {_userName}");
                }
            }
        }

        /// <summary>
        /// Initializes memory manager and loads existing data
        /// </summary>
        public MemoryManager()
        {
            Directory.CreateDirectory(UserDataDirectory);
            LoadUserData();
            LogActivity("System initialized");
        }

        /// <summary>
        /// Records a keyword and increments its count
        /// </summary>
        /// <param name="keyword">Keyword to remember</param>
        public void RememberKeyword(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return;

            string normalized = NormalizeKeyword(keyword);

            if (_keywordCounts.ContainsKey(normalized))
            {
                _keywordCounts[normalized]++;
            }
            else
            {
                _keywordCounts[normalized] = 1;
            }

            LogActivity($"Discussed topic: {normalized}");
            SaveUserData();
        }

        /// <summary>
        /// Gets discussion count for a keyword
        /// </summary>
        /// <param name="keyword">Keyword to check</param>
        /// <returns>Discussion count</returns>
        public int GetKeywordCount(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return 0;
            }

            string normalized = NormalizeKeyword(keyword);
            if (_keywordCounts.TryGetValue(normalized, out int count))
            {
                return count;
            }
            return 0;
        }

        /// <summary>
        /// Adds a new task to memory
        /// </summary>
        /// <param name="task">Task description</param>
        public void AddTask(string task)
        {
            if (!string.IsNullOrWhiteSpace(task))
            {
                _tasks.Add(task);
                LogActivity($"Added task: {task}");
                SaveUserData();
            }
        }

        /// <summary>
        /// Removes a task from memory
        /// </summary>
        /// <param name="task">Task description</param>
        public void RemoveTask(string task)
        {
            if (_tasks.Remove(task))
            {
                LogActivity($"Removed task: {task}");
                SaveUserData();
            }
        }

        /// <summary>
        /// Gets all stored tasks
        /// </summary>
        /// <returns>List of tasks</returns>
        public List<string> GetTasks()
        {
            return new List<string>(_tasks);
        }

        /// <summary>
        /// Logs an activity with timestamp
        /// </summary>
        /// <param name="activity">Activity description</param>
        public void LogActivity(string activity)
        {
            if (string.IsNullOrWhiteSpace(activity)) return;

            string entry = $"{DateTime.Now:HH:mm:ss} - {activity}";
            _history.Add(entry);

            if (_history.Count > 100)
            {
                _history.RemoveAt(0);
            }

            SaveUserData();
        }

        /// <summary>
        /// Gets the activity history
        /// </summary>
        /// <returns>List of activity entries</returns>
        public List<string> GetActivityHistory()
        {
            return new List<string>(_history);
        }

        #region Private Methods

        /// <summary>
        /// Normalizes a keyword for consistent storage
        /// </summary>
        private string NormalizeKeyword(string keyword)
        {
            return keyword?.ToLower().Trim() ?? string.Empty;
        }

        /// <summary>
        /// Gets user-specific folder path
        /// </summary>
        private string GetUserFolderPath()
        {
            return Path.Combine(UserDataDirectory, _userName);
        }

        /// <summary>
        /// Gets history file path
        /// </summary>
        private string GetHistoryFilePath()
        {
            return Path.Combine(GetUserFolderPath(), HistoryFileName);
        }

        /// <summary>
        /// Gets tasks file path
        /// </summary>
        private string GetTasksFilePath()
        {
            return Path.Combine(GetUserFolderPath(), TasksFileName);
        }

        /// <summary>
        /// Gets keywords file path
        /// </summary>
        private string GetKeywordsFilePath()
        {
            return Path.Combine(GetUserFolderPath(), KeywordsFileName);
        }

        /// <summary>
        /// Loads user data from files
        /// </summary>
        private void LoadUserData()
        {
            try
            {
                string userFolder = GetUserFolderPath();
                Directory.CreateDirectory(userFolder);

                string historyPath = GetHistoryFilePath();
                _history.Clear();
                if (File.Exists(historyPath))
                {
                    _history.AddRange(File.ReadAllLines(historyPath));
                }

                string tasksPath = GetTasksFilePath();
                _tasks.Clear();
                if (File.Exists(tasksPath))
                {
                    _tasks.AddRange(File.ReadAllLines(tasksPath));
                }

                string keywordsPath = GetKeywordsFilePath();
                _keywordCounts.Clear();
                if (File.Exists(keywordsPath))
                {
                    foreach (string line in File.ReadLines(keywordsPath))
                    {
                        var parts = line.Split(':');
                        if (parts.Length == 2 && int.TryParse(parts[1], out int count))
                        {
                            _keywordCounts[parts[0]] = count;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogActivity($"Error loading user data: {ex.Message}");
            }
        }

        /// <summary>
        /// Saves user data to files
        /// </summary>
        private void SaveUserData()
        {
            try
            {
                string userFolder = GetUserFolderPath();
                Directory.CreateDirectory(userFolder);

                File.WriteAllLines(GetHistoryFilePath(), _history);
                File.WriteAllLines(GetTasksFilePath(), _tasks);
                File.WriteAllLines(
                    GetKeywordsFilePath(),
                    _keywordCounts.Select(kvp => $"{kvp.Key}:{kvp.Value}"));
            }
            catch (Exception ex)
            {
                LogActivity($"Error saving user data: {ex.Message}");
            }
        }

        #endregion
    }
}