using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shrex.Filters
{
    public class StringCondition : FilterCondition<string>
    {
        public override bool UseQuotationMarks => true;
    }
}
