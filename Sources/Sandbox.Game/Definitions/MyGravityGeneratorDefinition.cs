using Sandbox.Common.ObjectBuilders.Definitions;
using VRage.Game;
using VRage.Game.Definitions;
using VRage.Utils;
using VRageMath;

namespace Sandbox.Definitions
{
    [MyDefinitionType(typeof(MyObjectBuilder_GravityGeneratorDefinition))]
    public class MyGravityGeneratorDefinition : MyGravityProviderDefinition
    {
	    public MyStringHash ResourceSinkGroup;
        public float RequiredPowerInput;
        public MyBoundedVector3 FieldSize;
        protected override void Init(MyObjectBuilder_DefinitionBase builder)
        {
            base.Init(builder);

            var obGenerator = builder as MyObjectBuilder_GravityGeneratorDefinition;
            MyDebug.AssertDebug(obGenerator != null, "Initializing gravity generator definition using wrong object builder.");
	        ResourceSinkGroup = MyStringHash.GetOrCompute(obGenerator.ResourceSinkGroup);
            RequiredPowerInput = obGenerator.RequiredPowerInput;
            FieldSize = obGenerator.FieldSize;
        }
    }
}
