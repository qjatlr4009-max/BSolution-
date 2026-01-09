using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSolution_.Teach
{
    public class DiagramEntity
    {
        public InspWindow LinkedWindow { get; set; }
        public Rectangle EntityROI { get; set; }
        public Color EntityColor { get; set; }
        public bool IsHold { get; set; }

        public DiagramEntity()
        {
            LinkedWindow = null;
            EntityROI = new Rectangle(0, 0, 0, 0);
            EntityColor = Color.Red;
            IsHold = false;
        }

        public DiagramEntity(Rectangle rect, Color entityColor, bool hold = false)
        {
            LinkedWindow = null;
            EntityROI = rect;
            EntityColor = entityColor;
            IsHold = hold;
        }
    }
}
