using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Text.RegularExpressions;



namespace TouchToolkit.GestureProcessor.Objects
{
    /// <summary>
    /// TODO: Store the data in a queue or maintain an index sorted by timestamp... this will help improve performance
    /// of the cleanup process
    /// </summary>
    public class PartiallyEvaluatedGestures
    {
        private static List<ValidateBlockResult> _cache = new List<ValidateBlockResult>();

        public static void Add(string gestureName, int blockNo, ValidSetOfPointsCollection data, string blockName)
        {
            List<ValidateBlockResult> previous = Get(gestureName, blockNo);

            if (previous.Count >= 1)
            {
                Remove(previous[0]);            
            } 

            var item = new ValidateBlockResult()
            {
                Data = data,
                GestureName = gestureName,
                ValidateBlockNo = blockNo,
                ValidateBlockName = blockName
            };
            _cache.Add(item);
        }

        public static void clearCache(){
          _cache = new List<ValidateBlockResult>();       
        
        }

        public static List<ValidateBlockResult> Get(string gestureName, int blockNo)
        {
            var results = _cache.Where(x => x.GestureName == gestureName && x.ValidateBlockNo == blockNo).ToList<ValidateBlockResult>();

            return results;
        }

        public static List<ValidateBlockResult> Get(string gestureName, string blockName)
        {
            var results = _cache.Where(x => x.GestureName == gestureName && x.ValidateBlockName == blockName).ToList<ValidateBlockResult>();

            return results;
        }

        public static ValidateBlockResult[] GetAll()
        {
            ValidateBlockResult[] list = new ValidateBlockResult[_cache.Count];
            _cache.CopyTo(list);

            return list;
        }

        /// <summary>
        /// Removes the item and any other results listed in item.AssociatedResults
        /// </summary>
        /// <param name="item"></param>
        public static void Remove(ValidateBlockResult item)
        {           
        /*    foreach (var id in item.AssociatedResults)
            {
                var relatedResult = _cache.SingleOrDefault(x => x.Id == id);
                
                if (relatedResult != null) {                   
                    _cache.Remove(relatedResult);
                    Remove(relatedResult);                    
                      
               } */


            _cache.Remove(item);
           
          //  _cache.Remove(item);
            
            
            
            //TODO: Should follow the above logic. The following code is only for testing another feature
        /*    var itemsToRemove = _cache.Where(x => x.GestureName == item.GestureName);
            foreach (var g in itemsToRemove)
                _cache.Remove(g);*/
        }

        static Dictionary<string, double> _buffer;

        public static double getValueFromID(string id)
        {

            if (_buffer == null)
            {
                cleanBuffer();
            }
            double result;
            if (_buffer.TryGetValue(id, out result))
            {
                return result;
            }
            return 0;
        }


        public static bool addInBuffer(string id, double value)
        {
            if (_buffer == null)
            {
                cleanBuffer();
            }

            if (!_buffer.ContainsKey(id))
            {
                _buffer.Add(id, value);
                return true;
            }
            else
            {
                _buffer.Remove(id);
                _buffer.Add(id, value);            
            }

            return false;

        }

        public static void cleanBuffer()
        {
            _buffer = new Dictionary<string, double>();
        }



        public static string getIDFromVariable(string variable)
        {

            Match found = System.Text.RegularExpressions.Regex.Match(variable, "[A-Za-z]+");
            return found.Value;

        }

        public static double getMultiplierFromVariable(string variable)
        {

            Match found = System.Text.RegularExpressions.Regex.Match(variable, "[0-9]+.[0-9]+");
            try
            {
                double value = Convert.ToDouble(found.Value);
                return value;
            }
            catch (Exception)
            {
                return 0;
            }

        }

    }



}
