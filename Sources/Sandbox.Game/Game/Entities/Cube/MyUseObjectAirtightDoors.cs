﻿using Sandbox.Engine.Utils;
using Sandbox.Game.Entities.Character;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.ModAPI;
using Sandbox.Common;
using System.Diagnostics;
using VRage.Game.Entity.UseObject;
using VRage.Import;
using VRage.Input;
using VRage.ModAPI;
using VRageMath;
using Sandbox.Game.Entities.Blocks;
using VRage.Game;
using VRageRender.Import;

namespace Sandbox.Game.Entities.Cube
{   
    [MyUseObject("door")]
    public class MyUseObjectAirtightDoors : MyUseObjectBase
    {
        public readonly MyAirtightDoorGeneric Door;
        public readonly Matrix LocalMatrix;

        public MyUseObjectAirtightDoors(IMyEntity owner, string dummyName, MyModelDummy dummyData, uint key)
            : base(owner, dummyData)
        {
            Door = (MyAirtightDoorGeneric)owner;
            LocalMatrix = dummyData.Matrix;
        }

        public override float InteractiveDistance
        {
            get { return MyConstants.DEFAULT_INTERACTIVE_DISTANCE; }
        }

        public override MatrixD ActivationMatrix
        {
            get { return LocalMatrix * Door.WorldMatrix; }
        }

        public override MatrixD WorldMatrix
        {
            get { return Door.WorldMatrix; }
        }

        public override int RenderObjectID
        {
            get
            {
                return Door.Render.GetRenderObjectID();
            }
        }

        public override int InstanceID
        {
            get { return -1; }
        }

        public override bool ShowOverlay
        {
            get { return true; }
        }

        public override UseActionEnum SupportedActions
        {
            get { return UseActionEnum.OpenTerminal | UseActionEnum.Manipulate; }
        }

        public override void Use(UseActionEnum actionEnum, IMyEntity entity)
        {
            var user = entity as MyCharacter;
            var relation = Door.GetUserRelationToOwner(user.ControllerInfo.ControllingIdentityId);
            if (!relation.IsFriendly())
            {
                if (user.ControllerInfo.IsLocallyHumanControlled())
                {
                    MyHud.Notifications.Add(MyNotificationSingletons.AccessDenied);
                }
                return;
            }

            switch (actionEnum)
            {
                case UseActionEnum.Manipulate:
                    Door.ChangeOpenClose(!Door.Open);
                    break;

                case UseActionEnum.OpenTerminal:
                    MyGuiScreenTerminal.Show(MyTerminalPageEnum.ControlPanel, user, Door);
                    break;
            }
        }

        public override MyActionDescription GetActionInfo(UseActionEnum actionEnum)
        {
            switch(actionEnum)
            {
                case UseActionEnum.Manipulate:
                    return new MyActionDescription()
                    {
                        Text = MySpaceTexts.NotificationHintPressToOpenDoor,
                        FormatParams = new object[] { MyInput.Static.GetGameControl(MyControlsSpace.USE) },
                        IsTextControlHint = true,
                        JoystickFormatParams = new object[] { MyControllerHelper.GetCodeForControl(MySpaceBindingCreator.CX_CHARACTER, MyControlsSpace.USE) },
                    };

                case UseActionEnum.OpenTerminal:
                    return new MyActionDescription()
                    {
                        Text = MySpaceTexts.NotificationHintPressToOpenControlPanel,
                        FormatParams = new object[] { MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL), Door.DefinitionDisplayNameText },
                        IsTextControlHint = true,
                        JoystickText = MySpaceTexts.NotificationHintJoystickPressToOpenControlPanel,
                        JoystickFormatParams = new object[] { Door.DefinitionDisplayNameText },
                    };

                default:
                    Debug.Fail("Invalid branch reached.");
                    return new MyActionDescription()
                    {
                        Text = MySpaceTexts.NotificationHintPressToOpenControlPanel,
                        FormatParams = new object[] { MyInput.Static.GetGameControl(MyControlsSpace.TERMINAL), Door.DefinitionDisplayNameText },
                        IsTextControlHint = true
                    };
            }
        }

        public override bool ContinuousUsage
        {
            get { return false; }
        }

        public override bool HandleInput() { return false; }

        public override void OnSelectionLost() { }

        public override bool PlayIndicatorSound
        {
            get { return true; }
        }
    }
}
