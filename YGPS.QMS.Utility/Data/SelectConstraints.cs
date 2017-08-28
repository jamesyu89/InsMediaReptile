using System.Collections.Generic;
using System.Linq;

namespace InstagramPhotos.Utility.Data
{
    public class FilterConstraints
    {
        List<string> _Constraints { get; set; }
        public CmdParams Params { get; set; }

        public FilterConstraints()
        {
            _Constraints = new List<string>();
            Params = new CmdParams();
        }

        public string BuildWhere()
        {
            return (_Constraints.Count > 0 ? "WHERE " : "") + _Constraints.Select( c => "(" + c + ")").ToCSV(" AND ");
        }

        public void Add(string constraint)
        {
            Add(constraint, null);
        }

        public void Add(string constraint, CmdParams cmdParams)
        {
            _Constraints.Add(constraint);
            if(cmdParams != null)
            {
                foreach( var p in cmdParams )
                {
                    this.Params.Add(p.Key, p.Value);
                }
            }
        }
    }
}
