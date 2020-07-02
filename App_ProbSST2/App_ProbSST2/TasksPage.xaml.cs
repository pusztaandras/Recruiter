using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App_ProbSST2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TasksPage : ContentPage
    {
        public TasksPage()
        {
            InitializeComponent();
            WM_preview.Source = ImageSource.FromResource("App_ProbSST2.Pictures.WM.4_3.jpg");
            SST_preview.Source = ImageSource.FromResource("App_ProbSST2.Pictures.SST.GoR.bmp");
        }

        private async void WMicon_Tapped(object sender, EventArgs e)
        {
            WMTaskPage Page = new WMTaskPage();
            await Navigation.PushAsync(Page);
        }

        private async void SSTicon_Tapped(object sender, EventArgs e)
        {
            SST_Intro Page = new SST_Intro();
            await Navigation.PushAsync(Page);
        }
    }
}