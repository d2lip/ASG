using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ActiveStoryTouch.DataModel
{
    public class ObservableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        private ObservableCollection<TValue> _valueObservableCollection;
        public ObservableCollection<TValue> ValueObservableCollection
        {
            get
            {
                return _valueObservableCollection;
            }
        }

        public ObservableDictionary()
        {
            _valueObservableCollection = new ObservableCollection<TValue>();
        }

        public new void Add(TKey key, TValue value)
        {
            _valueObservableCollection.Add(value);
            base.Add(key, value);
        }

        public new void Clear()
        {
            _valueObservableCollection.Clear();
            base.Clear();
        }

        public new bool Remove(TKey key)
        {
            _valueObservableCollection.Remove(this[key]);
            return base.Remove(key);
        }
    }
}
