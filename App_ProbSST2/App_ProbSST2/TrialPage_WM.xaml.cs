using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

namespace App_ProbSST2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrialPage_WM : ContentPage
    {

        public int currTrialinPhase;
        public int currTrial = 1;
        public Random rnd = new Random();
        public bool canPress = false;
        public bool isPressed = false;
        public int currPhase;

        public bool isLate;
        public bool isCorrect;
        public bool restartPhase;
        public bool isTerminated = false;
        public int RT;
        public bool isEnded { get; set; }
        public string pressedSide;

        public string CurrCue;
        public string CurrAns;
        public string AnsLocation;

        Stopwatch stopwatcha = new Stopwatch();

        Stopwatch stopwatchb = new Stopwatch();

        public SettingsClass_WM settings;
        public PersonData person;
        public int phase;
        public TrialPage_WM(SettingsClass_WM setting, PersonData personDat)
        {
            InitializeComponent();
            settings = setting;
            person = personDat;
            WriteAnsToFile(setting.FileName, String.Concat("gender:", person.gender, "\n"));
            WriteAnsToFile(setting.FileName, String.Concat("age:", person.age.ToString(), "\n"));
            WriteAnsToFile(setting.FileName, String.Concat("isDrug:", person.isDrug.ToString(), "\n"));
            WriteAnsToFile(setting.FileName, String.Concat("Drug:", person.Drug, "\n"));
            WriteAnsToFile(setting.FileName, String.Concat("isDisease:", person.isDisease.ToString(), "\n"));
            WriteAnsToFile(setting.FileName, String.Concat("Disease:", person.Disease, "\n"));

            WriteAnsToFile(setting.FileName, "Picture Names:\n");
            WriteAnsToFile(setting.FileName, String.Concat("X1:", setting.ImageNames["X1"].ToString().Substring(setting.ImageNames["X1"].ToString().Length - 5, 5)));
            WriteAnsToFile(setting.FileName, String.Concat("X2:", setting.ImageNames["X2"].ToString().Substring(setting.ImageNames["X2"].ToString().Length - 5, 5)));
            WriteAnsToFile(setting.FileName, String.Concat("X3:", setting.ImageNames["X3"].ToString().Substring(setting.ImageNames["X3"].ToString().Length - 5, 5)));
            WriteAnsToFile(setting.FileName, String.Concat("Y1:", setting.ImageNames["Y1"].ToString().Substring(setting.ImageNames["Y1"].ToString().Length - 5, 5)));
            WriteAnsToFile(setting.FileName, String.Concat("Y2:", setting.ImageNames["Y2"].ToString().Substring(setting.ImageNames["Y2"].ToString().Length - 5, 5)));
            WriteAnsToFile(setting.FileName, String.Concat("Y3:", setting.ImageNames["Y3"].ToString().Substring(setting.ImageNames["Y3"].ToString().Length - 5, 5)));
            WriteAnsToFile(setting.FileName, String.Concat("A1:", setting.ImageNames["A1"].ToString().Substring(setting.ImageNames["A1"].ToString().Length - 5, 5)));
            WriteAnsToFile(setting.FileName, String.Concat("A2:", setting.ImageNames["A2"].ToString().Substring(setting.ImageNames["A2"].ToString().Length - 5, 5)));
            WriteAnsToFile(setting.FileName, String.Concat("B1:", setting.ImageNames["B1"].ToString().Substring(setting.ImageNames["B1"].ToString().Length - 5, 5)));
            WriteAnsToFile(setting.FileName, String.Concat("B2:", setting.ImageNames["B2"].ToString().Substring(setting.ImageNames["B2"].ToString().Length - 5, 5)));



            isEnded = false;

            RunPhase();


            if (isEnded)
            {

            }

            //Closing += TrialWindow_Closed;


        }

        //private void TrialWindow_Closed(object sender, EventArgs e)
        //{
        //    if (isEnded)
        //    {
        //        var EndBloc = new SelectRuleWindow(settings, person);
        //    }
        //    else
        //    {
        //        if (person.IsMore)
        //        {
        //            var endBlock = new EndofBlock_wantmore(person);
        //            endBlock.Show();
        //        }
        //        else
        //        {
        //            var endBlock = new ByeWindow();
        //            endBlock.Show();
        //        }
        //    }
        //    settings.FileName.Close();


        //}



        private async void RunPhase()
        {
            int[] phaseArray = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 16 };

            for (int p = 0; p < phaseArray.Length + 1; p++)
            {
                currPhase = p + 1;
                int v = phaseArray[p];
                //intitialize current phaseList
                List<TrialList> PhaseList = new List<TrialList>();
                PhaseList = newPhase(v);

                //Run Trials
                int trialNumsinPhase = 0;
                foreach (TrialList a in PhaseList)
                {
                    trialNumsinPhase += a.trialNum;
                }
                currTrialinPhase = 1;
                while ((currTrialinPhase < trialNumsinPhase + 1))
                {
                    if (restartPhase)
                    {
                        PhaseList = newPhase(v);
                        currTrialinPhase = 1;
                        Debug.WriteLine("Phase resetted");
                        restartPhase = false;
                    }
                    int s = rnd.Next(PhaseList.Count() - 1);
                    CurrCue = PhaseList[s].CueImageName;
                    CurrAns = PhaseList[s].AnsImage1Name;
                    await RunTrial(PhaseList[s]);

                    PhaseList[s].trialNum -= 1;
                    if (PhaseList[s].trialNum == 0)
                    {
                        PhaseList.RemoveAt(s);
                    }
                    currTrial += 1;
                    currTrialinPhase += 1;
                    //Debug.WriteLine(String.Concat("TrialinPhase ", currTrialinPhase.ToString()));
                }
                if (p == phaseArray.Length)
                {
                    isEnded = true;
                }

            }
        }

        private List<TrialList> newPhase(int v)
        {
            List<TrialList> PhaseList = new List<TrialList>();
            for (int p2 = 0; p2 < v; p2++)
            {
                int iterationperpairs;
                if (v < 9)
                {
                    iterationperpairs = settings.Learningitinum;
                }
                else
                {
                    iterationperpairs = settings.Testitinum;
                }
                PhaseList.Add(new TrialList(settings, iterationperpairs, settings.CueNames[p2], settings.AnsImageNames[p2]));

            }
            return PhaseList;
        }

        private async Task RunTrial(TrialList trialList)
        {
            isPressed = false;
            canPress = false;

            //Set up image locations
            ImageSource left;
            ImageSource right;

            int g = rnd.Next(2);
            if (g == 0)
            {
                //Correct image to the left
                AnsLocation = "Left";
                left = trialList.AnsImage1;
                right = trialList.AnsImage2;
                //correct Key is D

            }
            else
            {
                //Correct image to the right
                AnsLocation = "Right";
                left = trialList.AnsImage2;
                right = trialList.AnsImage1;
                //correct Key is K
            }


            //ITI 1000-1400ms
            await Task.Run(() => System.Threading.Thread.Sleep(rnd.Next(1000, 1400)));
            //clear all images
            this.Dispatcher.BeginInvokeOnMainThread(() =>
            {
                this.CuePlace.Source = null;
                this.LeftImage.Source = null;
                this.RightImage.Source = null;
                this.FeedBackPlace.Source = null;
                //Debug.WriteLine(String.Concat("Trial ", currTrial.ToString()));


            });



            //stimulus presentation until keyDown or 2000 ms
            this.Dispatcher.BeginInvokeOnMainThread(() =>
            {
                this.CuePlace.Source = trialList.CueImage;
                this.LeftImage.Source = left;
                this.RightImage.Source = right;
                //Debug.WriteLine("Cue presentation");
                stopwatcha.Start();
                canPress = true;
                stopwatchb.Start();



            });



            await Task.Delay(1500, new System.Threading.CancellationToken(isPressed)).ContinueWith(_ =>
            {
                if (!isPressed)
                {
                    isLate = true;
                    stopwatcha.Stop();
                    stopwatcha.Reset();
                    Feedback();
                };

            });

            //Feedback for 1000 ms





            //WriteAnsToFile(settings.FileNameLat, String.Concat(i.ToString(), "||", TrialType2, "||", TrialType, "||", outvar.isCorrect.ToString(), "||", outvar.PressedKey, "||", outvar.isEarly.ToString(), "||", outvar.RT.ToString(), "||", outvar.SSD.ToString(), "\n"));






        }

        private void Feedback()
        {

            canPress = false;
            if (!isPressed)
            {
                Dispatcher.BeginInvokeOnMainThread(() =>
                {
                    FeedBackPlace.Source = settings.ImageNames["Bad"];
                });
                restartPhase = true;
                isCorrect = false;
            }
            else
            {
                if (pressedSide == AnsLocation)
                {
                    Dispatcher.BeginInvokeOnMainThread(() =>
                    {
                        FeedBackPlace.Source = settings.ImageNames["Good"];
                    });
                    isCorrect = true;
                }
                else
                {
                    Dispatcher.BeginInvokeOnMainThread(() =>
                    {
                        FeedBackPlace.Source = settings.ImageNames["Bad"];
                    });
                    restartPhase = true;
                    isCorrect = false;
                }
            }
            stopwatchb.Stop();
            Debug.WriteLine(stopwatchb.ElapsedMilliseconds.ToString());
            stopwatchb.Reset();




            WriteAnsToFile(settings.FileName, String.Concat(currPhase.ToString(), "||", currTrial.ToString(), "||", CurrCue, "||", CurrAns, "||", AnsLocation, "||", pressedSide, "||", isCorrect.ToString(), "||", isLate.ToString(), "||", RT.ToString(), "\n"));




        }
        public void WriteAnsToFile(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            if (fs.CanWrite)
            {
                fs.Write(info, 0, info.Length);
            }

        }

        private void LeftImage_Tapped(object sender, EventArgs e)
        {
            if (canPress)
            {
                isPressed = true;
                pressedSide = "Left";

                RT = Convert.ToInt32(stopwatcha.ElapsedMilliseconds);
                stopwatcha.Stop();
                stopwatcha.Reset();
                Feedback();
                canPress = false;
            }
        }

        private void RightImage_Tapped(object sender, EventArgs e)
        {
            if (canPress)
            {
                isPressed = true;
                pressedSide = "Right";
                RT = Convert.ToInt32(stopwatcha.ElapsedMilliseconds);
                stopwatcha.Stop();
                stopwatcha.Reset();
                Feedback();
                canPress = false;
            }
        }
    }

}