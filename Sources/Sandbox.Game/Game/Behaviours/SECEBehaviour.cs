using VRage.Game;
using VRage.Game.Components;
using VRage.Game.Entity;
using VRage.Game.ObjectBuilders.Behaviours;

namespace Sandbox.Game.Behaviours
{
    [SECEBehaviourType(typeof(MyObjectBuilder_SECEBehaviour))]
    public class SECEBehaviour
    {
        //TODO: This should be set only inside entity
        public MyDefinitionId? DefinitionId = null; //{ get; private set; }

        public MyEntityComponentContainer Components { get; private set; }

        private MyGameLogicComponent m_gameLogic;
        public MyGameLogicComponent GameLogic
        {
            get { return m_gameLogic; }
            set { Components.Add<MyGameLogicComponent>(value); }
        }
        public virtual void Init(MyObjectBuilder_SECEBehaviour behaviourBuilder)
        {
        }
    }
}
