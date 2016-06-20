using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilitiaOrganizationSystem
{
    public class MoveTag
    {
        public List<Militia> moveMilitias { get; set; }
        public object source { get; set; }

        public MoveTag(object src, List<Militia> mList)
        {
            source = src;
            moveMilitias = mList;
        }
    }
}
