using System.Collections.Generic;

namespace InstagramPhotos.Utility.Data
{
	public class CmdParams : Dictionary<string, object>
	{
        public CmdParams()
        {
        }

        public CmdParams(Dictionary<string, object> init): base(init)
        {
        }
	}
}
