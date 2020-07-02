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
    public partial class SST_Intro : ContentPage
    {
        public SST_Intro()
        {
            InitializeComponent();
            goL.Source = ImageSource.FromResource("App_ProbSST2.Pictures.SST.GoL.bmp");
            goR.Source = ImageSource.FromResource("App_ProbSST2.Pictures.SST.GoR.bmp");
            high.Source = ImageSource.FromResource("App_ProbSST2.Pictures.SST.High.bmp");
            low.Source = ImageSource.FromResource("App_ProbSST2.Pictures.SST.Low.bmp");
            stop.Source = ImageSource.FromResource("App_ProbSST2.Pictures.SST.Stop.bmp");
        }

        private async void Grid_Tapped(object sender, EventArgs e)
        {
            //SST_TrialPage Page = new SST_TrialPage(new SettingsClass_SST());
            TrialPage_SST Page = new TrialPage_SST(new SettingsClass_SST());
            await Navigation.PushAsync(Page);
        }
    }
}