using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App_ProbSST2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrialPage_SST : ContentPage
    {

        public SettingsClass_SST settings;
        public int SSD;
        public Random rnd = new Random();
        Stopwatch stopwatcha = new Stopwatch();
        OutputVar outvar = new OutputVar();
        public string State;
        string TrialType;
        string TrialType2;
        public string TrialType3;
        bool canPress = false;
        public bool isPressed;
        public bool isCorrect;
        public int currTrialNum;
        public int sameTrials = 0;
        public BlockVarClass BlockVar;
        public int currBlockNum;
        public string pressedKey;
        public Xamarin.Forms.Color fill;
        public float radius;

        public TrialPage_SST(SettingsClass_SST sett)
        {
            InitializeComponent();
            settings = sett;
            SSD = settings.StartSSD;
            outvar.SSD = SSD;

            radius = Convert.ToSingle(((Xamarin.Essentials.DeviceDisplay.MainDisplayInfo.Height) / 2) * 0.8);
            //write initial data to file
            //WriteAnsToFile(settings.File, "Prob_SST task \n Designed by Andras Puszta (andrasp@uio.no) \n \n");

            //WriteAnsToFile(settings.File, String.Concat("\nDate:", DateTime.Now.ToString(), "\n", "Trial number||CueType||S2Type||GoType||Pressed Key||isCorrect||isEarly||RT||SSD\n"));
            BlockRun();
            Screen.PaintSurface += Screen_PaintSurface;
        }

        private void TrialWindow_Closed(object sender, EventArgs e)
        {
            settings.File.Close();
        }
        private async void BlockRun()
        {
            for (int b = 1; b <= settings.BlockNum; b++)
            {
                currTrialNum = 1;
                BlockVar = new BlockVarClass();
                List<TrialList_SST> TrialList_SSTs = new List<TrialList_SST>();
                //High probability of Stop trials
                TrialList_SSTs.Add(new TrialList_SST() { TrialNum = settings.TrialNumGoHighL, TrialType = "Go", TrialType2 = "High", TrialType3 = "Left" }); //GoLeft
                TrialList_SSTs.Add(new TrialList_SST() { TrialNum = settings.TrialNumGoHighR, TrialType = "Go", TrialType2 = "High", TrialType3 = "Right" }); //GoRight
                TrialList_SSTs.Add(new TrialList_SST() { TrialNum = settings.TrialNumStopHighL, TrialType = "Stop", TrialType2 = "High", TrialType3 = "Left" }); //Stop Left
                TrialList_SSTs.Add(new TrialList_SST() { TrialNum = settings.TrialNumStopHighR, TrialType = "Stop", TrialType2 = "High", TrialType3 = "Right" }); //Stop Right
                                                                                                                                                                  //Low probability of Stop trials
                TrialList_SSTs.Add(new TrialList_SST() { TrialNum = settings.TrialNumGoLowL, TrialType = "Go", TrialType2 = "Low", TrialType3 = "Left" }); //GoLeft
                TrialList_SSTs.Add(new TrialList_SST() { TrialNum = settings.TrialNumGoLowR, TrialType = "Go", TrialType2 = "Low", TrialType3 = "Right" });//GoRight
                TrialList_SSTs.Add(new TrialList_SST() { TrialNum = settings.TrialNumStopLowL, TrialType = "Stop", TrialType2 = "Low", TrialType3 = "Left" }); //StopLeft
                TrialList_SSTs.Add(new TrialList_SST() { TrialNum = settings.TrialNumStopLowR, TrialType = "Stop", TrialType2 = "Low", TrialType3 = "Right" }); //StopRight

                int totaltrialNum = 0;
                foreach (TrialList_SST a in TrialList_SSTs)
                {
                    totaltrialNum += a.TrialNum;
                }

                await CurrBlockRun(totaltrialNum, TrialList_SSTs);

                currBlockNum = b;
                Debug.WriteLine(String.Concat("Current block=", b.ToString()));

                if (b == settings.BlockNum)
                {
                    //await ThankYou();
                }
                else
                {
                    await Feedback(BlockVar);
                }



            }


        }


        private async Task Feedback(BlockVarClass blockVar)
        {
            Screen.IsVisible = false;
            Debug.WriteLine("Feedback");

            FeedbackLayout.IsVisible = true;
            Accuracy.Text = String.Concat("Accuracy=", (100 * blockVar.correctTrials / currTrialNum).ToString(), "%");
            Accuracy.TextColor = blockVar.MessageColorAcc;

            RT.Text = String.Concat("Average reaction time=", blockVar.RT.Average().ToString(), "ms");
            RT.TextColor = blockVar.MessageColorRT;

            if (System.Drawing.Color.Equals(blockVar.MessageColorAcc, System.Drawing.Color.Green))//Accuracy good 
            {
                if (System.Drawing.Color.Equals(blockVar.MessageColorRT, System.Drawing.Color.Green))
                {
                    Additional.Text = "Outstanding! You are a Jack Bauer! Keep goin'!";
                    Additional.TextColor = System.Drawing.Color.Green;
                }
                else
                {
                    Additional.Text = "Too slow! Try faster!";
                    Additional.TextColor = System.Drawing.Color.GreenYellow;
                }
            }
            else
            {
                if (System.Drawing.Color.Equals(blockVar.MessageColorRT, System.Drawing.Color.Green))
                {
                    Additional.Text = "Slow down! You are too fast!";
                    Additional.TextColor = System.Drawing.Color.GreenYellow;
                }

                else
                {
                    Additional.Text = "Fail!";
                    Additional.TextColor = System.Drawing.Color.Red;
                }

            }




            //Feedback Time
            await Task.Run(() => System.Threading.Thread.Sleep(settings.FeedbackTime));
            Screen.IsVisible = true;
        }



        private async Task CurrBlockRun(int totaltrialNum, List<TrialList_SST> TrialList_SSTs)
        {
            for (int i = 1; i <= totaltrialNum; i++)
            {
                currTrialNum = i;
                int itiTime = rnd.Next(settings.minITITime, settings.maxITITime);
                await Task.Run(() => System.Threading.Thread.Sleep(itiTime));

                State = "blank";
                Screen.Dispatcher.BeginInvokeOnMainThread(() => Screen.InvalidateSurface());
                FeedbackLayout.IsVisible = false;


                isPressed = false;
                int c = rnd.Next(TrialList_SSTs.Count() - 1);
                string TrialTypeNew = TrialList_SSTs[c].TrialType;
                string TrialType2New = TrialList_SSTs[c].TrialType2;
                //check if this trial is equal to the previous trial
                if (String.Equals(TrialTypeNew, TrialType) & String.Equals(TrialType2New, TrialType2))
                {
                    sameTrials += 1;
                }
                else
                {
                    sameTrials = 0;
                }

                //check if there were 4 same trials consequtively
                if (sameTrials > 4) //break loop
                {
                    i -= 1;
                    break;
                }
                else //do the job
                {
                    TrialType = TrialTypeNew;
                    TrialType2 = TrialType2New;
                    TrialType3 = TrialList_SSTs[c].TrialType3;
                    Debug.WriteLine(String.Concat("Trial...", currTrialNum.ToString(), "..started"));
                    Debug.WriteLine(String.Concat(TrialType, TrialType2, TrialType3, "..started"));
                    isPressed = false;
                    //Cue presentation
                    if (String.Equals(TrialType2, "High"))
                    {
                        State = "High";
                        fill = Xamarin.Forms.Color.Black;
                        Screen.Dispatcher.BeginInvokeOnMainThread(() => Screen.InvalidateSurface());
                    }
                    else
                    {
                        State = "Low";
                        fill = Xamarin.Forms.Color.Gray;
                        Screen.Dispatcher.BeginInvokeOnMainThread(() => Screen.InvalidateSurface());
                    }


                    // Wait 1000 ms after cue presentation
                    int cueTime = rnd.Next(settings.minCueTime, settings.maxCueTime);
                    await Task.Run(() => System.Threading.Thread.Sleep(cueTime));

                    // Run Go
                    if (String.Equals(TrialType, "Go"))
                    {
                        if (String.Equals(TrialType3, "Left"))
                        {

                            stopwatcha.Restart();
                            State = "Left";
                            Debug.WriteLine("GoLeft");
                            canPress = true;
                            Screen.Dispatcher.BeginInvokeOnMainThread(() => Screen.InvalidateSurface());

                        }
                        else
                        {
                            stopwatcha.Restart();
                            State = "Right";
                            Debug.WriteLine("GoRight");
                            canPress = true;
                            Screen.Dispatcher.BeginInvokeOnMainThread(() => Screen.InvalidateSurface());
                        }


                    } // end of Go trials

                    //Or Go and Stop sequentially
                    else
                    {

                        if (String.Equals(TrialType3, "Left"))
                        {

                            stopwatcha.Restart();
                            State = "Left";
                            Debug.WriteLine("GoLeft");
                            canPress = true;
                            Screen.Dispatcher.BeginInvokeOnMainThread(() => Screen.InvalidateSurface());

                        }
                        else
                        {
                            stopwatcha.Restart();
                            State = "Right";
                            Debug.WriteLine("GoRight");
                            canPress = true;
                            Screen.Dispatcher.BeginInvokeOnMainThread(() => Screen.InvalidateSurface());
                        }
                        await Task.Run(() => System.Threading.Thread.Sleep(SSD));
                        State = "Stop";
                        fill = Xamarin.Forms.Color.Red;
                        Screen.Dispatcher.BeginInvokeOnMainThread(() => Screen.InvalidateSurface());

                    } //end of stop trials



                    await Task.Delay(settings.maxWaitingTime, new CancellationToken(isPressed)).ContinueWith(_ =>
                    {
                        if (!isPressed)
                        {
                            if (String.Equals(TrialType, "Stop"))
                            {
                                isCorrect = true;
                            }
                            outvar.RT = 0;
                            Evaluate();
                        }
                        Debug.WriteLine(isCorrect.ToString());

                        //!!!!!uncomment if saving is solved!!!!!
                        //WriteAnsToFile(settings.File, String.Concat(i.ToString(), "||", TrialType2, "||", TrialType, "||", TrialType3, "||", pressedKey.ToString(), "||", isCorrect.ToString(), "||", outvar.isEarly.ToString(), "||", outvar.RT.ToString(), "||", outvar.SSD.ToString(), "\n"));
                        //!!!

                        //Update SSD
                        if (String.Equals(TrialType, "Stop"))
                        {
                            if (isCorrect)
                            {
                                SSD += settings.SSDCorrect;
                                outvar.SSD = SSD;
                            }
                            else
                            {
                                if (SSD > 30)
                                {
                                    SSD -= settings.SSDWrong;
                                    outvar.SSD = SSD;
                                }
                            }

                        }
                    });






                    int numt = TrialList_SSTs[c].TrialNum - 1;
                    if (numt == 0)
                    {
                        TrialList_SSTs.RemoveAt(c);
                    }
                    else
                    {
                        TrialList_SSTs[c].TrialNum = numt;
                    }




                }

            }
        }

        private void Screen_PaintSurface(object sender, SkiaSharp.Views.Forms.SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            float x = info.Width / 2;
            float y = info.Height / 2;
            SKPoint center = new SKPoint(x, y);

            SKRect rect = new SKRect(center.X - radius, center.Y - radius, center.X + radius, center.Y + radius);

            SKPath path = new SKPath();

            switch (State)
            {
                case "Left":
                    //GoLeft
                    path = new SKPath();
                    path.MoveTo(center);
                    path.ArcTo(rect, 90, 180, false);
                    path.Close();

                    canvas.DrawPath(path, new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = SKColors.Green
                    });
                    canvas.DrawPath(path, new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = SKColors.Black
                    });

                    //White half
                    path = new SKPath();
                    path.MoveTo(center);
                    path.ArcTo(rect, 90 + 180, 180, false);
                    path.Close();
                    canvas.DrawPath(path, new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = SKColors.White
                    });
                    canvas.DrawPath(path, new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = SKColors.Black
                    });
                    break;
                case "Right":
                    //GoRight
                    path = new SKPath();
                    path.MoveTo(center);
                    path.ArcTo(rect, 270, 180, false);
                    path.Close();

                    canvas.DrawPath(path, new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = SKColors.Green
                    });
                    canvas.DrawPath(path, new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = SKColors.Black
                    });

                    //White half
                    path = new SKPath();
                    path.MoveTo(center);
                    path.ArcTo(rect, 90, 180, false);
                    path.Close();
                    canvas.DrawPath(path, new SKPaint
                    {
                        Style = SKPaintStyle.Fill,
                        Color = SKColors.White
                    });
                    canvas.DrawPath(path, new SKPaint
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = SKColors.Black
                    });
                    break;
                case "High":
                    //High
                    canvas.DrawCircle(new SKPoint(center.X, center.Y), radius, new SKPaint
                    {
                        Color = SKColors.Black,
                        Style = SKPaintStyle.Fill
                    });
                    break;
                case "Low":
                    //High
                    canvas.DrawCircle(new SKPoint(center.X, center.Y), radius, new SKPaint
                    {
                        Color = SKColors.Gray,
                        Style = SKPaintStyle.Fill
                    });
                    break;
                case "Stop":
                    //High
                    canvas.DrawCircle(center, radius, new SKPaint
                    {
                        Color = SKColors.Red,
                        Style = SKPaintStyle.Fill
                    });
                    break;
                case "blank":
                    canvas.Clear();
                    break;
            }
        }


        public void HalfCircle(SKPoint center, SKCanvas canvas, int v)
        {
            SKRect rect = new SKRect(center.X - radius, center.Y - radius, center.X + radius, center.Y + radius);

            //Green half
            SKPath path = new SKPath();
            path.MoveTo(center);
            path.ArcTo(rect, v, 180, false);
            path.Close();

            canvas.DrawPath(path, new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.Green
            });
            canvas.DrawPath(path, new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Black
            });

            //White half
            path = new SKPath();
            path.MoveTo(center);
            path.ArcTo(rect, v + 180, 180, false);
            path.Close();
            canvas.DrawPath(path, new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = SKColors.White
            });
            canvas.DrawPath(path, new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Black
            });
        }


        private void Evaluate()
        {
            Debug.WriteLine("Evaluate Response");
            State = "blank";
            Screen.Dispatcher.BeginInvokeOnMainThread(() => Screen.InvalidateSurface());


            if (isPressed)
            {
                if (String.Equals(TrialType, "Stop"))
                {
                    isCorrect = false;
                }
                else
                {
                    isCorrect = String.Equals(pressedKey, TrialType3);
                }
            }

            if (!outvar.isLate)
            {

                if (isCorrect)
                {
                    BlockVar.correctTrials += 1;
                    if (outvar.RT != 0)
                    {
                        BlockVar.RT.Add(outvar.RT);
                    }

                }
                else
                {
                    if (outvar.RT != 0)
                    {
                        BlockVar.RT.Add(outvar.RT);
                    }
                }
            }
            else
            {
                if (outvar.RT != 0)
                {
                    BlockVar.RT.Add(outvar.RT);
                }
            }

            currTrialNum += 1;

            //Evaluate Block
            if (BlockVar.RT.Count > 1)
            {
                if (BlockVar.RT.Average() > 800)
                {
                    BlockVar.MessageColorRT = Xamarin.Forms.Color.Red;

                }
                else
                {
                    BlockVar.MessageColorRT = Xamarin.Forms.Color.Green;
                }
                if ((BlockVar.correctTrials / currTrialNum) < 0.6)
                {
                    BlockVar.MessageColorAcc = Xamarin.Forms.Color.Red;
                }
                else
                {
                    BlockVar.MessageColorAcc = Xamarin.Forms.Color.Green;
                }
            }
        }

        //public void WriteAnsToFile(FileStream file, string value)
        //{
        //    byte[] info = new UTF8Encoding(true).GetBytes(value);
        //    file.Write(info, 0, info.Length);
        //}

        private void LeftTapped(object sender, EventArgs e)
        {
            if (canPress)
            {


                isPressed = true;

                outvar.RT = Convert.ToInt32(stopwatcha.ElapsedMilliseconds);
                pressedKey = "Left";


                Evaluate();
                canPress = false;

            }

        }

        private void RightTapped(object sender, EventArgs e)
        {
            if (canPress)
            {


                isPressed = true;


                outvar.RT = Convert.ToInt32(stopwatcha.ElapsedMilliseconds);
                pressedKey = "Right";


                Evaluate();
                canPress = false;
            }

        }


    }
}