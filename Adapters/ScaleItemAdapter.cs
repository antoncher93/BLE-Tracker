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
    public class ScaleItemAdapter : ArrayAdapter
    {
        private IList<float> values;

        public ScaleItemAdapter(Context context, int resource, float[] objects) 
            : base(context, resource, objects)
        {
            values = objects;
        }

        public float this[int position] => values[position];

        public override View GetDropDownView(int position, View convertView, ViewGroup parent)
        {
            View item = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ScaleItem, parent, false);
            TextView value = item.FindViewById<TextView>(Resource.Id.textView_value);
            value.Text = values[position].ToString();
            return item;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View item = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ScaleItem, parent, false);
            TextView value = item.FindViewById<TextView>(Resource.Id.textView_value);
            value.Text = values[position].ToString();
            return item;
            //return base.GetView(position, convertView, parent);
        }

        public View GetCustomView(int position, View convertView, ViewGroup parent)
        {
            View item = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.ScaleItem, parent, false);
            TextView value = item.FindViewById<TextView>(Resource.Id.textView_value);
            value.Text = values[position].ToString();
            return item;
        }



    }
}