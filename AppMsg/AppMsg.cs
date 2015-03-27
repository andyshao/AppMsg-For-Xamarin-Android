using Android.App;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppMsg
{
    public class AppMsg : Java.Lang.Object
    {
        public const int LENGTH_SHORT = 3000;
        public const int LENGTH_LONG = 5000;
        public const int LENGTH_STICKY = -1;
        public const int PRIORITY_LOW = int.MinValue;
        public const int PRIORITY_NORMAL = 0;
        public const int PRIORITY_HIGH = int.MaxValue;

        public static Style STYLE_ALERT = new Style(LENGTH_LONG, Resource.Color.alert);
        public static Style STYLE_CONFIRM = new Style(LENGTH_SHORT, Resource.Color.confirm);
        public static Style STYLE_INFO = new Style(LENGTH_SHORT, Resource.Color.info);

        private Activity mActivity;
        private int mDuration = LENGTH_SHORT;
        private View mView;
        private ViewGroup mParent;
        private ViewGroup.LayoutParams mLayoutParams;
        private bool mFloating;
        public Animation mInAnimation, mOutAnimation;
        private int mPriority = PRIORITY_NORMAL;

        public AppMsg(Activity activity)
        {
            mActivity = activity;
        }

        public static AppMsg MakeText(Activity context, String text, Style style)
        {
            return MakeText(context, text, style, Resource.Layout.app_msg);
        }

        public static AppMsg MakeText(Activity context, String text, Style style, View.IOnClickListener clickListener)
        {
            return MakeText(context, text, style, Resource.Layout.app_msg);
        }

        public static AppMsg MakeText(Activity context, String text, Style style, float textSize)
        {
            return MakeText(context, text, style, Resource.Layout.app_msg, textSize);
        }

        public static AppMsg MakeText(Activity context, String text, Style style, float textSize, View.IOnClickListener clickListener)
        {
            return MakeText(context, text, style, Resource.Layout.app_msg, textSize, clickListener);
        }

        public static AppMsg MakeText(Activity context, String text, Style style, int layoutId)
        {
            LayoutInflater inflate = context.LayoutInflater;
            View v = inflate.Inflate(layoutId, null);
            return MakeText(context, text, style, v, true);
        }

        public static AppMsg MakeText(Activity context, String text, Style style, int layoutId, View.IOnClickListener clickListener)
        {
            LayoutInflater inflate = context.LayoutInflater;
            View v = inflate.Inflate(layoutId, null);
            return MakeText(context, text, style, v, true);
        }

        public static AppMsg MakeText(Activity context, String text, Style style, int layoutId, float textSize, View.IOnClickListener clickListener)
        {
            LayoutInflater inflate = context.LayoutInflater;
            View v = inflate.Inflate(layoutId, null);
            return MakeText(context, text, style, v, true, textSize, clickListener);
        }

        public static AppMsg MakeText(Activity context, String text, Style style, int layoutId, float textSize)
        {
            LayoutInflater inflate = context.LayoutInflater;
            View v = inflate.Inflate(layoutId, null);
            return MakeText(context, text, style, v, true, textSize);
        }

        public static AppMsg MakeText(Activity context, String text, Style style, View customViw)
        {
            return MakeText(context, text, style, customViw, false);
        }

        public static AppMsg MakeText(Activity context, String text, Style style, View customeView, View.IOnClickListener clickListener)
        {
            return MakeText(context, text, style, customeView, false, clickListener);
        }

        public static AppMsg MakeText(Activity context, String text, Style style, View view, bool floating)
        {
            return MakeText(context, text, style, view, floating, 0);
        }

        public static AppMsg MakeText(Activity context, String text, Style style, View view, bool floating, View.IOnClickListener clickListener)
        {
            return MakeText(context, text, style, view, floating, 0, clickListener);
        }

        private static AppMsg MakeText(Activity context, String text, Style style, View view, bool floating, float textSize)
        {
            AppMsg result = new AppMsg(context);
            view.SetBackgroundResource(style.Background);

            TextView tv = view.FindViewById<TextView>(Android.Resource.Id.Message);
            if (textSize > 0)
            {
                tv.SetTextSize(Android.Util.ComplexUnitType.Sp, textSize);
            }
            tv.Text = text;

            result.mView = view;
            result.mDuration = style.Duration;
            result.mFloating = floating;

            return result;
        }

        private static AppMsg MakeText(Activity context, String text, Style style, View view, bool floating, float textSize, View.IOnClickListener clickListener)
        {
            AppMsg result = new AppMsg(context);

            view.SetBackgroundResource(style.Background);
            view.Clickable = true;

            TextView tv = view.FindViewById<TextView>(Android.Resource.Id.Message);
            if (textSize > 0)
            {
                tv.SetTextSize(Android.Util.ComplexUnitType.Sp, textSize);
            }
            tv.Text = text;

            result.mView = view;
            result.mDuration = style.Duration;
            result.mFloating = floating;

            view.SetOnClickListener(clickListener);
            return result;
        }

        public static AppMsg MakeText(Activity context, int resId, Style style, View customView, bool floating)
        {
            return MakeText(context, context.Resources.GetText(resId), style, customView, floating);
        }

        public static AppMsg MakeText(Activity context, int resId, Style style)
        {
            return MakeText(context, context.Resources.GetText(resId), style);
        }

        public static AppMsg MakeText(Activity context, int resId, Style style, int layoutId)
        {
            return MakeText(context, context.Resources.GetText(resId), style, layoutId);
        }

        public void Show()
        {
            MsgManager manager = MsgManager.ObTain(mActivity);
            manager.Add(this);
        }

        public bool IsShowing
        {
            get
            {
                if (mFloating)
                {
                    return mView != null && mView.Parent != null;
                }
                else
                {
                    return mView.Visibility == ViewStates.Visible;
                }
            }
        }

        public void Dismiss()
        {
            MsgManager.ObTain(mActivity).RemoveMsg(this);
        }

        public void Cancel()
        {
            MsgManager.ObTain(mActivity).ClearMsg(this);
        }

        public static void CancelAll()
        {
            MsgManager.ClearAll();
        }

        public static void CancelAll(Activity activity)
        {
            MsgManager.Release(activity);
        }

        public Activity Activity
        {
            get
            {
                return mActivity;
            }
        }

        public View View
        {
            get
            {
                return mView;
            }
            set
            {
                mView = value;
            }
        }

        public int Duration
        {
            get
            {
                return mDuration;
            }
            set
            {
                mDuration = value;
            }
        }

        public void SetText(int resId)
        {
            SetText(mActivity.GetText(resId));
        }

        public void SetText(String s)
        {
            if (mView == null)
            {
                throw new ArgumentNullException("mView");
            }
            TextView tv = mView.FindViewById<TextView>(Android.Resource.Id.Message);
            if (tv == null)
            {
                throw new ArgumentNullException("tv");
            }
            tv.Text = s;
        }

        public ViewGroup.LayoutParams LayoutParams
        {
            get
            {
                if (mLayoutParams == null)
                {
                    mLayoutParams = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent);
                }
                return mLayoutParams;
            }
            set
            {
                mLayoutParams = value;
            }
        }

        public AppMsg SetLayoutGravity(GravityFlags gravity)
        {
            mLayoutParams = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, gravity);
            return this;
        }

        public bool Floating
        {
            get
            {
                return mFloating;
            }
            set
            {
                mFloating = value;
            }
        }

        public AppMsg SetAnimation(int inAnimation, int outAnimation)
        {
            return SetAnimation(AnimationUtils.LoadAnimation(mActivity, inAnimation),
                AnimationUtils.LoadAnimation(mActivity, outAnimation));
        }

        public AppMsg SetAnimation(Animation inAnimation, Animation outAnimation)
        {
            mInAnimation = inAnimation;
            mOutAnimation = outAnimation;
            return this;
        }

        public int Priority
        {
            get
            {
                return mPriority;
            }
            set
            {
                mPriority = value;
            }
        }

        public ViewGroup Parent
        {
            get
            {
                return mParent;
            }
            set
            {
                mParent = value;
            }
        }
    }
}
