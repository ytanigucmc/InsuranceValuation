using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity
{
    public abstract class BaseRecordHolder
    {
        public List<List<(string, object)>> AllRecords;
        public List<(string, object)> record;
        public BaseRecordHolder()
        {
            AllRecords = new List<List<(string, object)>>();
            record = new List<(string, object)>();
        }

        public abstract List<(string, object)> GetCurrentRecord();

        public abstract void PushCurrentRecord();

        public abstract void StartNewRecord();

        public abstract void AddElement(string key, object value);

        public abstract void AddDateTime(string key, DateTime value, string formatString = "yyyy/MM/dd");

        public abstract void AddBoolAsOneZeoro(string key, bool value);
    }
}
