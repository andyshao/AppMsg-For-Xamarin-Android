using System;
using System.Collections.Generic;
using System.Text;

namespace AppMsg
{
    public class Style
    {
        private int duration;
        private int backgrond;

        public Style(int duration, int resId)
        {
            this.duration = duration;
            this.backgrond = resId;
        }

        public int Duration
        {
            get
            {
                return this.duration;
            }
        }

        public int Background
        {
            get
            {
                return this.backgrond;
            }
        }

        public override int GetHashCode()
        {
            int hash = duration.GetHashCode();
            hash ^= backgrond.GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            if (obj is Style)
            {
                Style style = obj as Style;
                return style.Duration == this.duration && style.Background == this.backgrond;
            }
            return false;
        }
    }
}
