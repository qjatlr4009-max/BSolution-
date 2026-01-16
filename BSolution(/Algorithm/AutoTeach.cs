using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSolution_.Algorithm
{
    public class AutoTeach
    
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Score { get; set; }  // 0~100

        public AutoTeach() { }

        public AutoTeach(int x, int y, int w, int h, int score)
        {
            X = x; Y = y; Width = w; Height = h; Score = score;
        }
    }
}
