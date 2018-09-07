using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using BLE_Tracker.Points;
using Android.Graphics;
using Java.Lang;
using System.Timers;
using BLE_Tracker.Activities;

namespace BLE_Tracker.Calc
{
    public class Tracker : Thread
    {
        private float diapazone = 50;
        private SurfaceView view;
        private Context context;
        private Timer timer;
        private float scale;

        private int xAxis;
        private int yAxis;

        private ISharedPreferences settings;

        public Tracker(SurfaceView view, Context context)
        {
            settings = context.GetSharedPreferences(TrackerSettings.SettingsName, FileCreationMode.WorldReadable);
            diapazone = settings.GetFloat(TrackerSettings.ScaleSize, float.MaxValue);
            this.view = view;

            scale = view.Height / diapazone;
            xAxis = view.Height / 2;
            yAxis = view.Width / 2;

            timer = new Timer
            {
                Enabled = true,
                Interval = 100
            };

            timer.Elapsed += (s, e) =>
            {
                DrawTrack();
            };
        }
       

        public override void Run()
        {
            base.Run();

            timer.Start();
        }

        private void ClearCanvas(Canvas canvas)
        {
            try
            {
                canvas = view.Holder.LockCanvas();

                lock (canvas)
                {
                    canvas.DrawColor(Color.Black);

                    //Paint clearPaint = new Paint();
                    //clearPaint.SetXfermode(new PorterDuffXfermode(PorterDuff.Mode.Clear));
                    //canvas.DrawRect(0, 0, view.Width, view.Height, clearPaint);
                }
            }
            catch { }
            finally
            {
                if (canvas != null)
                {
                    view.Holder.UnlockCanvasAndPost(canvas);
                }
            }
        }


        private void DrawDiapazone(Canvas canvas)
        {
            lock(canvas)
            {
                Paint paint = new Paint { AntiAlias = true, Color = Color.Green };
                paint.TextSize = 80;
                paint.SetStyle(Paint.Style.Fill);

                canvas.DrawText("Scale (m): " + diapazone.ToString(), 50, 150, paint);

            }
        }

        private void DrawPointInfo(Canvas canvas)
        {
            if(PointCollection.Points.Count > 0)
            {
                Points.Point point = PointCollection.Points[PointCollection.Points.Count - 1];
                Paint paint = new Paint { AntiAlias = true, Color = Color.Aqua, Alpha = point.Alfa };
                paint.TextSize = 50;
                paint.SetStyle(Paint.Style.Fill);

                canvas.DrawText("Point X: " + point.X + " Y: " + point.Y, view.Width * 3 /4, 100, paint);
            }

            
        }

        private void DrawAxisItem(Canvas canvas, string text, float x, float y, int textSize, Color color)
        {
            Paint paint = new Paint { AntiAlias = true, Color = color};
            paint.TextSize = textSize;
            paint.SetStyle(Paint.Style.Fill);
            canvas.DrawText(text, x, y, paint);
        }

        private void ShowBeaconsInfo(Canvas canvas)
        {
            var point = PointCollection.Points[PointCollection.Points.Count - 1];
            string text = "";
            foreach(var b in TraectoryActivity.Beacons)
            {
                text += b.Name + " rssi: " + b.Rssi[0].ToString() + "; ";
            }
            Paint paint = new Paint { AntiAlias = true, Color = Color.Green, Alpha = point.Alfa };
            paint.TextSize = 70;
            paint.SetStyle(Paint.Style.Fill);
            canvas.DrawText(text, 100, view.Height - 100, paint);
        }

        private void DrawCoordinates(Canvas canvas)
        {
            lock(canvas)
            {
                Paint mPaint = new Paint();
                mPaint.Color = Color.Yellow;
                //рисуем оси
                canvas.DrawLine(yAxis, 0, yAxis, view.Height, mPaint);
                DrawAxisItem(canvas, "X", view.Width - 70, xAxis + 50 + 10, 50, Color.Pink);
                canvas.DrawLine(0, xAxis , view.Width, xAxis, mPaint);
                DrawAxisItem(canvas, "Y", yAxis + 10, 50 + 10, 50, Color.Pink);
                DrawAxisItem(canvas, "0", yAxis + 10, xAxis - 10, 50, Color.Pink);

                //шкалы над осью Х
                mPaint = new Paint();
                mPaint.Color = Color.White;
                int i = 1;
                while ((i * scale * 5) < xAxis)
                {
                    canvas.DrawLine(0, xAxis - i * scale * 5, 15, xAxis - i * scale * 5, mPaint);

                    DrawAxisItem(canvas, (i * 5).ToString(), 20, xAxis - i * scale * 5 + 40 / 2, 40, Color.Yellow);
                    i++;
                }

                //// шкалы под осью X
                i = 1;
                while((i * scale * 5)<view.Height)
                {
                    canvas.DrawLine(0, xAxis + i * scale * 5, 15, xAxis + i * scale * 5, mPaint);
                    DrawAxisItem(canvas, "-" + (i*5).ToString(), 20, xAxis + i * scale * 5 + 40 / 2, 40, Color.Yellow);
                    i++;
                }
            }

           
        }

        private void DrawTrack()
        {
            Canvas canvas = null;

            try
            {
                canvas = view.Holder.LockCanvas();
                canvas.DrawColor(Color.Black);
                DrawCoordinates(canvas);
                //DrawDiapazone(canvas);
                DrawPointInfo(canvas);
                ShowBeaconsInfo(canvas);
                lock (canvas)
                {
                    foreach (var point in PointCollection.Points)
                    {
                        float x = (float)(yAxis + point.X * scale);
                        float y = (float)(xAxis - point.Y * scale);

                        Paint mPaint = new Paint();
                        mPaint.SetStyle(Paint.Style.Fill);
                        mPaint.Color = Color.Red;
                        mPaint.Alpha = point.Alfa;
                        canvas.DrawCircle(x, y, 20, mPaint);

                        
                    }
                }
            }
            catch { }
            finally
            {
                if (canvas != null)
                {
                    view.Holder.UnlockCanvasAndPost(canvas);
                }
            }



            
        }
    }
}