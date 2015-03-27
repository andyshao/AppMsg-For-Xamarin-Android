using Android.Views;
using Android.Views.Animations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppMsg
{
    public class OutAnimationListener : Java.Lang.Object, Animation.IAnimationListener
    {
        private AppMsg appMsg;

        public OutAnimationListener(AppMsg appMsg)
        {
            this.appMsg = appMsg;
        }

        public void OnAnimationEnd(Animation animation)
        {
            View view = appMsg.View;
            if (appMsg.Floating)
            {
                ViewGroup parent = view.Parent as ViewGroup;
                if (parent != null)
                {
                    parent.Post(() =>
                    {
                        parent.RemoveView(view);
                    });
                }
            }
            else
            {
                view.Visibility = ViewStates.Gone;
            }
        }

        public void OnAnimationRepeat(Animation animation)
        {

        }

        public void OnAnimationStart(Animation animation)
        {

        }
    }
}
