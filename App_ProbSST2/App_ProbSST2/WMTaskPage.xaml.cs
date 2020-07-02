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
    public partial class WMTaskPage : ContentPage
    {
        public WMTaskPage()
        {
            InitializeComponent();
            level1.Source = ImageSource.FromResource("App_ProbSST2.Pictures.2_2.jpg");
            level2.Source = ImageSource.FromResource("App_ProbSST2.Pictures.3_2.jpg");
            level3.Source = ImageSource.FromResource("App_ProbSST2.Pictures.4_2.jpg");
            level4.Source = ImageSource.FromResource("App_ProbSST2.Pictures.5_2.jpg");

        }

        private async void level1_Tapped(object sender, EventArgs e)
        {
            WM_Intro Page = new WM_Intro(2);
            await Navigation.PushAsync(Page);
        }

        private async void level2_Tapped(object sender, EventArgs e)
        {
            WM_Intro Page = new WM_Intro(3);
            await Navigation.PushAsync(Page);
        }

        private async void level3_Tapped(object sender, EventArgs e)
        {
            WM_Intro Page = new WM_Intro(4);
            await Navigation.PushAsync(Page);
        }

        private async void level4_Tapped(object sender, EventArgs e)
        {
            WM_Intro Page = new WM_Intro(5);
            await Navigation.PushAsync(Page);
        }
    }
}