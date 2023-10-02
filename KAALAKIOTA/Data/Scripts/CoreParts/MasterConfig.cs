
namespace Scripts
{
    partial class Parts
    {
        internal Parts()
        {
            // naming convention: WeaponDefinition Name
            //
            // Enable your definitions using the follow syntax:
            // PartDefinitions(Your1stDefinition, Your2ndDefinition, Your3rdDefinition);
            // PartDefinitions includes both weapons and phantoms
            PartDefinitions(Coil250,HeavyPulse,QuadArtilleryBase,QuadBarrelAutocannon,B8T_Flare,HeavyTorpedoLauncher,DualBarrelArtillery,PrototypeSiegeArtillery,LightMissileLauncher,HeavyAssaultMissileLauncher);
            ArmorDefinitions();
            SupportDefinitions();
            UpgradeDefinitions();
        }
    }
}
