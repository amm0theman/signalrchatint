using System;
using System.Collections.Generic;
using System.Text;
using NUnit;
using NUnit.Compatibility;
using NUnit.Framework;
using Moq;
using SignalRChat;
using ServerSignalR;
using System.Data.SqlClient;

namespace SignalRChatTests
{
    [TestFixture]
    class FinalSprintTests
    {
        //public interface IClientHubProxy
        //{
        //    void setName();
        //    void startHub();
        //    void sendMessage(string user, string message);
        //    void getLog();
        //    event EventHandler<MessageEventArgs> MessageReceived;
        //    event EventHandler<MessageEventArgs> UsernameReceived;
        //    event EventHandler<LogEventArgs> LogReceived;
        //    event EventHandler<UsersArgs> UsernamesReceived;
        //    event EventHandler<MessageEventArgs> LocalUsernameReceived;
        //}

        //public interface IDispatcher
        //{
        //    void Invoke(Action a);
        //}

        Mock<IDispatcher> dispatcher;
        Mock<IClientHubProxy> hubproxy;
        ChatVM chatVM;
        CreateUser u = new CreateUser();
        

        [SetUp]
        public void SetUp()
        {
            dispatcher = new Mock<IDispatcher>();
            hubproxy = new Mock<IClientHubProxy>();
            chatVM = new ChatVM(hubproxy.Object, dispatcher.Object);

            dispatcher.Setup(mock => mock.Invoke(It.IsAny<Action>())).Callback<Action>(action => action());
        }

        [Test]
        public void userLoggedOnThenAddedToList()
        {
            hubproxy.Setup(mock => mock.setName()).Callback(new Action(() => chatVM.receivedUsername(null, new MessageEventArgs { User = "Test" })));
            chatVM.setName();
            Assert.Contains("Test", chatVM.Users);
        }

        [Test]
        public void userLoggedOnSetLocal()
        {
            hubproxy.Setup(mock => mock.setName()).Callback(new Action(() => chatVM.receivedLocalUser(null, new MessageEventArgs { User = "Test" })));
            chatVM.setName();
            Assert.AreEqual("Test", chatVM.LocalUser);
        }

        [Test]
        public void logOnHaveMultipleConnectedUsers()
        {
            hubproxy.Setup(mock => mock.setName()).Callback(new Action(() => chatVM.receivedUsername(null, new MessageEventArgs { User = "Test" })));
            chatVM.setName();
            chatVM.receivedUsername(null, new MessageEventArgs { User = "Test2" });
            Assert.IsTrue(chatVM.Users.Count > 1);
        }

        [Test]
        public void userDisconnectedMessage()
        {
            //all the logic is on the server for this one, so simulated what happens clientside
            chatVM.receivedMessage(null, new MessageEventArgs { User = "Test", Message = "Has Disconnected" });
            Assert.Contains("Test: Has Disconnected", chatVM.ChatLog);
        }

        [Test]
        public void chatMessagesShowUpInChatWhenReceived()
        {
            chatVM.receivedMessage(null, new MessageEventArgs { User = "Test", Message = "Test" });
            chatVM.receivedMessage(null, new MessageEventArgs { User = "Test", Message = "Test" });
            chatVM.receivedMessage(null, new MessageEventArgs { User = "Test", Message = "Test" });
            Assert.IsTrue(chatVM.ChatLog.Count > 2);
        }

        [Test]
        public void ensureUserNamesStoredOnServer()
        {
            //ServerSignalR.ServerSignalR server = new ServerSignalR.ServerSignalR();
            //ClientHubProxy proxy = new ClientHubProxy("http://localhost:8080", "chat");
            //proxy.setName();
            //Assert.IsTrue(true);
        }

        [Test]
        public void chatLogStoredOnServer()
        {
            
        }

        [Test]
        public void chatLogRetrievedToClientAndDisplayedRightSide()
        {

        }

        [Test]
        public void whenMessageSentTextboxCleared()
        {

        }

        [Test]
        public void userRemovedFromListOnDisconnect()
        {

        }

        [Test]
        public void addUserTest()
        {
            SqlConnection cn = CreateUser.OpenDBConnection;
            u.Add_User("hi", "there");
        }
    }
}
