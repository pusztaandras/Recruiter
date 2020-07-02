using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Xamarin.Forms;
using System.Reflection;

namespace App_ProbSST2
{
    public class SettingsClass_WM
    {


        public bool IsMore { get; set; }
        public List<string> CueNames { get; set; }

        public List<string> AnsImageNames { get; set; }

        public Dictionary<string, ImageSource> ImageNames { get; set; }

        public int Learningitinum { get; set; }

        public int Testitinum { get; set; }

        public FileStream FileName { get; set; }

        public string gender { get; set; }

        public int age { get; set; }

        public string Drug { get; set; }

        public bool isDrug { get; set; }

        public string Disorder { get; set; }

        public bool isDisorder { get; set; }

        public int CurrPart { get; set; }

        public SettingsClass_WM(int partnumb)
        {

            CurrPart = partnumb;
            Learningitinum = 2;
            Testitinum = 4;

            CueNames = new List<string>();
            CueNames.Add("X1"); //phase 1
            CueNames.Add("Y1"); //phase 2
            CueNames.Add("X2"); //phase 3
            CueNames.Add("Y2"); //phase 4
            CueNames.Add("X3"); //phase 5
            CueNames.Add("Y3"); //phase 6
            CueNames.Add("X1"); //phase 7
            CueNames.Add("Y1"); //phase 8

            AnsImageNames = new List<string>();
            AnsImageNames.Add("A1");
            AnsImageNames.Add("B1");
            AnsImageNames.Add("A1");
            AnsImageNames.Add("B1");
            AnsImageNames.Add("A1");
            AnsImageNames.Add("B1");
            AnsImageNames.Add("A2");
            AnsImageNames.Add("B2");

            //for test/gen
            CueNames.Add("X2"); //phase 9
            CueNames.Add("Y2"); //phase 10
            CueNames.Add("X3"); //phase 11
            CueNames.Add("Y3"); //phase 12
            CueNames.Add("XG1"); //phase 13
            CueNames.Add("YG1"); //phase 14
            CueNames.Add("XG2"); //phase 15
            CueNames.Add("YG2"); //phase 16

            AnsImageNames.Add("A2");
            AnsImageNames.Add("B2");
            AnsImageNames.Add("A2");
            AnsImageNames.Add("B2");
            AnsImageNames.Add("A2");
            AnsImageNames.Add("B2");
            AnsImageNames.Add("A2");
            AnsImageNames.Add("B2");

            ImageSource graue = ImageSource.FromResource("App_ProbSST2.Pictures.WM.graue.jpg");


            Random rnd = new Random();


            
            
            string[] temp = new string[]
                        {
                String.Concat("App_ProbSST2.Pictures.WM.", partnumb.ToString(), "_1.jpg"),
                String.Concat("App_ProbSST2.Pictures.WM.", partnumb.ToString(), "_2.jpg"),
                String.Concat("App_ProbSST2.Pictures.WM.", partnumb.ToString(), "_3.jpg"),
                String.Concat("App_ProbSST2.Pictures.WM.", partnumb.ToString(), "_4.jpg"),
                String.Concat("App_ProbSST2.Pictures.WM.", partnumb.ToString(), "_5.jpg"),
                String.Concat("App_ProbSST2.Pictures.WM.", partnumb.ToString(), "_6.jpg"),
                        };
            List<string> x = new List<string>(temp);
            List<int> y = new List<int>();
            for (int w = 0; w < 6; w++)
            {
                y.Add(w);
            }
            int[] i1 = new int[] { 1, 2, 3 };
            int[] i2 = new int[] { 1, 3, 4 };
            int[] i3 = new int[] { 1, 2, 4 };
            int[] i4 = new int[] { 1, 3, 5 };
            int[] i5 = new int[] { 1, 2, 5 };
            int[] i6 = new int[] { 1, 2, 6 };
            int[] i7 = new int[] { 1, 3, 6 };
            int[] i8 = new int[] { 1, 4, 5 };
            int[] i9 = new int[] { 1, 4, 6 };
            int[] i10 = new int[] { 1, 5, 6 };
            List<Array> h = new List<Array>();
            h.Add(i1);
            h.Add(i2);
            h.Add(i3);
            h.Add(i4);
            h.Add(i5);
            h.Add(i6);
            h.Add(i7);
            h.Add(i8);
            h.Add(i9);
            h.Add(i10);


            int u = rnd.Next(partnumb);
            int[] ChoosenArray = h[u] as int[];
            List<int> gf = new List<int>(ChoosenArray);
            List<int> ImagePairs = gf.OrderByDescending(o => o).ToList();

            List<ImageSource> PictureNameX = new List<ImageSource>();
            List<ImageSource> PictureNameY = new List<ImageSource>();
            List<ImageSource> PictureNameA = new List<ImageSource>();
            List<ImageSource> PictureNameB = new List<ImageSource>();

            for (int f = 0; f < 3; f++)
            {
                var xxx = x[ImagePairs[f] - 1];
                ImageSource kep = ImageSource.FromResource(x[ImagePairs[f] - 1]);
                PictureNameX.Add(kep);
                x.Remove(x[ImagePairs[f] - 1]);
                y.Remove(y[ImagePairs[f] - 1]);
            }



            for (int a = 0; a < 3; a++)
            {
                ImageSource kep = ImageSource.FromResource(x[a]);
                PictureNameY.Add(kep);
            }

            //Generalization sounds based on the categorization.
            ImageSource genX1 = ImageSource.FromResource(String.Concat("App_ProbSST2.Pictures.WM.gen_", partnumb.ToString(), "_", ImagePairs[0], ".jpg"));
            PictureNameX.Add(genX1);

            ImageSource genX2 = ImageSource.FromResource(String.Concat("App_ProbSST2.Pictures.WM.gen_", partnumb.ToString(), "_", ImagePairs[1], ".jpg"));

            PictureNameX.Add(genX2);

            ImageSource genY1 = ImageSource.FromResource(String.Concat("App_ProbSST2.Pictures.WM.gen_", partnumb.ToString(), "_", y[0], ".jpg"));


            PictureNameY.Add(genY1);


            ImageSource genY2 = ImageSource.FromResource(String.Concat("App_ProbSST2.Pictures.WM.gen_", partnumb.ToString(), "_", y[1], ".jpg"));
            PictureNameY.Add(genY2);
            //

            ImageSource kep2 = ImageSource.FromResource("App_ProbSST2.Pictures.WM.apple.jpg");

            if (rnd.Next(2) == 0)
            {
                PictureNameA.Add(kep2);
            }
            else
            {
                PictureNameB.Add(kep2);
            }


            kep2 = ImageSource.FromResource("App_ProbSST2.Pictures.WM.pear.jpg");

            if (rnd.Next(2) == 0)
            {
                PictureNameA.Add(kep2);
            }
            else
            {
                PictureNameB.Add(kep2);
            }


            kep2 = ImageSource.FromResource("App_ProbSST2.Pictures.WM.grape.jpg");

            if (PictureNameB.Count == 2)
            {
                PictureNameA.Add(kep2);

            }
            else if (PictureNameB.Count == 1)
            {
                if (rnd.Next(2) == 0)
                {
                    PictureNameA.Add(kep2);
                }
                else
                {
                    PictureNameB.Add(kep2);
                }
            }
            else
            {
                PictureNameB.Add(kep2);
            }


            kep2 = ImageSource.FromResource("App_ProbSST2.Pictures.WM.banana.jpg");

            if (PictureNameB.Count == 2)
            {
                PictureNameA.Add(kep2);

            }
            else
            {
                PictureNameB.Add(kep2);
            }

            ImageSource PictureNameGood =  ImageSource.FromResource("App_ProbSST2.Pictures.WM.Yes.bmp");

            ImageSource PictureNameBad = ImageSource.FromResource("App_ProbSST2.Pictures.WM.No.jpg");


            ImageSource PictureNameEnd = ImageSource.FromResource(String.Concat("App_ProbSST2.Pictures.WM.vege_", partnumb.ToString(), ".jpg"));


            ImageNames = new Dictionary<string, ImageSource>();
            ImageNames.Add("X1", PictureNameX[0]);
            ImageNames.Add("X2", PictureNameX[1]);
            ImageNames.Add("X3", PictureNameX[2]);
            ImageNames.Add("Y1", PictureNameY[0]);
            ImageNames.Add("Y2", PictureNameY[1]);
            ImageNames.Add("Y3", PictureNameY[2]);
            ImageNames.Add("XG1", PictureNameX[3]);
            ImageNames.Add("XG2", PictureNameX[4]);
            ImageNames.Add("YG1", PictureNameY[3]);
            ImageNames.Add("YG2", PictureNameY[4]);


            ImageNames.Add("A1", PictureNameA[0]);
            ImageNames.Add("A2", PictureNameA[1]);
            ImageNames.Add("B1", PictureNameB[0]);
            ImageNames.Add("B2", PictureNameB[1]);

            ImageNames.Add("Good", PictureNameGood);
            ImageNames.Add("Bad", PictureNameBad);

            ImageNames.Add("End", PictureNameEnd);


            var asa = String.Concat(DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Day.ToString(), DateTime.Now.Hour.ToString(), DateTime.Now.Minute.ToString(), "_Part_", CurrPart.ToString(), ".txt");
            FileName = File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), asa));


        }


    }
}
