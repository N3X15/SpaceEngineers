using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions.Behaviours;
using VRage.Utils;

namespace Sandbox.Definitions
{
    [MyDefinitionType(typeof(MyObjectBuilder_SECEMergeBehaviourDefinition))]
    public class SECEMergeBehaviourDefinition : SECEBehaviourDefinition
    {
        public float Strength;

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);

            var mergeBuilder = builder as MyObjectBuilder_SECEMergeBehaviourDefinition;
            MyDebug.AssertDebug(mergeBuilder != null, "Initializing merge behaviour using wrong object builder.");
            Strength = mergeBuilder.Strength;
        }
    }
}
