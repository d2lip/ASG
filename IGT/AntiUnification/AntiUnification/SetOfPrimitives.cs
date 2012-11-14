using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AntiUnification
{

    public class IdComparer : IComparer<PrimitiveData>
    {
        public int Compare(PrimitiveData x, PrimitiveData y)
        {
            if (x.ID.Count != y.ID.Count)
                return (x.ID.Count > y.ID.Count) ? 1 : -1;
            else if (x.ID.Count == 0)
                    return 0;
            else
                return (x.ID[0] >= y.ID[0]) ? 1 : -1;
        }
    }


    public class SetOfPrimitives:  List<PrimitiveData>
    {
        public string Tag { get; set; }

        public SetOfPrimitives()
            : base()
        { }

        public SetOfPrimitives(int capacity)
            : base(capacity)
        {
        }

        public SetOfPrimitives(List<PrimitiveData> points)
        {
            this.AddRange(points);
        }

        public SetOfPrimitives(PrimitiveData[] array)
        {
            this.Capacity = array.Length;
            foreach (var item in array)
            {
                this.Add(item);
            }            
        }


        public void checkAndAdd(PrimitiveData data) {

            foreach (PrimitiveData primitive in this) {
                if (primitive.IDcompare(data.ID) && primitive.Name == data.Name) {
                    return;
                }
            }

            this.Add(data);
        
        }






        public string toString() {

            string result = "";

            foreach (PrimitiveData item in this) {
                if (item.ListValue.Count !=0)
                {
                    result += Environment.NewLine + "ID[" + item.IDtoString() + "]:  " + item.Name + ":" + string.Join(",", item.ListValue);
                }
                else 
                {
                    result += Environment.NewLine + "ID[" + item.IDtoString() + "]  " + item.Name + ":" + item.Value;
                }
            
            }
                return result;
        
        }

    }
}
