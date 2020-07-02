using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_ProbSST2
{
    public class OutputVar
    {
        public int RT { get; set; }
        public bool isCorrect { get; set; }
        public bool isPressed { get; set; }
        public int SSD { get; set; }

        public bool isEarly { get; set; }

        public bool isLate { get; set; }

        public string PressedKey { get; set; }
    }
}
