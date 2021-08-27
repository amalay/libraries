using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Amalay.Entities
{
    public class LogAnalyticSearchResult
    {
        public List<LogAnalyticTable> Tables { get; set; }
    }

    public class LogAnalyticTable
    {
        public string Name { get; set; }

        public LogAnalyticTableColumn[] Columns { get; set; }

        public List<string[]> Rows { get; set; }
    }

    public class LogAnalyticTableColumn
    {
        public string Name { get; set; }

        public string Type { get; set; }
    }

    public class LogSearchQuery
    {
        public string Query { get; set; }

        public int Top { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}
