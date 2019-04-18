using System;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;
using Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin;
using System.Collections.ObjectModel;
using SignalRChat.Entities.Validation;

[assembly: OwinStartup(typeof(ServerSignalR.Startup))]

namespace ServerSignalR
{
    public class ServerSignalR
    {
        //Public for testing, idk if there is a better way to do this
        public static void Main(string[] args)
        {
            // This will *ONLY* bind to localhost, if you want to bind to all addresses
            // use http://*:8080 to bind to all addresses. 
            // See http://msdn.microsoft.com/library/system.net.httplistener.aspx 
            // for more information.
            string url = "http://localhost:8080";
            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }
    }
    public class Startup
    {
        //Globals for the program. These are bad and will be removed when database is added. TODO: Add database remove these lines
        public ObservableCollection<string> connectedUsers = new ObservableCollection<string>();
        public ObservableCollection<string> chatLog = new ObservableCollection<string>();

        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.Properties["host.AppMode"] = "development";
            app.UseErrorPage(new Microsoft.Owin.Diagnostics.ErrorPageOptions { ShowExceptionDetails = true });

            //Register dependencies
            GlobalHost.DependencyResolver.Register(
                typeof(ChatHub),
                () => new ChatHub(ref connectedUsers, ref chatLog));

            GlobalHost.DependencyResolver.Register(
                typeof(DataValidator),
                () => new DataValidator());

            var hubConfiguration = new HubConfiguration
            {
                EnableJSONP = true,
                EnableDetailedErrors = true
            };
            app.MapSignalR(hubConfiguration);
        }
    }
}
