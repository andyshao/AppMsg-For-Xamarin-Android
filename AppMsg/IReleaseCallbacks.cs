using Android.App;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppMsg
{
    public interface IReleaseCallbacks
    {
        void Register(Application application);
    }
}
