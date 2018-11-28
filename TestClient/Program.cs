using System;
using SignalRChat;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            ChatClientModel ccm = new ChatClientModel();

            while (true) {
                //if()
                Console.WriteLine(ccm.Messages);
                System.Threading.Thread.Sleep(100);
            }
        }
    }
}
