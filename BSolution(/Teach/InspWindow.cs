using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BSolution_.Core;
using BSolution_.Algorithm;

namespace BSolution_.Teach
{
    public class InspWindow
    {
        private static readonly Lazy<InspWindowFactory> _instance = new Lazy<InspWindowFactory>

            public static InspWindowFactory Inst
        {
            get { return _instance.Value; }
        }
    }
}
