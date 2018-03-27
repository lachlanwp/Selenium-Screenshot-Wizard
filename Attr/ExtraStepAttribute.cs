using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossBrowserPreviews.Attr
{
    [System.AttributeUsage(System.AttributeTargets.Method)]
    class ExtraStepAttribute : System.Attribute
    {
        public string StepTag;
        public ExtraStepAttribute(string stepTag)
        {
            this.StepTag = stepTag;
        }
    }
}
