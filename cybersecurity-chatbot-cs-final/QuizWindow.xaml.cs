using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace cybersecurity_chatbot_cs_final
{
    public partial class QuizWindow : Window
    {
        private readonly List<QuizQuestion> _questions = new List<QuizQuestion>();
        private readonly ChatBot _chatBot;
        private int _currentQuestionIndex = 0;
        private int _score = 0;
        private QuizAnswer _selectedAnswer = null;

        public QuizWindow(ChatBot chatBot)
        {
            InitializeComponent();
            _chatBot = chatBot ?? throw new ArgumentNullException(nameof(chatBot));

            try
            {
                _questions = LoadQuizQuestions();
                if (_questions.Count == 0)
                {
                    throw new InvalidOperationException("No quiz questions loaded");
                }
                DisplayQuestion();
                _chatBot.MemoryManager.LogActivity("Started cybersecurity quiz");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Quiz initialization failed: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private List<QuizQuestion> LoadQuizQuestions()
        {
            return new List<QuizQuestion>
            {
                new QuizQuestion(
                    "What should you do if you receive an email asking for your password?",
                    new List<QuizAnswer>
                    {
                        new QuizAnswer("Reply with your password", false),
                        new QuizAnswer("Delete the email", false),
                        new QuizAnswer("Report the email as phishing", true),
                        new QuizAnswer("Ignore it", false)
                    },
                    "Legitimate organizations never ask for passwords via email."),

                new QuizQuestion(
                    "Which password is strongest?",
                    new List<QuizAnswer>
                    {
                        new QuizAnswer("password123", false),
                        new QuizAnswer("P@ssw0rd!", false),
                        new QuizAnswer("CorrectHorseBatteryStaple", true),
                        new QuizAnswer("12345678", false)
                    },
                    "Long passphrases are more secure than complex short passwords."),

                new QuizQuestion(
                    "What does HTTPS indicate?",
                    new List<QuizAnswer>
                    {
                        new QuizAnswer("The site has videos", false),
                        new QuizAnswer("The connection is encrypted", true),
                        new QuizAnswer("The site is government-approved", false),
                        new QuizAnswer("The site loads faster", false)
                    },
                    "HTTPS ensures encrypted connections."),

                new QuizQuestion(
                    "What is two-factor authentication?",
                    new List<QuizAnswer>
                    {
                        new QuizAnswer("Using two passwords", false),
                        new QuizAnswer("Verifying identity with two methods", true),
                        new QuizAnswer("Logging in from two devices", false),
                        new QuizAnswer("Having two security questions", false)
                    },
                    "2FA requires password plus device verification."),

                new QuizQuestion(
                    "What should you do before public WiFi?",
                    new List<QuizAnswer>
                    {
                        new QuizAnswer("Disable file sharing", true),
                        new QuizAnswer("Nothing special needed", false),
                        new QuizAnswer("Tell everyone your name", false),
                        new QuizAnswer("Check WiFi speed first", false)
                    },
                    "Disable file sharing to prevent unauthorized access."),

                new QuizQuestion(
                    "How often update software?",
                    new List<QuizAnswer>
                    {
                        new QuizAnswer("Only when broken", false),
                        new QuizAnswer("Once a year", false),
                        new QuizAnswer("When updates are available", true),
                        new QuizAnswer("Never", false)
                    },
                    "Updates include critical security patches."),

                new QuizQuestion(
                    "Common phishing sign?",
                    new List<QuizAnswer>
                    {
                        new QuizAnswer("Poor spelling", true),
                        new QuizAnswer("Professional design", false),
                        new QuizAnswer("HTTPS in URL", false),
                        new QuizAnswer("Customer reviews", false)
                    },
                    "Phishing sites often have spelling errors."),

                new QuizQuestion(
                    "Never share on social media?",
                    new List<QuizAnswer>
                    {
                        new QuizAnswer("Pet's name", false),
                        new QuizAnswer("Vacation plans", false),
                        new QuizAnswer("Mother's maiden name", true),
                        new QuizAnswer("All of above", true)
                    },
                    "Personal details answer security questions."),

                new QuizQuestion(
                    "What is malware?",
                    new List<QuizAnswer>
                    {
                        new QuizAnswer("A type of virus", false),
                        new QuizAnswer("Harmful software", true),
                        new QuizAnswer("Slow internet", false),
                        new QuizAnswer("Outdated OS", false)
                    },
                    "Malware includes viruses, spyware, ransomware."),

                new QuizQuestion(
                    "Why log out of accounts?",
                    new List<QuizAnswer>
                    {
                        new QuizAnswer("Save electricity", false),
                        new QuizAnswer("Prevent unauthorized access", true),
                        new QuizAnswer("Make computer faster", false),
                        new QuizAnswer("All of above", false)
                    },
                    "Prevents access if device is compromised.")
            };
        }

        private void DisplayQuestion()
        {
            if (_currentQuestionIndex < 0 || _currentQuestionIndex >= _questions.Count)
            {
                throw new IndexOutOfRangeException("Invalid question index");
            }

            var question = _questions[_currentQuestionIndex];
            _selectedAnswer = null;

            QuestionNumber.Text = $"Question {_currentQuestionIndex + 1}/{_questions.Count}";
            ScoreText.Text = $"Score: {_score}";
            QuestionText.Text = question.Text;
            AnswersList.ItemsSource = question.Answers;

            PrevButton.IsEnabled = _currentQuestionIndex > 0;
            NextButton.Content = _currentQuestionIndex < _questions.Count - 1 ? "Next" : "Finish";
        }

        private void Answer_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.DataContext is QuizAnswer answer)
            {
                _selectedAnswer = answer;
                bool isCorrect = answer.IsCorrect;

                if (isCorrect) _score++;

                QuestionText.Text += $"\n\n{(isCorrect ? "✓ Correct" : "✗ Incorrect")}: {_questions[_currentQuestionIndex].Explanation}";

                _chatBot.MemoryManager.LogActivity(
                    $"Quiz Q{_currentQuestionIndex + 1}: {(isCorrect ? "Correct" : "Incorrect")}");
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentQuestionIndex < _questions.Count - 1)
            {
                _currentQuestionIndex++;
                DisplayQuestion();
            }
            else
            {
                FinishQuiz();
            }
        }

        private void FinishQuiz()
        {
            string performance = GetPerformanceRating();
            string result = $"Score: {_score}/{_questions.Count}\n{performance}";

            MessageBox.Show(result, "Quiz Complete",
                MessageBoxButton.OK, MessageBoxImage.Information);

            _chatBot.MemoryManager.LogActivity(
                $"Quiz completed: {_score}/{_questions.Count} - {performance}");

            Close();
        }

        private void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentQuestionIndex > 0)
            {
                _currentQuestionIndex--;
                DisplayQuestion();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private string GetPerformanceRating()
        {
            double percentage = (double)_score / _questions.Count;

            if (percentage >= 0.9) return "Excellent!";
            if (percentage >= 0.7) return "Good job.";
            if (percentage >= 0.5) return "Fair.";
            return "Review basics.";
        }

        private class QuizQuestion
        {
            public string Text { get; }
            public List<QuizAnswer> Answers { get; }
            public string Explanation { get; }

            public QuizQuestion(string text, List<QuizAnswer> answers, string explanation)
            {
                Text = text ?? throw new ArgumentNullException(nameof(text));
                Answers = answers ?? throw new ArgumentNullException(nameof(answers));
                Explanation = explanation ?? throw new ArgumentNullException(nameof(explanation));
            }
        }

        private class QuizAnswer
        {
            public string Text { get; }
            public bool IsCorrect { get; }

            public QuizAnswer(string text, bool isCorrect)
            {
                Text = text ?? throw new ArgumentNullException(nameof(text));
                IsCorrect = isCorrect;
            }
        }
    }
}