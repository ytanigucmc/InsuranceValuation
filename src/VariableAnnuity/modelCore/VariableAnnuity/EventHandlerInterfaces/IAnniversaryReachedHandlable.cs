using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariableAnnuity.modelCore.VariableAnnuity.EventHandlerInterfaces
{
    internal interface IAnniversaryReachedHandlable
    {
        void OnAnniversaryReached(object source, EventArgs args);
    }
}
