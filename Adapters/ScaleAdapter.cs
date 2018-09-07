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

namespace BLE_Tracker.Adapters
{
    class ScaleAdapter : BaseAdapter<double>
    {
        Context context;
        double[] values;
        public ScaleAdapter(Context context, double[] values)
        {
            this.context = context;
        }


        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //var view = convertView;
            //ScaleAdapterViewHolder holder = null;

            //if (view != null)
            //    holder = view.Tag as ScaleAdapterViewHolder;

            //if (holder == null)
            //{

            //    holder = new ScaleAdapterViewHolder();

            //    //replace with your item and your holder items
            //    //comment back in
            //    //view = inflater.Inflate(Resource.Layout.item, parent, false);
            //    //holder.Title = view.FindViewById<TextView>(Resource.Id.text);
            //    view.Tag = holder;
            //}


            //fill in your items
            //holder.Title.Text = "new text here";

            var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
            var view = inflater.Inflate(Resource.Layout.ScaleItem, parent, false);
            TextView value = view.FindViewById<TextView>(Resource.Id.textView_value);
            value.Text = values[position].ToString();

            return view;
        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return 0;
            }
        }

        public override double this[int position] => values[position];
    }

    class ScaleAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}