using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VRage;
using VRage.ObjectBuilders;
using VRage.Game.Components;
using VRage.Game.Entity;
using Sandbox.Game.Behaviours;
using VRage.Game.ObjectBuilders.Behaviours;

namespace Sandbox.Game.Entities
{
    // Based on MyEntityFactory
    internal static class SECEBehaviourFactory
    {
        static MyObjectFactory<SECEBehaviourTypeAttribute, SECEBehaviour> m_behaviourFactory = new MyObjectFactory<SECEBehaviourTypeAttribute, SECEBehaviour>();

        public static void RegisterDescriptorsFromAssembly(Assembly assembly)
        {
            if (assembly != null)
                m_behaviourFactory.RegisterFromAssembly(assembly);
        }

        public static SECEBehaviour Create(MyObjectBuilder_Base builder)
        {
            return Create(builder.TypeId, builder.SubtypeName);
        }

        public static SECEBehaviour Create(MyObjectBuilderType typeId, string subTypeName = null, bool autoRaiseCreated = true)
        {
            ProfilerShort.Begin("SECEBehaviourFactory.Create(...)");
            var behaviour = m_behaviourFactory.CreateInstance(typeId);
            AddScriptGameLogic(behaviour, typeId, subTypeName);
            ProfilerShort.End();
            //MyEntities.RaiseEntityCreated(behaviour);            
            return behaviour;
        }

        public static T Create<T>(MyObjectBuilder_Base builder) where T : SECEBehaviour
        {
            ProfilerShort.Begin("SECEBehaviourFactory.Create(...)");
            T behaviour = m_behaviourFactory.CreateInstance<T>(builder.TypeId);
            AddScriptGameLogic(behaviour, builder.GetType(), builder.SubtypeName);
            ProfilerShort.End();
            //MyEntities.RaiseEntityCreated(behaviour);           
            return behaviour;
        }

        // using an empty set instead of null avoids special-casing null
        private static readonly HashSet<Type> m_emptySet = new HashSet<Type>();

        public static void AddScriptGameLogic(SECEBehaviour entity, MyObjectBuilderType builderType, string subTypeName = null)
        {
            var scriptManager = Sandbox.Game.World.MyScriptManager.Static;
            if (scriptManager == null || entity == null)
                return;

            // both types of logic components are valid to be attached:

            // (1) those that are specific for the given subTypeName
            HashSet<Type> subEntityScripts;
            if (subTypeName != null)
            {
                var key = new Tuple<Type, string>(builderType, subTypeName);
                subEntityScripts = scriptManager.SubEntityScripts.GetValueOrDefault(key, m_emptySet);
            }
            else
            {
                subEntityScripts = m_emptySet;
            }

            // (2) and those that don't care about the subTypeName
            HashSet<Type> entityScripts = scriptManager.EntityScripts.GetValueOrDefault(builderType, m_emptySet);

            // if there are no component types to attach leave the entity as-is
            var count = subEntityScripts.Count + entityScripts.Count;
            if (count == 0)
                return;

            // just concatenate the two type-sets, they are disjunct by definition (see ScriptManager)
            var logicComponents = new List<MyGameLogicComponent>(count);
            foreach (var logicComponentType in entityScripts.Concat(subEntityScripts))
            {
                logicComponents.Add((MyGameLogicComponent)Activator.CreateInstance(logicComponentType));
            }

            // wrap the gamelogic-components to appear as a single component to the entity
            //entity.GameLogic = MyCompositeGameLogicComponent.Create(logicComponents, entity);
        }

        public static MyObjectBuilder_SECEBehaviour CreateObjectBuilder(SECEBehaviour entity)
        {
            return m_behaviourFactory.CreateObjectBuilder<MyObjectBuilder_SECEBehaviour>(entity);
        }
    }
}
