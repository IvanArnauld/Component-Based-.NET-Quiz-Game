using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace QuizGameLibrary
{
    public interface ICallBack
    {
        [OperationContract(IsOneWay = true)]
        void SendAllPlayers(Player[] messages);
        [OperationContract(IsOneWay = true)]
        void SendAllPoints(Player[] messages);

    }
}
