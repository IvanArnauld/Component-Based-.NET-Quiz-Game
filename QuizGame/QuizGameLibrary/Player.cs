using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace QuizGameLibrary
{
    [DataContract]
    public class Player
    {
        [DataMember]
        public string name = "";
        [DataMember]
        public uint points = 0;
        public Player(string name)
        {
            this.name = name;
        }
        public Player(string name, uint points)
        {
            this.name = name;
            this.points = points;
        }
        public override string ToString()
        {
            return $"{name}: {points} points";
        }
    }
}
