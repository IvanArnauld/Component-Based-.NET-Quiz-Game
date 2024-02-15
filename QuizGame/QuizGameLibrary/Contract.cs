using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace QuizGameLibrary
{
    [ServiceContract(CallbackContract = typeof(ICallBack))]
    public interface IGameQuestions
    {
        [OperationContract]
        Question GetQuestion();
        uint NumQuestions { [OperationContract] get; }
        [OperationContract]
        uint GetNumberOfPlayers();
        [OperationContract]
        bool Join(string name);
        [OperationContract]
        bool Join2(string name);
        [OperationContract(IsOneWay = true)]
        void Leave(string name);
        [OperationContract(IsOneWay = true)]
        void PostUserName(string message);
        [OperationContract(IsOneWay = true)]
        void PostUser(string name, uint pts);
    }
}
