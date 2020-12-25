using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections;

namespace Lab1_2
{
    class V1DataOnGrid : V1Data, IEnumerable<DataItem>                  /*значения поля на равномерной сетке, которые хранятся в массиве*/
    {
        public Grid grid { set; get; }
        public Vector3[] points_value { set; get; }

        public V1DataOnGrid(string new_data, DateTime new_date, Grid new_grid) : base(new_data, new_date)
        {
            grid = new_grid;
            points_value = new Vector3[grid.number_of_grid_points];
            InitRandom(0, 20);
        }

        public override float[] NearZero(float eps)
        {
            float t;
            List<float> time = new List<float>();
            for (int i = 0; i < points_value.Length; i++)
            {
                if (points_value[i].Length() < eps)
                {
                    t = grid.t;
                    for (int j = 0; j < i; j++)
                    {
                        t += grid.time_step;
                    }
                    time.Add(t);
                    /*for (int j = 0; j < grid.number_of_grid_points; j++)
                    {
                        time.Add(grid.t + j * grid.time_step);
                    }*/
                }
            }
            return time.ToArray();
        }

        public void InitRandom(float minValue, float maxValue)
        {
            Random rnd = new Random();
            for (int i = 0; i < points_value.Length; i++)
            {
                points_value[i] = new Vector3(rnd.Next(Convert.ToInt32(minValue), Convert.ToInt32(maxValue)), rnd.Next(Convert.ToInt32(minValue), Convert.ToInt32(maxValue)), rnd.Next(Convert.ToInt32(minValue), Convert.ToInt32(maxValue)));
            }
        }

        public static implicit operator V1DataCollection(V1DataOnGrid value)
        {
            V1DataCollection buf = new V1DataCollection(value.data, value.date);
            buf.value.Clear();
            for (int i = 0; i < value.points_value.Length; i++)
            {
                buf.value.Add(new DataItem(value.grid.t + i * value.grid.time_step, value.points_value[i]));
            }

            return buf;
        }

        public override string ToString()
        {
            return "type is: V1DataOnGrid\n" + base.ToString() + "\n" + grid.ToString() + "\n";
        }

        public override string ToLongString()
        {
            string str = "";
            str += ToString();
            for (int i = 0; i < points_value.Length; i++)
            {
                str += "time is:" + (grid.t + i * grid.time_step) + " <" + points_value[i].X + "," + points_value[i].Y + "," + points_value[i].Z + ">\n";
            }
            return str;
        }


        IEnumerator<DataItem> IEnumerable<DataItem>.GetEnumerator()
        {
            for (int i = 0; i < grid.number_of_grid_points; i++)
            {
                yield return new DataItem(grid.t + i * grid.time_step, points_value[i]);
            }

        }



        public override string ToLongString(string format)
        {
            string str = "";
            str += "type is: V1DataOnGrid " + "\ndata is:" + base.data + "\ndate is: " + base.date + " " + grid.ToString(format) + "\n";
            for (int i = 0; i < points_value.Length; i++)
            {
                str += "time is:" + String.Format(format, grid.t + i * grid.time_step) + " <" + String.Format(format, points_value[i].X) +
                    "," + String.Format(format, points_value[i].Y) + "," + String.Format(format, points_value[i].Z) + String.Format(format, points_value[i].Length()) + ">\n";
            }
            return str;
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}