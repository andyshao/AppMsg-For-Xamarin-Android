using Android.App;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppMsg
{
    public class ReleaseCallbacksIcs : Java.Lang.Object, Android.App.Application.IActivityLifecycleCallbacks, IReleaseCallbacks
    {
        private WeakReference<Application> mLastApp;

        public void OnActivityCreated(Android.App.Activity activity, Android.OS.Bundle savedInstanceState)
        {

        }

        public void OnActivityDestroyed(Android.App.Activity activity)
        {
            MsgManager.Release(activity);
        }

        public void OnActivityPaused(Android.App.Activity activity)
        {

        }

        public void OnActivityResumed(Android.App.Activity activity)
        {

        }

        public void OnActivitySaveInstanceState(Android.App.Activity activity, Android.OS.Bundle outState)
        {

        }

        public void OnActivityStarted(Android.App.Activity activity)
        {

        }

        public void OnActivityStopped(Android.App.Activity activity)
        {

        }

        public void Register(Android.App.Application application)
        {
            if (mLastApp != null)
            {
                Application app;
                mLastApp.TryGetTarget(out app);
                if (application == app)
                {
                    return;
                }
                else
                {
                    mLastApp = new WeakReference<Application>(application);
                }
                application.RegisterActivityLifecycleCallbacks(this);
            }
        }
    }
}
