// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXFLib
{
    public class DXFAppIDRecord : DXFRecord
    {
        public string ApplicationName { get; set; }

    }

    class DXFAppIDParser : DXFRecordParser
    {
        #region ISectionParser Member

        public override void ParseGroupCode(DXFDocument doc, int groupcode, string value)
        {
            base.ParseGroupCode(doc, groupcode, value);
            switch (groupcode)
            {
                case 2:
                    _currentRecord.ApplicationName = value;
                    break;
            }
        }

        #endregion

        DXFAppIDRecord _currentRecord;
        protected override DXFRecord currentRecord
        {
            get { return _currentRecord; }
        }

        protected override void createRecord(DXFDocument doc)
        {
            _currentRecord = new DXFAppIDRecord();
            doc.Tables.AppIDs.Add(_currentRecord);
        }
    }

}
