using Sandbox.ModAPI;
using VRage.Game;
using VRage.Game.Components;

namespace ShipPoints
{
    [MySessionComponentDescriptor(MyUpdateOrder.NoUpdate)]
    class PointAdditions : MySessionComponentBase
    {
        public override void Init(MyObjectBuilder_SessionComponent sessionComponent)
        {
            MyAPIGateway.Utilities.SendModMessage(2546247, MyAPIGateway.Utilities.SerializeToBinary(@"
				LargeBlockBatteryBlock@15;
				LargeBlockLargeGenerator@300;
				LargeBlockSmallGenerator@30;

				ARYXAtlasPDC@75;
				C100mmTurret@100;
				C200mmTurret@350;
				C203mmSingleTurret@125;
				C300mmTurret@1000;
				C400mmTurret@1000;
				C500mmTurret@1250;
				LargeCalibreTurret@400;
				DualBarrelArtillery@500;
				PrototypeSiegeArtillery@1500;
				ARYXSiegeMortarCannon@750;
				ARYXCycloneCannon@500;
				ARYXHurricaneCannon@800;
				ARYXTsunamiCannon@800;
				ARYXTempestCannon@650;
				ARYXTyphoonCannon@1250;
				ARYXSlantedAtlasPDC@75;
				MK1BattleshipGun_Block@450;
				MK2_Battleship_Block@750;
				MK3_Battleship_Block@1150;
				QuadArtilleryBase@750;

				RailgunxTurretM@325;
				RailgunxTurret@400;
				RailgunxTurretS@250;
				Coil250@350;
				ARYXHeavyCoilgun@850;
				ARYXPicketRailgun@250;
				ARYXRailgunTurret@375;
				ARYXRailgun@275;
				ARYXLightCoilgun@550;
				CoilgunMk2_Block@75;
				MK1Railgun_Block@250;
				MK2_Railgun_Block@350;
				MK3_Railgun_Block@550;
				LargeRailgun@250;
				ARYXKingswordSupercannon@10000;
				ARYXGaussTurret@4500;
				ARYXGaussCannon@1250;

				Torp_Block@300;
				ARYXFlechetteLauncher@250;
				ARYXHeavyMissileSalvoLauncher@2000;
				HeavyTorpedoLauncher@2000;
				ARYXMissileBattery@475;
				MA_Derecho@300;
				ARYXLongbowLauncher@150;
				ARYXGladiatorMissileLauncher@300;
				ARYXInterceptorPDGun@125;
				ARYXRocketArtillery@1750;
				ARYXHydraTurret@250;
				ARYX_Small_Sidekick_Hangar@225;
				ARYXTorpLauncher@325;
				ARYXInlineTorpLauncher@325;
				ARYXHeavyTorpedoLauncher@350;
				VMLS_Block@275;
				LightMissileLauncher@400;

				CIWS@125;
				ARYX_ChordAutocannon@175;
				ARYX_EchoAutocannon@250;
				LargeBlockMediumCalibreTurret@150;
				ARYXBurstTurret@175;
				ARYXBurstTurretSlanted@175;
				MA_Crouching_Tiger@250;
				IronMaiden_Block@225;
				ARYXWarriorGatling@150;
				ARYXWarriorGatlingGun@150;
				ARYXCodexPDC@100;
				ARYXSlantedCodexPDC@100;
				QuadBarrelAutocannon@450;
				ARYXFlakTurret@100;
				MA_AC150@275;
				MA_AC150_Gantry@275;
				MA_AC150_30@275;
				MA_AC150_45@275;
				MA_AC150_45_Gantry@275;
				ARYXSentinel@100;
				MA_Tiger@250;
				PDCTurret_Block@100;

				ARYXAuroraLaser@75;
				AMSlaser@75;
				ARYXOculusLaserBase@75;
				MA_PDX@100;
				ARYXNucleonShotgun@150;
				MA_T2PDX@150;
				MA_T2PDX_Slope@150;
				MA_T2PDX_Slope2@150;
				ARYXPulseTurret@150;
				ARYXCompactPulseTurret@150;
				Laser_Block@175;
				ARYXPulsarCannon@175;
				ARYX_FW_NovaBlaster@200;
				ARYXPlasmaPulser@200;
				ARYX_FocusBeam_CompactTurret@200;
				ARYXVariableLaser@225;
				ARYXSpartanTurret@375;
				HeavyPulse@265;
				MA_Gladius@375;
				ARYXFocusLance@250;
				AWE_MarkV_SuperLaser_Large_CB@10000;
				ARYXPlasmaBeamCannon@600;
				ARYXMagnetarCannon@550;
				ARYXReaperPulseCannon@600;
				ARYXPhaseRepeaterCannon@500;

				ARYXLargeIonCannon@425;
				ARYXLargeRadar@75;
				TestEWAR_One@125;
				MediumEWARBase@250;
				ARYX_LargeFlareLauncher@100;
				B8T_Flare@100;
				ARYXMace@150;
				ARYXHydraTurret@340;

				ARYLYNX_SILVERSMITH_DRIVE@100;
				ARYLNX_QUADRA_Epstein_Drive@25;
				LargeBlockSmallHydrogenThrust@5;
				LargeBlockLargeHydrogenThrust@25;
				ARYLNX_RAIDER_Epstein_Drive@50;
				ARYLNX_DRUMMER_Epstein_Drive@250;
				ARYLNX_PNDR_Epstein_Drive@25;
				ARYLNX_Mega_Epstein_Drive@350;
				ARYLNX_Epstein_Drive@50;
				ARYLNX_ROCI_Epstein_Drive@75;
				ARYLNX_SCIRCOCCO_Epstein_Drive@175;
				ARYLNX_MUNR_Epstein_Drive@25;
				
				LargeBlockLargeThrust@1;
				LargeBlockSmallThrust@1;
				LargeBlockLargeThrustSciFi@1;
				LargeBlockSmallThrustSciFi@1;
				LargeBlockLargeModularThruster@1;
				LargeBlockSmallModularThruster@1;








				BlinkDriveLarge@250;
				LargeJumpDrive@250;
				MA_Afterburner_Large_small@100;
				MA_Afterburner_Large_5x@200;
				Afterburner_LG_Ion_Large@50;
				LynxRcsThruster1@5;
				AryxRCS@5;
				AryxRCSSlant@5;
				AryxRCSHalfRamp@5;
				AryxRCSRamp@5;

			"));
        }
    }
}
