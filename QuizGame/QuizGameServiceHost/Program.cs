using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizGameLibrary;
using System.ServiceModel;

namespace QuizGameServiceHost
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceHost servHost = null;

            try
            {
                servHost = new ServiceHost(typeof(GameQuestions));//, new Uri("net.tcp://localhost:13000/QuizGameLibrary/"));

                //servHost.AddServiceEndpoint(typeof(IGameQuestions), new NetTcpBinding(), "QuizService");

                servHost.Open();

                Console.WriteLine("Service started. Press any key to quit.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Wait for a keystroke
                Console.ReadKey();

                servHost?.Close();
            }
        }
    }
}
