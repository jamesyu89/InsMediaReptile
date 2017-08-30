using System;
using System.Collections.Generic;
using System.Data;

namespace InstagramPhotos.Utility.Data
{
    public class ComplexParams : List<ComplexParameter>
    {
        public ComplexParams()
        {
        }

        public ComplexParams(IEnumerable<ComplexParameter> init)
            : base(init)
        {
        }
    }

    public class ComplexParameter
    {
        public String Key { get; set; }
        public DbType DbType { get; set; }
        public Object Value { get; set; }
    }
}