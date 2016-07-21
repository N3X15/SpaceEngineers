using ProtoBuf;
using System.Xml.Serialization;
using VRage.ObjectBuilders;

namespace Sandbox.Common.ObjectBuilders
{
    [ProtoContract]
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("SpaceEngineers.ObjectBuilders.XmlSerializers")]
    [XmlInclude(typeof(MyObjectBuilder_SimpleAutopilot))]
    public class MyObjectBuilder_AutopilotBase : MyObjectBuilder_Base
    {
    }
}
