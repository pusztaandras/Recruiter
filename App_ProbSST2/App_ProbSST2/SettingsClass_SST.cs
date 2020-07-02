﻿using System;
using System.IO;
using Xamarin.Forms;

namespace App_ProbSST2
{
    public class SettingsClass_SST
    {

        public string gender { get; set; }

        public int age { get; set; }
        public bool isDisorder { get; set; }

        public bool isDrug { get; set; }

        public string Disorder { get; set; }

        public string Drug { get; set; }
        public int TrialNumGoHighR { get; set; }
        public int TrialNumGoLowR { get; set; }
        public int TrialNumStopHighR { get; set; }
        public int TrialNumStopLowR { get; set; }

        public int TrialNumGoHighL { get; set; }
        public int TrialNumGoLowL { get; set; }
        public int TrialNumStopHighL { get; set; }
        public int TrialNumStopLowL { get; set; }

        public int FeedbackTime { get; set; }

        public int minCueTime { get; set; }
        public int maxCueTime { get; set; }

        public int StartSSD { get; set; }

        public int SSDWrong { get; set; }

        public int SSDCorrect { get; set; }

        public FileStream File { get; set; }

        public int BlockNum { get; set; }

        public int minITITime { get; set; }
        public int maxITITime { get; set; }
        public int maxWaitingTime { get; set; }




        public SettingsClass_SST()
        {
            Go = ImageSource.FromResource("App_ProbSST2.Pictures.SST.Go.bmp");
            GoR = ImageSource.FromResource("App_ProbSST2.Pictures.SST.GoR.bmp");
            GoL = ImageSource.FromResource("App_ProbSST2.Pictures.SST.GoL.bmp");
            Stop = ImageSource.FromResource("App_ProbSST2.Pictures.SST.Stop.bmp");
            High = ImageSource.FromResource("App_ProbSST2.Pictures.SST.High.bmp");
            Low = ImageSource.FromResource("App_ProbSST2.Pictures.SST.Low.bmp");


            ////////////////////////////////////////////////////////
            /////////Settings (change here if necessarry)//////////
            ///////////////////////////////////////////////////////

            //////Trial numbers of one block:////////
            TrialNumGoHighR = 12;
            TrialNumGoHighL = 12;
            TrialNumGoLowR = 15;
            TrialNumGoLowL = 15;
            TrialNumStopHighR = 8;
            TrialNumStopHighL = 8;
            TrialNumStopLowR = 5;
            TrialNumStopLowL = 5;


            BlockNum = 6; //number of blocks

            ////Debug-mode:uncomment these:
            //TrialNumGoHighR = 1;
            //TrialNumGoLowR = 1;
            //TrialNumStopHighR = 1;
            //TrialNumStopLowR = 1;
            //TrialNumGoHighL = 1;
            //TrialNumGoLowL = 1;
            //TrialNumStopHighL = 1;
            //TrialNumStopLowL = 1;

            //////Timing/////
            minCueTime = 700;
            maxCueTime = 1000;
            
            maxWaitingTime = 1000;
            StartSSD = 250;
            SSDCorrect = 20;
            SSDWrong = 30;

            FeedbackTime = 10000;
            
            minITITime = 500;
            maxITITime = 800;


        }

        public ImageSource Low;
        public ImageSource High;
        public ImageSource Go;
        public ImageSource GoL;
        public ImageSource GoR;

        public ImageSource Stop;
    }

    
    
}
