using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Utilities
{
    public class Page<T>
    {
        public PageMetadata Meta { get; set; } = new PageMetadata();
        public IEnumerable<T> Data { get; set; } = [];

    }
}