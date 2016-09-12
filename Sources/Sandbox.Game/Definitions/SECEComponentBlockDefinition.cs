using System.Collections.Generic;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game.ObjectBuilders.Definitions.Behaviours;

namespace Sandbox.Definitions
{
    /// <summary>
    /// Defines a CubeBlock that uses the Component model for behaviours.
    /// This particular class is merely a container.
    /// </summary>
    [MyDefinitionType(typeof(MyObjectBuilder_SECEBehaviourBlockDefinition))]
    public class SECEBehaviourBlockDefinition : MyCubeBlockDefinition
    {
        public List<SECEBehaviourDefinition> Behaviours { get; set; }

        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);

            var objectBuilder = builder as MyObjectBuilder_SECEBehaviourBlockDefinition;
            Behaviours.Clear();
            foreach (var bdef in objectBuilder.Behaviours)
            {
                var behaviour = MyDefinitionManager.Static.GetBlockBehaviourById(bdef.GetId());
                behaviour.container = this;
                Behaviours.Add(behaviour);
            }
        }

        public override MyObjectBuilder_DefinitionBase GetObjectBuilder()
        {
            var builder = base.GetObjectBuilder() as MyObjectBuilder_SECEBehaviourBlockDefinition;

            builder.Behaviours = new MyObjectBuilder_SECEBehaviourDefinition[Behaviours.Count];
            for (var i = 0; i < Behaviours.Count; i++) {
                builder.Behaviours[i] = Behaviours[i].GetObjectBuilder() as MyObjectBuilder_SECEBehaviourDefinition;
            }

            return builder;
        }
    }
}
