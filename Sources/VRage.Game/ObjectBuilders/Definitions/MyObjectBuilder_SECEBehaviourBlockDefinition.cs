using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.Game.ObjectBuilders.Definitions.Behaviours;
using VRage.ObjectBuilders;

namespace VRage.Game.ObjectBuilders.Definitions
{
    /// <summary>
    /// A block implementing SECE's component-based design model.
    /// </summary>
    [ProtoContract]
    [MyObjectBuilderDefinition]
    public class MyObjectBuilder_SECEBehaviourBlockDefinition : MyObjectBuilder_CubeBlockDefinition
    {
        /// <summary>
        /// SECE Behaviours this block should have.
        /// </summary>
        [ProtoMember]
        public MyObjectBuilder_SECEBehaviourDefinition[] Behaviours { get; set; }
    }
}
