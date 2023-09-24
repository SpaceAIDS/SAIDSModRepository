using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI;
using Sandbox.Common.ObjectBuilders;
using VRage.Game.Components;
using VRage.ModAPI;
using VRage.ObjectBuilders;

namespace PowerOverride
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_Drill), false, "Erebus_LargeBeamDrill")]
    public class PowerOverrideLogic : MyGameLogicComponent
    {
        private MyResourceSinkComponent Sink = null;
        public IMyShipDrill exampleBlock;

        public override void Init(MyObjectBuilder_EntityBase objectBuilder)
        {
            NeedsUpdate |= MyEntityUpdateEnum.BEFORE_NEXT_FRAME;
        }

        public override void UpdateOnceBeforeFrame()
        {
            try
            {
                exampleBlock = (IMyShipDrill)Entity;

                if (exampleBlock.CubeGrid?.Physics == null)
                    return;

                Sink = exampleBlock.Components.Get<MyResourceSinkComponent>();
                Sink.SetRequiredInputFuncByType(MyResourceDistributorComponent.ElectricityId, CalculatePowerDraw);

                NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME | MyEntityUpdateEnum.EACH_10TH_FRAME;

            }
            catch (Exception e)
            {
                MyAPIGateway.Utilities.ShowNotification($"{e}", 5000, "Red");
            }
        }

        public override void UpdateAfterSimulation()
        {
            try
            {
                CalculatePowerDraw();
                Sink.Update();
            }
            catch (Exception e)
            {
                MyAPIGateway.Utilities.ShowNotification($"{e}", 5000, "Red");
            }
        }

        public override void Close()
        {
            try
            {
                if (exampleBlock == null)
                    return;


                exampleBlock = null;
            }
            catch (Exception e)
            {
                MyAPIGateway.Utilities.ShowNotification($"{e}", 5000, "Red");
            }
        }

        private float CalculatePowerDraw()
        {
            if (!exampleBlock.Enabled)
            {
                return 0f;
            }
            else
            {
                return 1000.000f;
            }
        }
    }
}