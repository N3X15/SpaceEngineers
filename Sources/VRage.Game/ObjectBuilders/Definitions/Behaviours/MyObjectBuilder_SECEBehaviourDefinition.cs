﻿using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRage.ObjectBuilders;

namespace VRage.Game.ObjectBuilders.Definitions.Behaviours
{
    /// <summary>
    /// A block implementing SECE's component-based design model.
    /// </summary>
    [ProtoContract]
    [MyObjectBuilderDefinition]
    public class MyObjectBuilder_SECEBehaviourDefinition : MyObjectBuilder_DefinitionBase
    {
    }
}
