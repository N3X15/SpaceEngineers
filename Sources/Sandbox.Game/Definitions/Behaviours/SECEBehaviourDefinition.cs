using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions.Behaviours;

namespace Sandbox.Definitions
{
    [MyDefinitionType(typeof(MyObjectBuilder_SECEBehaviourDefinition))]
    public class SECEBehaviourDefinition : MyDefinitionBase
    {
        public SECEBehaviourBlockDefinition container;
        /// <summary>
        /// Override to handle initialization.
        /// </summary>
        public virtual void Init(MyObjectBuilder_SECEBehaviourDefinition def)
        {
        }
    }
}
