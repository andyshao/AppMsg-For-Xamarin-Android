using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using AppMsg;

namespace Sample
{
    [Activity(Label = "Sample", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;
        AppMsg.AppMsg appmsg;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            button.Click += delegate { 
                button.Text = string.Format("{0} clicks!", count++);
                if (appmsg != null)
                {
                    appmsg.Dismiss();
                }
                if (appmsg == null)
                {
                    appmsg = AppMsg.AppMsg.MakeText(this, "测试", new Style(AppMsg.AppMsg.LENGTH_STICKY, Resource.Color.alert));
                    appmsg.Show();
                }
            };
        }
    }
}

