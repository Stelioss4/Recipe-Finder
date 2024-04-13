using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recipe_Finder
{
    public class MarketList
    {
		private List<string> _marketlinkList;

		public List<string> MarketlinkList
        {
			get { return _marketlinkList; }
			set { _marketlinkList = value; }
		}

	}
}
