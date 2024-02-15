using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace QuizGameLibrary
{
    [DataContract]
    public class Question
    {
        [DataMember]
        private string _questionText;
        [DataMember]
        private string _answer;
        [DataMember]
        public string QuestionText
        {
            get { return _questionText; }
            private set { _questionText = value; }
        }
        [DataMember]
        public string Answer
        {
            get { return _answer; }
            private set { _answer = value; }
        }

        //Question question = new();
        // question.Answer will get the answer
        // question.QuestionText will get the question
        [DataMember]
        private List<string> Options;

        public Question()
        {
        }

        internal Question(string questionText, string answer, List<string> options)
        {
            this._questionText = questionText;
            this._answer = answer;
            this.Options = options;
            ShuffleOptions();
        }

        private void ShuffleOptions()
        {
            Options.Add(this.Answer);
            Random random = new Random();
            Options = Options.OrderBy(option => random.Next()).ToList();
        }

        public List<string> GetOptions()
        {
            //List<string> options = new List<string>();

            //options.Add(this.Answer);
            //foreach (string item in this.Options)
            //{
            //    options.Add(item);
            //}
            //if (!optionsShuffled)
            //{
            //    Random random = new Random();
            //    options = options.OrderBy(option => random.Next()).ToList();
            //    optionsShuffled = true;
            //}
            return Options;
        }

        public override string ToString()
        {
            string result = "";
            List<string> options = GetOptions();
            result += $"{_questionText}\n";

            char questionNumber = 'a';
            result += "\nChoose an option from the list below:\n";
            foreach (string item in options)
            {
                //make this random
                result += $"{questionNumber++}. {item}\n";
            }

            result += "\n";

            return result;
        }

    }
}
