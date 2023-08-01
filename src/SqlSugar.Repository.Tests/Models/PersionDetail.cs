using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugar.Repository.Tests.Models
{
    [SugarTable("test.PersionDetail")]
    public class PersionDetail
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string Code { get; set; }
        public string CnName { get; set; }
        public string EnName { get; set; }
        public string Data { get; set; }
        public DateTimeOffset CreateTime { get; set; }
        public string Creator { get; set; }
        public int SortNo { get; set; }
    }
}
