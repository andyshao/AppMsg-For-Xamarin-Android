using Android.App;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppMsg
{
    public class MsgManager : Handler
    {
        private const int MESSAGE_DISPLAY = 0xc2007;
        private const int MESSAGE_ADD_VIEW = 0xc2008;
        private const int MESSAGE_REMOVE = 0xc2009;

        private static IDictionary<Activity, MsgManager> sManagers;
        private static IReleaseCallbacks sReleaseCallbacks;

        private Queue<AppMsg> msgQueue;
        private Queue<AppMsg> stickyQueue;

        private MsgManager()
        {
            msgQueue = new Queue<AppMsg>();
            stickyQueue = new Queue<AppMsg>();
        }

        public static MsgManager ObTain(Activity activity)
        {
            if (sManagers == null)
            {
                sManagers = new Dictionary<Activity, MsgManager>();
            }
            MsgManager manager = null;
            if (sManagers.ContainsKey(activity))
            {
                manager = sManagers[activity];
            }
            if (manager == null)
            {
                manager = new MsgManager();
                EnsureReleaseOnDestroy(activity);
                sManagers.Add(activity, manager);
            }
            return manager;
        }

        public static void EnsureReleaseOnDestroy(Activity activity)
        {
            if (Android.OS.Build.VERSION.SdkInt < BuildVersionCodes.IceCreamSandwich)
            {
                return;
            }
            if (sReleaseCallbacks == null)
            {
                sReleaseCallbacks = new ReleaseCallbacksIcs();
            }
            sReleaseCallbacks.Register(activity.Application);
        }

        public static void Release(Activity activity)
        {
            if (sManagers != null)
            {
                MsgManager manager = sManagers[activity];
                sManagers.Remove(activity);
                if (manager != null)
                {
                    manager.ClearAllMsg();
                }
            }
        }

        public static void ClearAll()
        {
            if (sManagers != null)
            {
                foreach(MsgManager manager in sManagers.Values)
                {
                    manager.ClearAllMsg();
                }
                sManagers.Clear();
            }
        }

        public void Add(AppMsg appMsg)
        {
            msgQueue.Enqueue(appMsg);
            if (appMsg.mInAnimation == null)
            {
                appMsg.mInAnimation = AnimationUtils.LoadAnimation(appMsg.Activity, Android.Resource.Animation.FadeIn);
            }
            if (appMsg.mOutAnimation == null)
            {
                appMsg.mOutAnimation = AnimationUtils.LoadAnimation(appMsg.Activity, Android.Resource.Animation.FadeOut);
            }
            DisplayMsg();
        }

        public void ClearMsg(AppMsg appMsg)
        {
            if (msgQueue.Contains(appMsg) || stickyQueue.Contains(appMsg))
            {
                RemoveMessages(MESSAGE_DISPLAY, appMsg);
                RemoveMessages(MESSAGE_ADD_VIEW, appMsg);
                RemoveMessages(MESSAGE_REMOVE, appMsg);
                Queue<AppMsg> save = new Queue<AppMsg>();
                int count = msgQueue.Count;
                for (int i = 0; i < count; i++)
                {
                    AppMsg msg = msgQueue.Dequeue();
                    if (msg == appMsg)
                    {
                        continue;
                    }
                    save.Enqueue(msg);
                }
                msgQueue = save;
                save = new Queue<AppMsg>();

                count = stickyQueue.Count;
                for (int i = 0; i < count; i++)
                {
                    AppMsg msg = stickyQueue.Dequeue();
                    if (msg == appMsg)
                    {
                        continue;
                    }
                    save.Enqueue(msg);
                }
                stickyQueue = save;
            }
        }

        public void ClearAllMsg()
        {
            RemoveMessages(MESSAGE_DISPLAY);
            RemoveMessages(MESSAGE_ADD_VIEW);
            RemoveMessages(MESSAGE_REMOVE);
            msgQueue.Clear();
            stickyQueue.Clear();
        }

        public void ClearShowing()
        {
            Queue<AppMsg> showing = new Queue<AppMsg>();
            ObTainShowing(msgQueue, showing);
            ObTainShowing(stickyQueue, showing);
            foreach (AppMsg msg in showing)
            {
                ClearMsg(msg);
            }
        }

        public static void ObTainShowing(Queue<AppMsg> from, Queue<AppMsg> appendTo)
        {
            foreach (AppMsg msg in from)
            {
                if (msg.IsShowing)
                {
                    appendTo.Enqueue(msg);
                }
            }
        }

        private void DisplayMsg()
        {
            if (msgQueue.Count <= 0)
            {
                return;
            }
            AppMsg appMsg = msgQueue.Peek();
            Message msg;
            if (!appMsg.IsShowing)
            {
                msg = ObtainMessage(MESSAGE_ADD_VIEW);
                msg.Obj = appMsg;
                SendMessage(msg);
            }
            else if (appMsg.Duration != AppMsg.LENGTH_STICKY)
            {
                msg = ObtainMessage(MESSAGE_DISPLAY);
                SendMessageDelayed(msg, appMsg.Duration + appMsg.mInAnimation.Duration + appMsg.mOutAnimation.Duration);
            }
        }

        public void RemoveMsg(AppMsg appMsg)
        {
            ClearMsg(appMsg);
            View view = appMsg.View;
            ViewGroup parent = (ViewGroup)view.Parent;
            if (parent != null)
            {
                appMsg.mOutAnimation.SetAnimationListener(new OutAnimationListener(appMsg));
                view.ClearAnimation();
                view.StartAnimation(appMsg.mOutAnimation);
            }
            Message msg = ObtainMessage(MESSAGE_DISPLAY);
            SendMessage(msg);
        }

        private void AddMsgToView(AppMsg appMsg)
        {
            View view = appMsg.View;
            if (view.Parent == null)
            {
                ViewGroup targetParent = appMsg.Parent;
                ViewGroup.LayoutParams param = appMsg.LayoutParams;
                if(targetParent != null)
                {
                    targetParent.AddView(view,param);
                }
                else
                {
                    appMsg.Activity.AddContentView(view,param);
                }
            }
            view.ClearAnimation();
            view.StartAnimation(appMsg.mInAnimation);
            if (view.Visibility != ViewStates.Visible)
            {
                view.Visibility = ViewStates.Visible;
            }
            int duration = appMsg.Duration;
            if (duration != AppMsg.LENGTH_STICKY)
            {
                Message msg = ObtainMessage(MESSAGE_REMOVE);
                msg.Obj = appMsg;
                SendMessageDelayed(msg, duration);
            }
            else
            {
                stickyQueue.Enqueue(msgQueue.Dequeue());
            }
        }

        public override void HandleMessage(Message msg)
        {
            AppMsg appMsg;
            switch (msg.What)
            {
                case MESSAGE_DISPLAY:
                    {
                        DisplayMsg();
                    }
                    break;
                case MESSAGE_ADD_VIEW:
                    {
                        appMsg = msg.Obj as AppMsg;
                        AddMsgToView(appMsg);
                    }
                    break;
                case MESSAGE_REMOVE:
                    {
                        appMsg = msg.Obj as AppMsg;
                        RemoveMsg(appMsg);
                    }
                    break;
                default:
                    {
                        base.HandleMessage(msg);
                    }
                    break;
            }
        }


    }
}
