using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchToolkit.GestureProcessor.Objects;

namespace AntiUnification
{
    public class PrimitiveData
    {

        public PrimitiveData() {
    
            _ID = new List<int>();
            ListValue = new List<string>();
    
        }
       
            // p11 = (i11,n11,v11)   --Gestures primitives and its values - Ex: gesture1,"Touch path length",1.234


            private List<int> _ID;

            public List<int> ID
            {
                get { return _ID; }
                set { _ID = value; }
            }
            private string _name;

            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }
            private double _value;

            public double Value
            {
                get { return _value; }
                set { _value = value; }
            }




            private List<string> _listValue;
            public List<string> ListValue
            {
                get { return _listValue; }
                set { _listValue = value; }
            }



            

            public void IDadd(int id) {

                _ID.Add(id);
            }


            public string IDtoString() {
                if (ID.Count == 0)
                {
                    return "";
                }
                string result = "";

                foreach (int id in ID) {
                    result += "step"+id+" and ";                
                }        
       

               return result.Remove(result.Length - 5, 5);
            
            }

           

            public bool StrListcompare(List<string> values)
            {
                try
                {
                    for (int i = 0; i < values.Count; i++)
                    {
                        if (ListValue[i] != values[i])
                        {
                            return false;
                        }
                    }
                }
                catch (Exception)
                {

                    return false;
                }
                return true;

            }


            public bool IDcompare(List<int> ids)
            {
                try
                {
                    for (int i = 0; i < ids.Count; i++)
                    {
                        if (_ID[i] != ids[i])
                        {
                            return false;
                        }
                    }
                }
                catch (Exception)
                {

                    return false;
                }
                return true;
               
            }







            public bool Equals(PrimitiveData compare){
                return (this.Name == compare.Name && this.Value == compare.Value && this.ListValue.SequenceEqual(compare.ListValue) && this.IDcompare(compare.ID));
            
            }


            private TouchPoint2 _pointsRecognized;
            public TouchPoint2 PointsRecognized
            {
                get { return _pointsRecognized; }
                set {  _pointsRecognized = value; }
            
            }
            




     

    }
}
