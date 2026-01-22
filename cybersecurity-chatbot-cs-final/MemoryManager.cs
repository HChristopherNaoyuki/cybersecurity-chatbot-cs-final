using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace cybersecurity_chatbot_cs_final
{
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

        public string UserName
        {
            get => _userName;
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

        public MemoryManager()
        {
            Directory.CreateDirectory(UserDataDirectory);
            LoadUserData();
            LogActivity("System initialized");
        }

        public void RememberKeyword(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return;

            string normalized = NormalizeKeyword(keyword);

            _keywordCounts[normalized] = _keywordCounts.TryGetValue(normalized, out int count)
                ? count + 1
                : 1;

            LogActivity($"Discussed topic: {normalized}");
            SaveUserData();
        }

        public int GetKeywordCount(string keyword)
        {
            return string.IsNullOrWhiteSpace(keyword)
                ? 0
                : _keywordCounts.TryGetValue(NormalizeKeyword(keyword), out int count)
                    ? count
                    : 0;
        }

        public void AddTask(string task)
        {
            if (!string.IsNullOrWhiteSpace(task))
            {
                _tasks.Add(task);
                LogActivity($"Added task: {task}");
                SaveUserData();
            }
        }

        public void RemoveTask(string task)
        {
            if (_tasks.Remove(task))
            {
                LogActivity($"Removed task: {task}");
                SaveUserData();
            }
        }

        public List<string> GetTasks()
        {
            return new List<string>(_tasks);
        }

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

        public List<string> GetActivityHistory()
        {
            return new List<string>(_history);
        }

        private string NormalizeKeyword(string keyword)
        {
            return keyword?.ToLower().Trim() ?? string.Empty;
        }

        private string GetUserFolderPath()
        {
            return Path.Combine(UserDataDirectory, _userName);
        }

        private string GetHistoryFilePath()
        {
            return Path.Combine(GetUserFolderPath(), HistoryFileName);
        }

        private string GetTasksFilePath()
        {
            return Path.Combine(GetUserFolderPath(), TasksFileName);
        }

        private string GetKeywordsFilePath()
        {
            return Path.Combine(GetUserFolderPath(), KeywordsFileName);
        }

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
    }
}