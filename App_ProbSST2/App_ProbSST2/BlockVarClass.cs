using System.Collections.Generic;
using System.Drawing;


namespace App_ProbSST2
{
    public class BlockVarClass
    {
        public List<int> RT { get; set; }

        public int correctTrials { get; set; }

        public Color MessageColorRT { get; set; }

        public Color MessageColorAcc { get; set; }

        public BlockVarClass()
        {
            RT = new List<int>();
            correctTrials = 0;

        }
    }
}
