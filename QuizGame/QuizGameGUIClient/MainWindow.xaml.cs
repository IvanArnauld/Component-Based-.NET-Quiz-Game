using QuizGameLibrary;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Windows;

namespace QuizGameGUIClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ICallBack
    {
        public IGameQuestions questions;
        public string username;
        public List<Player> players = new List<Player>();
        private uint numPlayers = 0;
        public Player[] leads;
        public MainWindow()
        {
            InitializeComponent();
            StartGameButton.IsEnabled = false;
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            GameWindow gameWindow = new GameWindow();
            gameWindow.Show();
            this.Close();
        }

        // ---------------------- Helper methods ----------------------

        private bool ConnectToGameBoard()
        {
            try
            {
                // Configure the ABCs of using the MessageBoard service
                DuplexChannelFactory<IGameQuestions> channel = new DuplexChannelFactory<IGameQuestions>(this, "GameQuestionsService");

                // Activate a MessageBoard object
                questions = channel.CreateChannel();
                if (questions.Join(UsernameTextBox.Text))
                {
                    // Alias accepted by the service so update GUI
                    //ListMessages.ItemsSource = msgBrd.GetAllMessages();
                    UsernameTextBox.IsEnabled = JoinGameLobbyButton.IsEnabled = false;
                    return true;
                }
                else
                {
                    // Alias rejected by the service so nullify service proxies
                    questions = null;
                    MessageBox.Show("ERROR: Username in use. Please try again.");
                    return false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return false;
        }

        private void JoinGameLobbyButton_Click(object sender, RoutedEventArgs e)
        {
            if (UsernameTextBox.Text != "")
            {
                try
                {
                    username = UsernameTextBox.Text;
                    if(ConnectToGameBoard())
                    {
                        players.Add(new Player(username));
                        questions.PostUserName(username + " joined the lobby...");
                        
                    }
                    else
                    {
                        JoinGameLobbyButton.IsEnabled = UsernameTextBox.IsEnabled = true;
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private delegate void GuiUpdateDelegate(Player[] messages);

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
                    PlayersInLobbyListBox.ItemsSource = names;
                    Check(messages);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                this.Dispatcher.BeginInvoke(new GuiUpdateDelegate(SendAllPlayers), new object[] { messages });
        }
        private delegate void GuiUpdateDelegate2(Player[] messages);
        public void SendAllPoints(Player[] messages)
        {
            if (this.Dispatcher.Thread == System.Threading.Thread.CurrentThread)
            {
                try
                {
                    leads = messages;
                    //Check(messages);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
                this.Dispatcher.BeginInvoke(new GuiUpdateDelegate2(SendAllPoints), new object[] { messages });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            questions.Leave(UsernameTextBox.Text);
        }

        private void Check(Player[] messages)
        {
            numPlayers = (uint)messages.Length;
            if (numPlayers >= 2 && numPlayers <= 8)
            {
                StartGameButton.IsEnabled = true;
                WaitingPlayers_TextBlock.Text = "Good to start...";
            }
            else
            {
                WaitingPlayers_TextBlock.Text = "Waiting minimum two players to start...";
            }
        }

        // Populates the questions

    }
}
