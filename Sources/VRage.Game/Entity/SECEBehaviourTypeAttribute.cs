using System;
using VRage.Game.Common;

namespace VRage.Game.Entity
{
    public class SECEBehaviourTypeAttribute : MyFactoryTagAttribute
    {
        public SECEBehaviourTypeAttribute(Type objectBuilderType, bool mainBuilder = true)
            : base(objectBuilderType, mainBuilder)
        {
        }
    }
}
