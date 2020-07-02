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
    public partial class WM_Intro : ContentPage
    {
        public int partnumb;
        public WM_Intro(int a)
        {
            InitializeComponent();
            partnumb = a;
            SegmentedCircle.Source = ImageSource.FromResource(String.Concat("App_ProbSST2.Pictures.WM.",partnumb.ToString(),"_2.jpg"));
            FruitImage1.Source = ImageSource.FromResource("App_ProbSST2.Pictures.WM.pear.jpg");
            FruitImage2.Source = ImageSource.FromResource("App_ProbSST2.Pictures.WM.apple.jpg");
            
        }

        private async void StartButton_Clicked(object sender, EventArgs e)
        {
            TrialPage_WM Page = new TrialPage_WM(new SettingsClass_WM(partnumb), new PersonData());
            await Navigation.PushAsync(Page);
        }
    }
}