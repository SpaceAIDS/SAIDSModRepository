using Sandbox.ModAPI;
using System;
using System.Collections.Generic;
using System.Timers;
using VRage.Game.ModAPI;
using VRage.ModAPI;

namespace SimpleInventorySort
{
    public class SimulationSort : SimulationProcessorBase
    {
        private DateTime m_lastUpdate;
        private Timer m_sortTimer;
        private bool m_init = false;
        private Queue<IMyCubeGrid> gridsToProcess;

        public SimulationSort()
        {
            gridsToProcess = new Queue<IMyCubeGrid>();
            PopulateGridQueue();

            m_sortTimer = new Timer();
            m_sortTimer.Interval = 60000;  // 1 minute
            m_sortTimer.Elapsed += TimerElapsed;
            m_sortTimer.Start();
        }

        private void PopulateGridQueue()
        {
            HashSet<IMyEntity> allEntities = new HashSet<IMyEntity>();
            MyAPIGateway.Entities.GetEntities(allEntities, e => e is IMyCubeGrid);

            foreach (IMyEntity entity in allEntities)
            {
                IMyCubeGrid grid = entity as IMyCubeGrid;
                if (grid != null)
                {
                    gridsToProcess.Enqueue(grid);
                }
            }
        }



        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (gridsToProcess.Count > 0)
                {
                    var grid = gridsToProcess.Dequeue();
                    SortInventory(grid);
                    gridsToProcess.Enqueue(grid);
                }
            }
            finally
            {
                int intervalTime;
                if (MyAPIGateway.Multiplayer.MultiplayerActive)
                {
                    intervalTime = Math.Max(30, Settings.Instance.Interval);
                }
                else
                {
                    intervalTime = Math.Max(2, Settings.Instance.Interval);
                }

                m_sortTimer.Interval = intervalTime * 1000;
                m_sortTimer.Enabled = true;
            }
        }

        private void SortInventory(IMyCubeGrid grid)
        {
            if (!Inventory.QueueReady && Settings.Instance.Enabled)
            {
                CubeGridTracker.TriggerRebuild();
                Inventory.TriggerRebuild();
                Conveyor.TriggerRebuild();

                if (MyAPIGateway.Multiplayer.MultiplayerActive && !MyAPIGateway.Multiplayer.IsServer)
                {
                    return;
                }

                Inventory.SortInventory();
            }
        }

        public override void Handle()
        {
            if (MyAPIGateway.Session == null)
            {
                return;
            }

            if (MyAPIGateway.Multiplayer.MultiplayerActive && !MyAPIGateway.Multiplayer.IsServer)
            {
                return;
            }

            try
            {
                if (!m_init)
                {
                    m_init = true;
                    SetupSort();
                }

                if (DateTime.Now - m_lastUpdate > TimeSpan.FromMilliseconds(500))
                {
                    m_lastUpdate = DateTime.Now;

                    if (Inventory.QueueReady)
                    {
                        Inventory.ProcessQueue();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Instance.WriteLine(string.Format("Handle(): {0}", ex.ToString()));
            }
            finally
            {
                if (Inventory.QueueReady && Inventory.QueueCount < 1)
                {
                    Inventory.QueueReady = false;
                }
            }

            base.Handle();
        }

        private void SetupSort()
        {
            Inventory.QueueReady = false;
            m_lastUpdate = DateTime.Now;
            m_sortTimer = new Timer();
            int intervalTime = MyAPIGateway.Multiplayer.MultiplayerActive ? Math.Max(30, Settings.Instance.Interval) : Math.Max(2, Settings.Instance.Interval);

            m_sortTimer.Interval = intervalTime * 1000;
            m_sortTimer.AutoReset = false;
            m_sortTimer.Elapsed += TimerElapsed;
            m_sortTimer.Enabled = true;
        }
    }
}
