using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ToDo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //By Default
            //MainPage = new MainPage();

            //Add Navigation property to travel b/w pages where MainPage is base page
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
