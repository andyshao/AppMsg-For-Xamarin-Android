using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using AMsg = AppMsg.AppMsg;

namespace Sample
{
    [Activity(Label = "AppMsg示例", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        AppMsg.AppMsg appmsg;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            FindViewById<Button>(Resource.Id.btnAlert).Click += (e, s) =>
            {
                AMsg.MakeText(this, "Alert 测试", AMsg.STYLE_ALERT).Show();
            };

            FindViewById<Button>(Resource.Id.btnConfirm).Click += (e, s) =>
            {
                AMsg.MakeText(this, "Confirm 测试", AMsg.STYLE_CONFIRM).Show();
            };

            FindViewById<Button>(Resource.Id.btnInfo).Click += (e, s) =>
            {
                AMsg.MakeText(this, "Info 测试", AMsg.STYLE_INFO).Show();
            };

            FindViewById<Button>(Resource.Id.btnOpen).Click += (e, s) =>
            {
                if (appmsg == null)
                {
                    appmsg = AMsg.MakeText(this, "不会自动关闭的提示", new AppMsg.Style(AMsg.LENGTH_STICKY, Resource.Color.info));
                }
                if (!appmsg.IsShowing)
                {
                    appmsg.Show();
                }
            };

            FindViewById<Button>(Resource.Id.btnClose).Click += (e, s) =>
            {
                if (appmsg != null)
                {
                    appmsg.Dismiss();
                }
            };
        }
    }
}

