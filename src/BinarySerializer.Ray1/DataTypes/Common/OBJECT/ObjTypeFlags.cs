using System;

namespace BinarySerializer.Ray1
{
    // TODO: This should probably be changed to a class like with RayEvts. Older versions use fewer bytes, some
    //       flags only exist in later versions and on Saturn it's reversed

    [Flags]
    public enum ObjTypeFlags
    {
        None = 0,

        Always = 1 << 0, // If true the game sets the pos to (-32000, -32000) on init
        Balle = 1 << 1, // Indicates if the object is TYPE_BALLE1 or TYPE_BALLE2
        NoCollision = 1 << 2, // Indicates if the object has no collision - does not include follow
        HitRay = 1 << 3, // Indicates if the object damages Rayman
        KeepActive = 1 << 4,
        DetectZone = 1 << 5, // Indicates if the detect zone should be set
        Flag_06 = 1 << 6,
        Boss = 1 << 7, // Indicates if the boss bar should show

        KeepLinkedObjectsActive = 1 << 8,
        Bonus = 1 << 9, // Indicates if the object can be collected and thus not respawn again
        BigRayHitKnockback = 1 << 10,
        RayDistMultisprCantchange = 1 << 11,
        InstantSpeedX = 1 << 12, // Indicates if the object x position should be changed by SpeedX in MOVE_OBJECT
        InstantSpeedY = 1 << 13, // Indicates if the object y position should be changed by SpeedY in MOVE_OBJECT
        SpecialPlatform = 1 << 14, // Indicates if DO_SPECIAL_PLATFORM should be called
        ReadCmds = 1 << 15, // Indicates if commands should be read for the object, otherwise the command is set to 30 (NOP)

        MoveOnBlock = 1 << 16, // Indicates if the object reacts to block types (tile collision), thus calling calc_btyp
        FallInWater = 1 << 17,
        BlocksRay = 1 << 18,
        JumpOnBlock = 1 << 19, // Indicates if obj_jump gets called when on a ressort (spring) block
        NoRayCollision = 1 << 20,
        KillIfOutsideActiveZone = 1 << 21,
        UturnOnBlock = 1 << 22,
        IncreaseSpeedX = 1 << 23,

        PoingCollisionSound = 1 << 24,
        Flag_19 = 1 << 25,
        StopMovingUpWhenHitBlock = 1 << 26,
        SwitchOff = 1 << 27,
        Flag_1C = 1 << 28,
        LinkRequiresGendoor = 1 << 29, // Indicates if the object requires a gendoor in the link group to be valid
        NoLink = 1 << 30, // Indicates that the object can't be linked
        Flag_1F = 1 << 31,
    }
}