using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VariableAnnuity;

namespace VariableAnnuity
{

    public class RecordHolder: BaseRecordHolder
    {
        public RecordHolder():base()
        {
        }

        public override List<(string, object)> GetCurrentRecord()
        {
            return record;
        }

        public override void PushCurrentRecord()
        {
            AllRecords.Add(record);
            record = new List<(string, object)>();
        }

        public override void StartNewRecord()
        {
            record = new List<(string, object)>();
        }

        public override void AddElement(string key, object value)
        {
            record.Add((key, value));
        }

        public override void AddDateTime(string key, DateTime value, string formatString = "yyyy/MM/dd")
        {
            record.Add((key, value.ToString(formatString)));
        }
        public override void AddBoolAsOneZeoro(string key, bool value)
        {
            AddElement(key, value ? 1 : 0);
        }
    }
}
