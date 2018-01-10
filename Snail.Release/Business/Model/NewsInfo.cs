using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Snail.Release.Business.Model
{
    [Alias("news")]
    public class NewsInfo
    {
        [AutoIncrement]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }
    }
}
