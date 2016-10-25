using Sandbox.Common.ObjectBuilders;
using Sandbox.Engine.Multiplayer;
using Sandbox.Engine.Utils;
using Sandbox.Game.Entities;
using Sandbox.Game.Entities.Blocks;
using Sandbox.Game.Entities.Cube;
using Sandbox.Game.Gui;
using Sandbox.Game.Localization;
using Sandbox.Game.Screens.Terminal.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VRage;
using VRage.Network;
using VRage.Voxels;
using VRageMath;

namespace Entities.Blocks
{
    [MyCubeBlockType(typeof(MyObjectBuilder_RepairProjector))]
    public class MySpaceRepairProjector : MyProjectorBase
    {
        private bool m_needsAlignment = false;
        private bool m_needsRotation = false;

        public MySpaceRepairProjector():base()
        {
            CreateTerminalControls();
            m_showOnlyBuildable = true;
            KeepProjection = true;
        }

        private void SendCaptureSnapshot()
        {
            MyMultiplayer.RaiseEvent(this, x => x.OnCaptureSnapshot);
        }

        [Event, Reliable, Server]
        private void OnCaptureSnapshot()
        {
            RemoveProjection(false);
            if (Clipboard.IsActive == false && this.CubeGrid is MyCubeGrid)
            {
                m_projectionOffset = Vector3I.Zero;
                m_projectionRotation = Vector3I.Zero;
                Clipboard.CopyGrid(this.CubeGrid);
                //Clipboard.Activate();
                OnBlueprintScreen_Closed(null);
                m_needsAlignment = true;
                m_needsRotation = true;
            }
        }

        public override void UpdateAfterSimulation100()
        {
            base.UpdateAfterSimulation100();
            if (Clipboard.IsActive && ProjectedGrid != null)
            {
                if (m_needsRotation)
                {
                    m_needsRotation = false;
                    Autorotate();
                    return;
                }
                if (m_needsAlignment)
                {
                    m_needsAlignment = false;

                    Autoalign();
                    return;
                }
            }
        }

        void Autorotate()
        {
            /*
            Quaternion localRot = Quaternion.CreateFromRotationMatrix(CubeGrid.WorldMatrix);
            var localDegrees = MyMath.QuaternionToEuler(localRot) / MathHelper.PiOver2;
            Quaternion projectedRot = Quaternion.CreateFromRotationMatrix(ProjectedGrid.WorldMatrix);
            var projectedDegrees = MyMath.QuaternionToEuler(localRot) / MathHelper.PiOver2;

            var rotOffset = -localRot;
            */
            Vector3 rotOffset = GetCubeRotation(ProjectedGrid.GetCubeBlock(Position)) - GetCubeRotation(SlimBlock);
            m_projectionRotation.X = (int)Math.Round(rotOffset.X); //(int)(rotOffset.X / 90f) * 90;
            m_projectionRotation.Y = (int)Math.Round(rotOffset.Y); //(int)(rotOffset.Y / 90f) * 90;
            m_projectionRotation.Z = (int)Math.Round(rotOffset.Z); //(int)(rotOffset.Z / 90f) * 90;

            OnOffsetsChanged();
        }

        private Vector3 GetCubeRotation(MySlimBlock subject)
        {
            Quaternion rot;
            subject.Orientation.GetQuaternion(out rot);
            return MyMath.QuaternionToEuler(rot) * MathHelper.PiOver2;
        }

        void Autoalign() { 

            Vector3D posOffset = (ProjectedGrid.GetCubeBlock(Position).WorldPosition - SlimBlock.WorldPosition) / CubeGrid.GridSize;
            m_projectionOffset.X = (int)Math.Round(posOffset.X);
            m_projectionOffset.Y = (int)Math.Round(posOffset.Y);
            m_projectionOffset.Z = (int)Math.Round(posOffset.Z);

            OnOffsetsChanged();
        }

        protected override void UpdateText()
        {
            if (m_needsRotation)
            {
                base.UpdateBaseText();
                DetailedInfo.AppendLine("ROTATING, PLEASE WAIT...");
                RaisePropertiesChanged();
                return;
            }
            if (m_needsAlignment)
            {
                base.UpdateBaseText();
                DetailedInfo.Append("ALIGNING, PLEASE WAIT...\n");
                RaisePropertiesChanged();
                return;
            }
            UpdateText();
        }

        protected override void UpdateDetailedInfoStats()
        {
            var ProjectedBlock = ProjectedGrid.GetCubeBlock(Position);
            DetailedInfo.AppendLine(string.Format("Local Rotation: {0}", GetCubeRotation(SlimBlock)));
            DetailedInfo.AppendLine(string.Format("Projected Rotation: {0}", GetCubeRotation(ProjectedBlock)));

            DetailedInfo.AppendLine(string.Format("Local Position: {0}", SlimBlock.WorldPosition));
            DetailedInfo.AppendLine(string.Format("Projected Position: {0}", ProjectedBlock.WorldPosition));
            base.UpdateDetailedInfoStats();
        }

        protected override void CreateTerminalControls()
        {
            if (MyTerminalControlFactory.AreControlsCreated<MySpaceRepairProjector>())
                return;
            base.CreateTerminalControls();
            if (!MyFakes.ENABLE_PROJECTOR_BLOCK)
            {
                return;
            }

            var captureSnapshotBtn = new MyTerminalControlButton<MySpaceRepairProjector>("CaptureSnapshot", MySpaceTexts.CaptureSnapshotButton, MySpaceTexts.Blank, (p) => p.SendCaptureSnapshot());
            captureSnapshotBtn.SupportsMultipleBlocks = true;
            captureSnapshotBtn.EnableAction(MyTerminalActionIcons.RESET);
            MyTerminalControlFactory.AddControl(captureSnapshotBtn);

            //Position
            var offsetX = new MyTerminalControlSlider<MySpaceRepairProjector>("X", MySpaceTexts.BlockPropertyTitle_ProjectionOffsetX, MySpaceTexts.Blank);
            offsetX.SetLimits(-50, 50);
            offsetX.DefaultValue = 0;
            offsetX.Getter = (x) => x.m_projectionOffset.X;
            offsetX.Setter = (x, v) =>
            {
                x.m_projectionOffset.X = Convert.ToInt32(v);
                x.OnOffsetsChanged();
            };
            offsetX.Writer = (x, result) => result.AppendInt32((int)(x.m_projectionOffset.X));
            offsetX.EnableActions(step: 0.01f);
            offsetX.Enabled = (x) => x.IsProjecting();
            MyTerminalControlFactory.AddControl(offsetX);

            var offsetY = new MyTerminalControlSlider<MySpaceRepairProjector>("Y", MySpaceTexts.BlockPropertyTitle_ProjectionOffsetY, MySpaceTexts.Blank);
            offsetY.SetLimits(-50, 50);
            offsetY.DefaultValue = 0;
            offsetY.Getter = (x) => x.m_projectionOffset.Y;
            offsetY.Setter = (x, v) =>
            {
                x.m_projectionOffset.Y = Convert.ToInt32(v);
                x.OnOffsetsChanged();
            };
            offsetY.Writer = (x, result) => result.AppendInt32((int)(x.m_projectionOffset.Y));
            offsetY.EnableActions(step: 0.01f);
            offsetY.Enabled = (x) => x.IsProjecting();
            MyTerminalControlFactory.AddControl(offsetY);

            var offsetZ = new MyTerminalControlSlider<MySpaceRepairProjector>("Z", MySpaceTexts.BlockPropertyTitle_ProjectionOffsetZ, MySpaceTexts.Blank);
            offsetZ.SetLimits(-50, 50);
            offsetZ.DefaultValue = 0;
            offsetZ.Getter = (x) => x.m_projectionOffset.Z;
            offsetZ.Setter = (x, v) =>
            {
                x.m_projectionOffset.Z = Convert.ToInt32(v);
                x.OnOffsetsChanged();
            };
            offsetZ.Writer = (x, result) => result.AppendInt32((int)(x.m_projectionOffset.Z));
            offsetZ.EnableActions(step: 0.01f);
            offsetZ.Enabled = (x) => x.IsProjecting();
            MyTerminalControlFactory.AddControl(offsetZ);

            //Rotation

            var rotationX = new MyTerminalControlSlider<MySpaceRepairProjector>("RotX", MySpaceTexts.BlockPropertyTitle_ProjectionRotationX, MySpaceTexts.Blank);
            rotationX.SetLimits(-2, 2);
            rotationX.DefaultValue = 0;
            rotationX.Getter = (x) => x.m_projectionRotation.X;
            rotationX.Setter = (x, v) =>
            {
                x.m_projectionRotation.X = Convert.ToInt32(v);
                x.OnOffsetsChanged();
            };
            rotationX.Writer = (x, result) => result.AppendInt32((int)x.m_projectionRotation.X * 90).Append("°");
            rotationX.EnableActions(step: 0.2f);
            rotationX.Enabled = (x) => x.IsProjecting();
            MyTerminalControlFactory.AddControl(rotationX);

            var rotationY = new MyTerminalControlSlider<MySpaceRepairProjector>("RotY", MySpaceTexts.BlockPropertyTitle_ProjectionRotationY, MySpaceTexts.Blank);
            rotationY.SetLimits(-2, 2);
            rotationY.DefaultValue = 0;
            rotationY.Getter = (x) => x.m_projectionRotation.Y;
            rotationY.Setter = (x, v) =>
            {
                x.m_projectionRotation.Y = Convert.ToInt32(v);
                x.OnOffsetsChanged();
            };
            rotationY.Writer = (x, result) => result.AppendInt32((int)x.m_projectionRotation.Y * 90).Append("°");
            rotationY.EnableActions(step: 0.2f);
            rotationY.Enabled = (x) => x.IsProjecting();
            MyTerminalControlFactory.AddControl(rotationY);

            var rotationZ = new MyTerminalControlSlider<MySpaceRepairProjector>("RotZ", MySpaceTexts.BlockPropertyTitle_ProjectionRotationZ, MySpaceTexts.Blank);
            rotationZ.SetLimits(-2, 2);
            rotationZ.DefaultValue = 0;
            rotationZ.Getter = (x) => x.m_projectionRotation.Z;
            rotationZ.Setter = (x, v) =>
            {
                x.m_projectionRotation.Z = Convert.ToInt32(v);
                x.OnOffsetsChanged();
            };
            rotationZ.Writer = (x, result) => result.AppendInt32((int)x.m_projectionRotation.Z * 90).Append("°");
            rotationZ.EnableActions(step: 0.2f);
            rotationZ.Enabled = (x) => x.IsProjecting();
            MyTerminalControlFactory.AddControl(rotationZ);
        }

        protected override bool CheckIsWorking()
        {

            if (ResourceSink != null && !ResourceSink.IsPowered)
            {
                return false;
            }

            // not sure if it was correct earlier
            //return Enabled && base.CheckIsWorking();

            return base.CheckIsWorking();
        }
    }
}
