using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_2
{
    class DataItem   /* значения поля в момент времени t*/
    {
        public float t { get; set; }
        public System.Numerics.Vector3 coordinates { get; set; }

        public DataItem(float new_t, System.Numerics.Vector3 new_coordinates)
        {
            t = new_t;
            coordinates = new_coordinates;
        }

        public override string ToString()
        {
            string str = "time is:" + t + "\ncoordinates are: ";
            //  str += ToString();
            str += "<" + coordinates.X + ";" + coordinates.Y + ";" + coordinates.Z + ">";
            return str;
        }
        public string ToString(string format)
        {
            string str = "";
            str += "current time:" + String.Format(format, t) + " current coordinates:<" + String.Format(format, coordinates.X) + ";" +
                String.Format(format, coordinates.Y) + ";" + String.Format(format, coordinates.Z) + "> vector`s length:" + String.Format(format, coordinates.Length()) + "\n";
            return str;
        }

        public static explicit operator DataItem(V1DataCollection v)
        {
            throw new NotImplementedException();
        }
    }
}
