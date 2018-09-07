using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using BLE_Tracker.Calc;

namespace BLE_Tracker.Views
{
    public class TrackView : SurfaceView, ISurfaceHolderCallback
    {
        public Canvas Canvas { get; private set; }
        private Tracker tracker;
        private Context context;

        public TrackView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize();
        }

        public TrackView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize();
        }

        public TrackView(Context context) : base(context)
        {
            Initialize();
            Holder.AddCallback(this);
            this.context = context;

        }

        public void SurfaceChanged(ISurfaceHolder holder, [GeneratedEnum] Format format, int width, int height)
        {
            
        }

        public void SurfaceCreated(ISurfaceHolder holder)
        {
            tracker = new Tracker(this, context);
            tracker.Start();
        }

        public void SurfaceDestroyed(ISurfaceHolder holder)
        {
            bool retry = true;

            while (retry)
            {
                try
                {
                    tracker.Join();
                    retry = false;
                }
                catch { }
            }
        }

        private void Initialize()
        {
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            Canvas = canvas;


            Paint mPaint = new Paint();
            mPaint.SetStyle(Paint.Style.Fill);
            mPaint.Color = Color.White;
            canvas.DrawPaint(mPaint);

            mPaint = new Paint();
            mPaint.Color = Color.Blue;
            canvas.DrawLine(Width / 2, 0, Width / 2, Height, mPaint);

            mPaint = new Paint();
            mPaint.Color = Color.Blue;
            canvas.DrawLine(0, Height / 2, Width, Height / 2, mPaint);
        }
    }
}