using System;
using Xamarin.Forms;

namespace App_ProbSST2
{
    class TrialList
    {
        public int trialNum { get; set; }

        public ImageSource CueImage { get; set; }

        public ImageSource AnsImage1 { get; set; }

        public ImageSource AnsImage2 { get; set; }

        public string AnsImage1Name { get; set; } // name of the correct AnsImage

        public string CueImageName { get; set; }

        public TrialList(SettingsClass_WM settings, int iterationNum, string Cue, string Answer)
        {
            string answer2Name;
            trialNum = iterationNum;
            CueImage = settings.ImageNames[Cue];
            CueImageName = Cue;
            AnsImage1 = settings.ImageNames[Answer];
            if(Answer.Contains("A"))
            {
                answer2Name = String.Concat("B", Answer[1]);
            }
            else
            {
                answer2Name = String.Concat("A", Answer[1]);
            }
            AnsImage2 = settings.ImageNames[answer2Name];

            AnsImage1Name = Answer;
        }
    }
}
