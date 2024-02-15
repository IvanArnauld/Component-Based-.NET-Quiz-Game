using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace QuizGameLibrary
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class GameQuestions : IGameQuestions
    {
        // Private member variables
        private readonly Dictionary<string, ICallBack> callbacks = new Dictionary<string, ICallBack>();
        private readonly Dictionary<string, ICallBack> callbacks2 = new Dictionary<string, ICallBack>();
        private readonly List<Question> questions = new List<Question>();   // Collection of Question objects managed by the GameQuestion object
        private readonly List<Player> users = new List<Player>();
        private readonly List<Player> userspoints = new List<Player>();
        private int questionsIndex = 0;
        const int NUMBEROFQUESTIONS = 20;
        private int a = 0;

        public uint NumQuestions
        {
            get { return (uint)(questions.Count - questionsIndex); }
        }

        public uint GetNumberOfPlayers()
        {
            return (uint)users.Count;
        }

        public GameQuestions()
        {
            FillQuestions();
        }

        public Question GetQuestion()
        {
            if (questionsIndex == questions.Count)
            {
                throw new InvalidOperationException("No more questions.");
            }
            return questions[questionsIndex++];
        }

        private void FillQuestions()
        {
            questions.Clear();

            Random random = new Random();
            int allQuestionsLength = QuestionLibrary.AllQuestions.Count;

            for (int i = 0; i < NUMBEROFQUESTIONS; i++)
            {
                int index = random.Next(allQuestionsLength);

                var question = QuestionLibrary.AllQuestions[index];

                while(questions.Any(q => q.QuestionText == question.QuestionText))
                {
                    index = random.Next(allQuestionsLength);
                    question = QuestionLibrary.AllQuestions[index];
                }

                questions.Add(question);
            }

        }

        // Stores the user's unique name or alias and subscribes the user's client to the
        // callbacks messages
        public bool Join(string name)
        {
            if (callbacks.ContainsKey(name.ToUpper()))
                // User alias must be unique
                return false;
            else
            {
                // Retrieve client's callback proxy
                ICallBack cb = OperationContext.Current.GetCallbackChannel<ICallBack>();

                // Save alias and callback proxy
                callbacks.Add(name.ToUpper(), cb);

                return true;
            }
        }
        public bool Join2(string name)
        {
            if (callbacks2.ContainsKey(name.ToUpper()))
                // User alias must be unique
                return false;
            else
            {
                // Retrieve client's callback proxy
                ICallBack cb = OperationContext.Current.GetCallbackChannel<ICallBack>();

                // Save alias and callback proxy
                callbacks2.Add(name.ToUpper() + a++, cb);

                return true;
            }
        }
        // Removes the user's name or alias amd unscubscribes from the callback messages for
        // any client that invokes this method
        public void Leave(string name)
        {
            if (callbacks.ContainsKey(name.ToUpper()))
            {
                callbacks.Remove(name.ToUpper());
            }
        }

        // Triggers a callback containing all posted message for all clients that are subscribed
        private void UpdateAllUsers()
        {
            Player[] usrs = users.ToArray<Player>();
            Player[] pts = userspoints.ToArray<Player>();
            //string[] points = users.ToArray<string>();
            foreach (ICallBack cb in callbacks.Values)
            {
                cb.SendAllPlayers(usrs);
                cb.SendAllPoints(pts);
            }
        }

        // Receives a message from a client and inserts it into the list
        public void PostUserName(string message)
        {
            users.Insert(0, new Player(message));
            UpdateAllUsers();
        }

        public void PostUser(string name,uint points)
        {
            int i = 0;
            bool found = false;
            while(i < userspoints.Count)
            {
                if (userspoints[i].name == name)
                {
                    found = true;
                    break;
                }
                i++;
            }
            if(found)
            {
                userspoints[i] = new Player(name, points);
            }
            else
            {
                userspoints.Insert(0, new Player(name, points));
            }
            
            UpdateAllUsers();
        }
    }
}
