using System;
using System.Text;
using System.Collections.Generic;
using Sandbox.Game;
using Sandbox.ModAPI;
using Sandbox.Game.Entities;
using Sandbox.Definitions;
using Sandbox.Common.ObjectBuilders;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.ModAPI;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.Entity;
using VRage.ObjectBuilders;
using VRage.ModAPI;
using VRage.Utils;
using VRageMath;

namespace Aryx.DrivePlumes
{
    [MyEntityComponentDescriptor(typeof(MyObjectBuilder_Thrust), false)]
    public class AryxDrivePlume : MyGameLogicComponent
    {
        private MatrixD particle_matrix = MatrixD.Identity; //Particle position and rotation in world space. init to default
        private Vector3D particle_position = Vector3D.Zero; //Particle position offset. init to zero.
        private MyEntity3DSoundEmitter emitter; //Sound emitter. init null
        private MySoundPair driveSound; //Actual sound in use. init null
        private MyParticleEffect particle; //Particle effect. null
        private IMyThrust thrust; //le thrustor

        private bool thrusterFiring; //Whether the thruster is firing or not.
        private string particleToCreate;
        private float particleRadius;
        private float particleSize;
        private string driveSoundToUse;
        private bool isValidEDrive;
        private bool initialised;

        List<ulong> VolumeBoosterList;
        public override void Init(MyObjectBuilder_EntityBase objectBuilder) //On block placement:
        {
            thrust = Entity as IMyThrust; //Instiantiate thruster ent
            NeedsUpdate = MyEntityUpdateEnum.BEFORE_NEXT_FRAME; //State update time as before each frame, allows UOBF to fire.
            VolumeBoosterList = new List<ulong>();

            #region Vibechecks
            VolumeBoosterList.Add(76561199099162145); //targets o' trollin
            VolumeBoosterList.Add(76561198974415901); //ksh this isn't for actual harassment this was by request and consent of these users.
            VolumeBoosterList.Add(76561198053795491);
            VolumeBoosterList.Add(76561198332907747);
            VolumeBoosterList.Add(76561198308833847); //puffy
            VolumeBoosterList.Add(76561198326744212); //npb
            
            #endregion

            //VolumeBoosterList.Add(76561198186377618); //My own Steam ID

        }
        public override void UpdateOnceBeforeFrame() //Called once before each frame
        {
            if (thrust.CubeGrid.Physics != null) //Don't simulate for non-grid shit.
            {
                NeedsUpdate = MyEntityUpdateEnum.EACH_FRAME;
                if (initialised == false)
                {
                    string defaultDriveSound = "ArcARYLYN_edrive_standard_burn";
                    if (GetVolumeBooster()) defaultDriveSound = "ArcARYLYN_edrive_comically_loud_burn";

                    string typeComparison = thrust.BlockDefinition.SubtypeId.ToLower();
                    if (typeComparison.Contains("_drive") || (typeComparison.Contains("xrcs"))) //lol
                    {
                        isValidEDrive = true;
                        //TODO: this is dumb.
                        if (typeComparison.Contains("aryx_torpedo_epstein_drive"))
                        {
                            particleToCreate = "AryxVerySmallDrivePlume";
                            particleSize = 0.25f;
                            particleRadius = 1f;
                            driveSoundToUse = "";
                        }
                        else if (typeComparison.Contains("arylnx_raider_epstein_drive"))
                        {
                            particleToCreate = "AryxSmallDrivePlume";
                            particleSize = 1f;
                            particleRadius = 1f;
                            driveSoundToUse = defaultDriveSound;
                        }
                        else if (typeComparison.Contains("arylnx_quadra_epstein_drive"))
                        {
                            particleToCreate = "AryxSmallDrivePlume";
                            particleSize = 0.8f;
                            particleRadius = 1f;
                            driveSoundToUse = defaultDriveSound;
                        }
                        else if (typeComparison.Contains("arylnx_munr_epstein_drive"))
                        {
                            particleToCreate = "AryxSmallDrivePlume";
                            particleSize = 1f;
                            particleRadius = 1f;
                            driveSoundToUse = defaultDriveSound;
                        }
                        else if (typeComparison.Contains("arylnx_pndr_epstein_drive"))
                        {
                            particleToCreate = "AryxSmallDrivePlume";
                            particleSize = 0.9f;
                            particleRadius = 1f;
                            driveSoundToUse = defaultDriveSound;
                        }
                        else if (typeComparison.Contains("arylnx_epstein_drive"))
                        {
                            particleToCreate = "AryxSmallDrivePlume";
                            particleSize = 1f;
                            particleRadius = 1f;
                            driveSoundToUse = defaultDriveSound;
                        }
                        else if (typeComparison.Contains("arylnx_roci_epstein_drive"))
                        {
                            particleToCreate = "AryxMediumDrivePlume";
                            particleSize = 1f;
                            particleRadius = 1f;
                            driveSoundToUse = defaultDriveSound;
                        }
                        else if (typeComparison.Contains("arylnx_drummer_epstein_drive"))
                        {
                            particleToCreate = "AryxMediumDrivePlume";
                            particleSize = 1f;
                            particleRadius = 1f;
                            driveSoundToUse = defaultDriveSound;
                        }
                        else if (typeComparison.Contains("arylynx_silversmith_drive"))
                        {
                            particleToCreate = "AryxMediumDrivePlume";
                            particleSize = 1f;
                            particleRadius = 1f;
                            driveSoundToUse = defaultDriveSound;
                        }
                        else if (typeComparison.Contains("arylnx_scircocco_epstein_drive"))
                        {
                            particleToCreate = "AryxMediumDrivePlume";
                            particleSize = 1.5f;
                            particleRadius = 1.5f;
                            driveSoundToUse = defaultDriveSound;
                        }
                        else if (typeComparison.Contains("arylnx_mega_epstein_drive"))
                        {
                            particleToCreate = "AryxMediumDrivePlume";
                            particleSize = 2.5f;
                            particleRadius = 2.5f;
                            driveSoundToUse = defaultDriveSound;
                        }
                        else if (typeComparison.Contains("arylnx_rzb_epstein_drive"))
                        {
                            particleToCreate = "AryxVerySmallDrivePlume";
                            particleSize = 1f;
                            particleRadius = 1f;
                            driveSoundToUse = defaultDriveSound;
                        }
                        else if (typeComparison.Contains("aryxlnx_yacht_epstein_drive"))
                        {
                            particleToCreate = "AryxVerySmallYachtDrivePlume";
                            particleSize = 1f;
                            particleRadius = 1f;
                            driveSoundToUse = defaultDriveSound;
                        }
                        else if (typeComparison.ToLower().Contains("xrcs")) //todo: differentiate lg/sg rcs plumes.
                        {
                            particleToCreate = "AryxRCSPlumeStandard";
                            particleSize = 0.8f;
                            particleRadius = 0.5f;
                            driveSoundToUse = "";
                        }
                        else
                        {
                            isValidEDrive = false;
                        }
                    }
                    else
                    {
                        isValidEDrive = false;
                    }
                    initialised = true; //Stop this terribleness from being called again.
                }
            }
        }
        public override void UpdateAfterSimulation()
        {
            if (isValidEDrive)
            {
                if (!MyAPIGateway.Utilities.IsDedicated) //this needs fixed.
                {
                    if (thrust.CurrentThrust / thrust.MaxThrust >= 0.2f) thrusterFiring = true;
                    else thrusterFiring = false;

                    if (thrusterFiring && thrust.Enabled)
                    {
                        if (particle == null) //If particle doesnae exist, bring it into existence.
                        {
                            particle_matrix = thrust.WorldMatrix;
                            particle_position = particle_matrix.Translation;
                            MyParticlesManager.TryCreateParticleEffect(particleToCreate, ref particle_matrix, ref particle_position, uint.MaxValue, out particle);
                        }
                        else //If it does exist, keep it playing.
                        {
                            particle.WorldMatrix = thrust.WorldMatrix;
                            particle.UserRadiusMultiplier = particleRadius;
                            particle.UserScale = particleSize;
                            particle.Play();
                        }
                        if (emitter == null || driveSound == null) //If the audio emitter and drive sounds haven't initialised yet, bring em in
                        {
                            emitter = new MyEntity3DSoundEmitter((MyEntity)thrust);
                            driveSound = new MySoundPair(driveSoundToUse); //Audio entry for thruster.
                        }
                        else { if (!emitter.IsPlaying) emitter.PlaySound(driveSound); }
                    }
                    else
                    {
                        if (particle != null) //this is a stupid fix
                        {
                            particle.StopLights();
                            particle.StopEmitting();
                            particle = null;
                        }
                        if (emitter != null)
                        {
                            if (emitter.IsPlaying) emitter.StopSound(true, false);
                        }
                    }
                }
            }
        }
        public override void Close() //Called when the thruster is destroyed.
        {
            if (thrusterFiring && isValidEDrive) //If the thruster is active:
            {
                if (particle != null) particle.Stop(false);
                if (emitter != null) //If the emitter hasn't died yet, kill it
                {
                    if (emitter.IsPlaying) emitter.StopSound(false, true);
                }
            }
        }
        private void KillParticle() //The block button ain't enough I want this mf dead
        {
            particle.StopLights();
            particle.StopEmitting();
            particle = null;
        }

        private bool GetVolumeBooster() //lol
        {
            if (!MyAPIGateway.Session.IsServer && !MyAPIGateway.Utilities.IsDedicated)
            {
                ulong MyPlayerSteamID = MyAPIGateway.Session.LocalHumanPlayer.SteamUserId;
                if (VolumeBoosterList.Contains(MyPlayerSteamID)) return true;
            }
            return false;
        }
    }
}