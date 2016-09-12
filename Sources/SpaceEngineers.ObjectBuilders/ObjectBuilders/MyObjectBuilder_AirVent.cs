using ProtoBuf;
using System.ComponentModel;
using System.Xml.Serialization;
using VRage.Game;
using VRage.ModAPI;
using VRage.ObjectBuilders;
using VRage.Serialization;

namespace Sandbox.Common.ObjectBuilders
{
    [ProtoContract]
    [MyObjectBuilderDefinition]
    [XmlSerializerAssembly("SpaceEngineers.ObjectBuilders.XmlSerializers")]
    [XmlInclude(typeof(MyObjectBuilder_ToolbarItem))]
    [XmlInclude(typeof(MyObjectBuilder_ToolbarItemTerminalGroup))]
    [XmlInclude(typeof(MyObjectBuilder_ToolbarItemTerminalBlock))]
    public class MyObjectBuilder_AirVent : MyObjectBuilder_FunctionalBlock
    {
        [ProtoMember]
        public bool IsDepressurizing;

        [ProtoMember, DefaultValue(null)]
        [Nullable, DynamicObjectBuilder(false)]
        public MyObjectBuilder_ToolbarItem OnEmptyAction;

        [ProtoMember, DefaultValue(null)]
        [Nullable, DynamicObjectBuilder(false)]
        public MyObjectBuilder_ToolbarItem OnFullAction;

        public override void Remap(IMyRemapHelper remapHelper)
        {
            base.Remap(remapHelper);

            if (OnEmptyAction != null)
            {
                OnEmptyAction.Remap(remapHelper);
            }

            if (OnFullAction != null)
            {
                OnFullAction.Remap(remapHelper);
            }
        }
    }
}
