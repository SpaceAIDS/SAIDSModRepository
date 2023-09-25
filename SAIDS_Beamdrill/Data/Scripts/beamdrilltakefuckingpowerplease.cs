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
using VRage.Game.ModAPI;

namespace PowerOverride
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_Drill), false, "Erebus_LargeBeamDrill")]
    public class PowerOverrideLogic : MyGameLogicComponent
    {
        private MyResourceSinkComponent Sink = null;
        public IMyShipDrill exampleBlock;
        private bool hasNotified = false;

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
                if (exampleBlock.Enabled)
                {
                    // Calculate the available power of the entire grid.
                    float availableGridPower = CalculateMaxAvailableGridPower();

                    // If available power is less than 1GW, turn off the block.
                    if (availableGridPower < 300.000f)
                    {
                        exampleBlock.Enabled = false;

                        // Show the notification only once
                        if (!hasNotified)
                        {
                            //MyAPIGateway.Utilities.ShowNotification("Insufficient Power", 1500, "Red"); // Show a notification
                            hasNotified = true;
                        }
                    }
                    else
                    {
                        hasNotified = false; // Reset the flag if there's enough power.
                    }

                    CalculatePowerDraw();
                    Sink.Update();
                }
            }
            catch (Exception e)
            {
                //MyAPIGateway.Utilities.ShowNotification($"{e}", 5000, "Red");
            }
        }

        // Method to calculate the available power for the grid, similar to your example.
        private float CalculateMaxAvailableGridPower()
        {
            float totalPower = 0f;
            var blocks = new List<IMySlimBlock>();
            exampleBlock.CubeGrid.GetBlocks(blocks);

            foreach (var block in blocks)
            {
                var fatBlock = block.FatBlock;
                if (fatBlock is IMyPowerProducer)
                {
                    var powerProducer = fatBlock as IMyPowerProducer;
                    totalPower += powerProducer.MaxOutput;
                }
            }

            return totalPower;
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
               // MyAPIGateway.Utilities.ShowNotification($"{e}", 5000, "Red");
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
                return 300.000f;
            }
        }
    }
}