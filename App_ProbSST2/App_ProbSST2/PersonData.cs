using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_ProbSST2
{
    public class PersonData
    {
        public int age { get; set; }
        public string gender { get; set; }

        public bool isDrug { get; set; }

        public bool isDisease { get; set; }

        public string Drug { get; set; }

        public string Disease { get; set; }

        public List<int> NumBlocks { get; set; }
        public bool IsMore { get; set; }


        public PersonData()
        {
            NumBlocks = new List<int>();
            for(int i = 2; i < 6; i++)
            {
                NumBlocks.Add(i);
            }
        }
        public void Update(int partnumb)
        {
            NumBlocks.RemoveAt(NumBlocks.IndexOf(partnumb));
            
            if (NumBlocks.Count < 0)
            {
                IsMore = false;
            }
            else
            {
                IsMore = true;
            }
        }
    }
}
