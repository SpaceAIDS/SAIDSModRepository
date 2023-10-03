using System.Collections.Generic;
using System.IO;
using Sandbox.Game;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.ModAPI;
using VRageMath;
using IMyCockpit = Sandbox.ModAPI.IMyCockpit;


namespace Invalid.Jumpgate
{
    [MySessionComponentDescriptor(MyUpdateOrder.BeforeSimulation)]
    public class Jumpgate : MySessionComponentBase
    {
        private Vector3D StarOut = Vector3D.Zero;
        private MatrixD StarOutMat = MatrixD.Identity;
        private string StarOutString = "";
        private Vector3D StarIn = Vector3D.Zero;
        private MatrixD StarInMat = MatrixD.Identity;
        private string StarInString = "";
        private string ip = "54.36.216.222:27058";
        private List<IMyPlayer> allP = new List<IMyPlayer>();
        private List<IMyCockpit> seats = new List<IMyCockpit>();
        private List<IMySlimBlock> allBlocks = new List<IMySlimBlock>();
        private List<IMySlimBlock> entAllblocks = new List<IMySlimBlock>();
        private List<IMyCharacter> groupOut = new List<IMyCharacter>();
        private List<string> blacklist = new List<string>();
        private Color OutgateCol = Color.IndianRed;
        private Color IngateCol = Color.LightBlue;
        private string mainPilot = "";
        private IMyCubeGrid entaddcube;
        private int _counter = 0;

        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {
            base.Init(sessionComponent);
            MyAPIGateway.Utilities.MessageEntered += UtilitiesOnMessageEntered;
            MyAPIGateway.Entities.OnEntityAdd += EntitiesOnOnEntityAdd;
        }

        private void EntitiesOnOnEntityAdd(IMyEntity obj)
        {
            if (MyAPIGateway.Session.IsServer)
            {
                if (obj is IMyCubeGrid)
                {
                    entaddcube = obj as IMyCubeGrid;
                    foreach (var player in allP)
                    {
                        if (entaddcube.CustomName != null && entaddcube.CustomName == player.DisplayName && player.Character != null)
                        {
                            entAllblocks.Clear();
                            entaddcube.GetBlocks(entAllblocks);
                            for (int i = 0; i < entAllblocks.Count; i++)
                            {
                                if (entAllblocks[i].FatBlock != null && entAllblocks[i].FatBlock is IMyCockpit cockpit)
                                {
                                    cockpit.AttachPilot(player.Character);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public override void LoadData()
        {
            base.LoadData();
            if (MyAPIGateway.Utilities.GetVariable("StarOutString", out StarOutString))
            {
                Vector3D.TryParse(StarOutString, out StarOut);
                StarOutMat = MatrixD.CreateTranslation(StarOut);
            }
            else
            {
                StarOut = Vector3D.Zero;
                StarOutString = "";
            }

            if (MyAPIGateway.Utilities.GetVariable("StarInString", out StarInString))
            {
                Vector3D.TryParse(StarInString, out StarIn);
                StarInMat = MatrixD.CreateTranslation(StarIn);
            }
            else
            {
                StarIn = Vector3D.Zero;
                StarOutString = "";
            }

            if (!MyAPIGateway.Utilities.GetVariable("ip", out ip))
            {
                ip = "54.36.216.222:27058";
            }
        }

        private void SaveCurrentGridAsPrefab(IMyCubeGrid grid, string prefabName)
        {
            MyObjectBuilder_CubeGrid objectBuilder = (MyObjectBuilder_CubeGrid)grid.GetObjectBuilder();
            string serializedGrid = MyAPIGateway.Utilities.SerializeToXML(objectBuilder);

            string prefabPath = Path.Combine(MyAPIGateway.Utilities.GamePaths.ModsPath, "Data", "Prefabs", prefabName + ".sbc");
            File.WriteAllText(prefabPath, serializedGrid);
        }

        private void UpdateSave()
        {
            MyAPIGateway.Utilities.SetVariable("StarOutString",StarOut.ToString());
            MyAPIGateway.Utilities.SetVariable("StarInString", StarIn.ToString());
            MyAPIGateway.Utilities.SetVariable("ip",ip);
        }

        private void UtilitiesOnMessageEntered(string messagetext, ref bool sendtoothers)
        {
            if (MyAPIGateway.Multiplayer.MyId == MyAPIGateway.Multiplayer.ServerId)
            {
                if (messagetext == "/outgate add")
                {
                    if (MyAPIGateway.Session.Player.Character != null)
                    {
                        StarOut = MyAPIGateway.Session.Player.Character.GetPosition();
                        StarOutMat = MatrixD.CreateTranslation(StarOut);
                        UpdateSave();
                    }
                }

                if (messagetext == "/outgate remove")
                {
                    StarOut = Vector3D.Zero;
                    StarOutMat = MatrixD.Identity;
                    UpdateSave();
                }

                if (messagetext == "/ingate add")
                {
                    if (MyAPIGateway.Session.Player.Character != null)
                    {
                        StarIn = MyAPIGateway.Session.Player.Character.GetPosition();
                        StarInMat = MatrixD.CreateTranslation(StarIn);
                        UpdateSave();
                    }
                }

                if (messagetext == "/ingate remove")
                {
                    StarIn = Vector3D.Zero;
                    StarInMat = MatrixD.Identity;
                    UpdateSave();
                }

                if (messagetext.StartsWith("/ip"))
                {
                    var words = messagetext.Split(' ');
                    if (words.Length == 2)
                    {
                        ip = words[1];
                        UpdateSave();
                    }
                }
            }

        }

        public override void UpdateBeforeSimulation()
        {
            _counter += 1;
            if (!MyAPIGateway.Utilities.IsDedicated)
            {
                if (StarOutMat != MatrixD.Identity)
                {
                    MySimpleObjectDraw.DrawTransparentSphere(ref StarOutMat,30, ref OutgateCol,MySimpleObjectRasterizer.Solid,
                        40,intensity:0.01f);
                }

                if (StarInMat != MatrixD.Identity)
                {
                    MySimpleObjectDraw.DrawTransparentSphere(ref StarInMat, 30, ref IngateCol, MySimpleObjectRasterizer.Solid,
                        40, intensity:0.01f);
                }
            }
            if (_counter%60 == 0)
            {
                allP.Clear();
                MyAPIGateway.Multiplayer.Players.GetPlayers(allP);
            }


            for (int i = 0; i < allP.Count; i++)
            {
                if (StarOut != Vector3D.Zero)
                {
                    if ((allP[i].GetPosition() - StarOut).LengthSquared() <= 100)
                    {
                        if (allP[i].Controller.ControlledEntity.Entity is IMyCharacter)
                        {
                            if (allP[i].SteamUserId == MyAPIGateway.Multiplayer.MyId)
                            {
                                MyAPIGateway.Multiplayer.JoinServer(ip);
                                allP[i].Character.Close();
                            }

                        }
                        else
                        {
                            if (allP[i].Controller.ControlledEntity.Entity is IMyCockpit outPit)
                            {
                                allBlocks.Clear();
                                seats.Clear();
                                groupOut.Clear();
                                outPit.CubeGrid.GetBlocks(allBlocks);
                                foreach (var block in allBlocks)
                                {
                                    if (block.FatBlock != null && block.FatBlock is IMyCockpit cockpit)
                                    {
                                        seats.Add(cockpit);
                                        if (cockpit.IsMainCockpit)
                                        {
                                            if (cockpit.Pilot != null)
                                            {
                                                mainPilot = cockpit.Pilot.DisplayName;
                                            }
                                        }
                                    }
                                }

                                if (mainPilot == "")
                                {
                                    mainPilot = outPit.Pilot.DisplayName;
                                }


                                if (outPit.CubeGrid.CustomName == mainPilot)
                                {
                                    for (int j = 0; j < seats.Count; j++)
                                    {
                                        groupOut.Add(seats[j].Pilot);
                                        seats[j].RemovePilot();
                                    }

                                    foreach (var person in groupOut)
                                    {
                                        if (person.DisplayName == MyAPIGateway.Multiplayer.MyName)
                                        {
                                            MyAPIGateway.Multiplayer.JoinServer(ip);
                                            person.Kill();
                                        }
                                    }

                                    for (int j = 0; j < allBlocks.Count; j++)
                                    {
                                        outPit.CubeGrid.RemoveBlock(allBlocks[j],true);
                                    }
                                }

                            }
                        }
                    }
                }

                if (StarIn != Vector3D.Zero)
                {
                    if ((allP[i].GetPosition() - StarIn).LengthSquared() <= 10000 && !blacklist.Contains(allP[i].DisplayName))
                    {
                        MyVisualScriptLogicProvider.SpawnLocalBlueprintInGravity(allP[i].DisplayName,StarIn,0f,0f);
                        blacklist.Add(allP[i].DisplayName);
                    }
                }
                
            }

            if (_counter%3600 == 0)
            {
                blacklist.Clear();
            }
            
        }

        protected override void UnloadData()
        {
            MyAPIGateway.Utilities.MessageEntered -= UtilitiesOnMessageEntered;
            MyAPIGateway.Entities.OnEntityAdd -= EntitiesOnOnEntityAdd;
            entAllblocks = null;
            blacklist = null;
            allP = null;
            seats = null;
            allBlocks = null;
            groupOut = null;
        }
    }
}