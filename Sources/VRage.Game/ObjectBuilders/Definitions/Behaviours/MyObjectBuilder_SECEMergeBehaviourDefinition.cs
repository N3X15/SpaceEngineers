using ProtoBuf;
using VRage.ObjectBuilders;

namespace VRage.Game.ObjectBuilders.Definitions.Behaviours
{
    /// <summary>
    /// A block implementing SECE's component-based design model.
    /// </summary>
    [ProtoContract]
    [MyObjectBuilderDefinition]
    public class MyObjectBuilder_SECEMergeBehaviourDefinition : MyObjectBuilder_SECEBehaviourDefinition
    {
        [ProtoMember]
        public float Strength;
    }
}
