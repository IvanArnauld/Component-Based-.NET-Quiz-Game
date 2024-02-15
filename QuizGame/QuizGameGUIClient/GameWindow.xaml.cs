using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.ServiceModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using QuizGameLibrary;
using System.Windows.Threading;

namespace QuizGameGUIClient
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    ///
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
    public partial class GameWindow : Window, ICallBack
    {
        private static MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
        private IGameQuestions questions = mainWindow.questions;
        private Question currentQuestion;
        private List<string> currentOptions;
        private uint playerPoints = 0;
        private int questionNumber = 1;
        public GameWindow()
        {
            InitializeComponent();
            try
            {
                // Initialize the GUI
                PopulateQuestions();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       
        private void NextQuestion_Button_Click(object sender, RoutedEventArgs e)
        {
            if (questionNumber == 20)
            {
                LeaderBoardListBox.Background = Brushes.Black;
                LeaderBoardListBox.Foreground = Brushes.Green;
            }
            else
            {
                PopulateQuestions();

                AnswerChoice_TextBox.Clear();

                //QuestionNumber_Label.Content = $"Question {questionNumber++}:";
                A_RadioButton.IsEnabled = true;
                B_RadioButton.IsEnabled = true;
                C_RadioButton.IsEnabled = true;
                D_RadioButton.IsEnabled = true;

                A_RadioButton.IsChecked = false;
                B_RadioButton.IsChecked = false;
                C_RadioButton.IsChecked = false;
                D_RadioButton.IsChecked = false;

                AnswerFeedback_Label.Content = "";
                SubmitAnswer_Button.IsEnabled = false;
            }
            LeaderBoardListBox.ItemsSource = mainWindow.leads;
        }

        private void A_RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            AnswerChoice_TextBox.Text = currentOptions[0];
        }

        private void B_RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            AnswerChoice_TextBox.Text = currentOptions[1];
        }

        private void C_RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            AnswerChoice_TextBox.Text = currentOptions[2];
        }

        private void D_RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            AnswerChoice_TextBox.Text = currentOptions[3];
        }

        // ---------------------- Helper methods ----------------------

        // Populates the questions
        private void PopulateQuestions()
        {
            NextQuestion_Button.IsEnabled = false;
            PlayerPoints_TextBlock.Text = $"{mainWindow.username}: {playerPoints} points";
            
            QuestionNumber_Label.Content = $"Question {questionNumber++}:";

            AnswerChoice_TextBox.IsReadOnly = true;

            currentQuestion = questions.GetQuestion();
            currentOptions = currentQuestion.GetOptions();

            Question_TextBox.Text = currentQuestion.ToString();// + $"Answer: {currentQuestion.Answer}";
            
            SubmitAnswer_Button.IsEnabled = false;
        }

        private bool VerifyAnswer()
        {
            if (AnswerChoice_TextBox.Text == currentQuestion.Answer)
            {
                playerPoints += 10;
                PlayerPoints_TextBlock.Text = $"{mainWindow.username}: {playerPoints} points";
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SubmitAnswer_Button_Click(object sender, RoutedEventArgs e)
        {
            if(VerifyAnswer())
            {
                AnswerFeedback_Label.Content = "Correct!";
                AnswerFeedback_Label.Foreground = Brushes.Green;
            }
            else
            {
                AnswerFeedback_Label.Content = "Wrong!";
                AnswerFeedback_Label.Foreground = Brushes.Red;
            }

            A_RadioButton.IsEnabled = false;
            B_RadioButton.IsEnabled = false;
            C_RadioButton.IsEnabled = false;
            D_RadioButton.IsEnabled = false;

            NextQuestion_Button.IsEnabled = true;
            SubmitAnswer_Button.IsEnabled = false;

            if (questionNumber == 20)
            {
                NextQuestion_Button.Content = "See Final Result";
            }
            
            try
            {
                //username = UsernameTextBox.Text;
                if (ConnectToGameBoard())
                {
                    //players.Add(new Player(username));
                    questions.PostUser(mainWindow.username, playerPoints);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private delegate void GuiUpdateDelegate(Player[] messages);
        public void SendAllPoints(Player[] messages)
        {
            if (this.Dispatcher.Thread == System.Threading.Thread.CurrentThread)
            {
                try
                {
                    List<string> values = new List<string>();
                    foreach (var item in messages)
                    {
                        values.Add(item.name);
                    }
                    string[] names = values.ToArray();
                    LeaderBoardListBox.ItemsSource = messages;
                    //Check(messages);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                this.Dispatcher.BeginInvoke(new GuiUpdateDelegate(SendAllPoints), new object[] { messages });
        }
        private delegate void GuiUpdateDelegate2(Player[] messages);
        public void SendAllPlayers(Player[] messages)
        {
            if (this.Dispatcher.Thread == System.Threading.Thread.CurrentThread)
            {
                try
                {
                    List<string> values = new List<string>();
                    foreach (var item in messages)
                    {
                        values.Add(item.name);
                    }
                    string[] names = values.ToArray();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                this.Dispatcher.BeginInvoke(new GuiUpdateDelegate2(SendAllPlayers), new object[] { messages });
        }

        private bool ConnectToGameBoard()
        {
            try
            {
                // Configure the ABCs of using the MessageBoard service
                DuplexChannelFactory<IGameQuestions> channel = new DuplexChannelFactory<IGameQuestions>(this, "GameQuestionsService");

                // Activate a MessageBoard object
                questions = channel.CreateChannel();
                if (questions.Join2(mainWindow.username + playerPoints))
                {

                    return true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return true;
        }

        private void AnswerChoice_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SubmitAnswer_Button.IsEnabled = true;
        }

    }
}
