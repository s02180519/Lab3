using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;


namespace Lab1_2
{
    enum ChangeInfo { ItemChanged, Add, Remove, Replace };

    delegate void DataChangedEventHandler(object source, DataChangedEventArgs args);

    class DataChangedEventArgs : EventArgs
    {
        public ChangeInfo ChangeInfo { get; set; }
        public string Data { get; set; }
        public DataChangedEventArgs(string Data, ChangeInfo ChangeInfo)
        {
            this.ChangeInfo = ChangeInfo;
            this.Data = Data;
        }
        public override string ToString() => ChangeInfo + " " + Data;
    }

    class V1MainCollection : IEnumerable<V1Data>
    {
        private/*public*/ List<V1Data> elements = new List<V1Data>();
        public int count { get { return elements.Count; } }

        public int max_count
        {
            get
            {
                try
                {
                    var query_1 = from elem in (from item in elements where item is V1DataOnGrid select (V1DataOnGrid)item)
                                  where elem.grid.number_of_grid_points == (from item in elements where item is V1DataOnGrid select (V1DataOnGrid)item).Max(x => x.grid.number_of_grid_points)
                                  select elem.grid.number_of_grid_points;
                    var query_2 = from elem in (from item in elements where item is V1DataCollection select (V1DataCollection)item)
                                  where elem.value.Count() == (from item in elements where item is V1DataCollection select (V1DataCollection)item).Max(x => x.value.Count())
                                  select elem.value.Count();

                    return query_1.First() > query_2.First() ? query_1.First() : query_2.First();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return -1;
                }

            }
        }
        public event DataChangedEventHandler DataChanged;


        public V1Data this[int index]
        {
            get
            {
                return elements[index];
            }
            set
            {

                if (DataChanged != null)
                    DataChanged(this, new DataChangedEventArgs(elements[index].data, ChangeInfo.Replace));
                elements[index] = value;
            }
        }



        public void PropertyChangedCollector(object sender, PropertyChangedEventArgs args)
        {
            // Console.WriteLine("PropertyChangedCollector");
            if (DataChanged != null)
                DataChanged(this, new DataChangedEventArgs((sender as V1Data).data, ChangeInfo.ItemChanged));
        }
        public IEnumerable<DataItem> V1Data_ordered_by_coordinates_length
        {
            get
            {
                try
                {
                    var query_1 = from elem in (from item in elements where item is V1DataOnGrid select (V1DataOnGrid)item) from elem_DataItem in elem select elem_DataItem;
                    var query_2 = from elem in (from item in elements where item is V1DataCollection select (V1DataCollection)item) from elem_DataItem in elem select elem_DataItem;
                    var query_3 = query_1.Union(query_2);
                    query_2 = from elem in query_3 orderby elem.coordinates.Length() descending select elem;
                    return query_2;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return (IEnumerable<DataItem>)(new DataItem(-1, new System.Numerics.Vector3(0, 0, 0)));
                }
            }
        }

        public IEnumerable<float> time_one_time
        {
            get
            {
                var query_1 = from elem in (from item in elements where item is V1DataOnGrid select (V1DataOnGrid)item) from elem_DataItem in elem select elem_DataItem;
                var query_2 = from elem in (from item in elements where item is V1DataCollection select (V1DataCollection)item) from elem_DataItem in elem select elem_DataItem;
                var res = query_1.Union(query_2);
                IEnumerable<float> times = (from elem in res
                                            where res.Count(param => param.t == elem.t) == 1
                                            select elem.t);
                return times;
            }
        }

        public void Add(V1Data item)
        {
            item.PropertyChanged += PropertyChangedCollector;
            //DataChanged += DataChangedCollector;
            elements.Add(item);
            if (DataChanged != null)
                DataChanged(this, new DataChangedEventArgs(item.data, ChangeInfo.Add));

        }

        public bool Remove(string id, DateTime dateTime)
        {
            bool flag = false; int i = 0;
            while (i < elements.Count)
            {
                if (String.Compare(elements[i].data, id) == 0 && DateTime.Compare(elements[i].date, dateTime) == 0)
                {
                    //  DataChanged -= DataChangedCollector;
                    elements[i].PropertyChanged -= PropertyChangedCollector;
                    if (DataChanged != null)
                        DataChanged(this, new DataChangedEventArgs(elements[i].data, ChangeInfo.Remove));
                    elements.RemoveAt(i);

                    if (!flag) { flag = true; }
                }
                else
                    i++;
            }


            return flag;
        }

        public void AddDefaults()
        {
            Random rnd = new Random();


            Grid new_grid;
            V1DataOnGrid value1;
            V1DataCollection value2;

            new_grid = new Grid(rnd.Next(100), rnd.Next(5), 4);
            value1 = new V1DataOnGrid("ID1", new DateTime(5, 5, 5), new_grid);
            value1.InitRandom(3, 7);
            Add(value1);
            new_grid = new Grid(rnd.Next(100), rnd.Next(5), 7);
            value1 = new V1DataOnGrid("ID2", new DateTime(5, 5, 5), new_grid);
            value1.InitRandom(3, 7);
            Add(value1);
            new_grid = new Grid(rnd.Next(100), rnd.Next(5), 0);
            value1 = new V1DataOnGrid("ID3", new DateTime(5, 5, 5), new_grid);
            value1.InitRandom(3, 7);
            Add(value1);
            value2 = new V1DataCollection("ID4", new DateTime(5, 5, 5));
            value2.InitRandom(5, 1, 4, 3, 4);
            Add(value2);
            value2 = new V1DataCollection("ID5", new DateTime(5, 5, 5));
            value2.InitRandom(0, 1, 4, 3, 4);
            Add(value2);

        }
        /*  public void AddDefaults()
          {
              Random rnd = new Random();
              Grid new_grid;
              V1DataOnGrid value1;
              V1DataCollection value2;
              new_grid = new Grid(rnd.Next(100), rnd.Next(5), 4);
              value1 = new V1DataOnGrid("ID1", new DateTime(5, 5, 5),
  new_grid);
              value1.InitRandom(3, 7);
              Add(value1);
              value2 = new V1DataCollection("ID2", new DateTime(5, 5, 5));
              value2.InitRandom(5, 1, 4, 3, 4);
              Add(value2);
          }*/

        public override string ToString()
        {
            string str = "";
            for (int i = 0; i < count; i++)
            {
                str = str + elements[i].ToString() + "\n";
            }
            return str;
        }

        public IEnumerator<V1Data> GetEnumerator()
        {
            return ((IEnumerable<V1Data>)elements).GetEnumerator();
        }


        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((System.Collections.IEnumerable)elements).GetEnumerator();
        }

        public string ToLongString(string format)
        {
            string str = "";
            for (int i = 0; i < count; i++)
            {
                str = str + elements[i].ToLongString(format) + "\n";
            }
            return str;
        }
    }
}