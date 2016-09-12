using ProtoBuf;
using System.ComponentModel;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace VRage.Game.ObjectBuilders.Behaviours
{
    public class MyObjectBuilder_SECEBehaviour : MyObjectBuilder_Base
    {
        [ProtoMember, DefaultValue(0)]
        [Serialize(MyObjectFlags.DefaultZero)]
        public long BehaviourId = 0;
        public bool ShouldSerializeBehaviourId() { return BehaviourId != 0; }
    }
}
