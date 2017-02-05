// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFLib
{
    public class DXFBlockRecord : DXFRecord
    {
        public string BlockName { get; set; }
    }

    class DXFBlockRecordParser : DXFRecordParser
    {
        private DXFBlockRecord _currentRecord;
        protected override DXFRecord currentRecord
        {
            get { return _currentRecord; }
        }

        protected override void createRecord(DXFDocument doc)
        {
            _currentRecord = new DXFBlockRecord();
            doc.Tables.Blocks.Add(_currentRecord);
        }

        public override void ParseGroupCode(DXFDocument doc, int groupcode, string value)
        {
            base.ParseGroupCode(doc, groupcode, value);
            if (groupcode == 2)
            {
                _currentRecord.BlockName = value;
            }
        }
    }

}
