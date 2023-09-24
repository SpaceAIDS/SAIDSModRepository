@BlockID "PDCTurret_Block"
@Version 2
@Author Krynoc
@Weaponcore 0

#Valid subpart ID's of PDCTurret_Block
#|  PDCTurretElevator
#|  |  PDCTurretAZ
#|  |  |  PDCTurretEL
#|  |  |  |  PDCBarrelSpin
#|  PDCTurretLeftDoor
#|  PDCTurretRightDoor

# Declarations
using Elevator as Subpart("PDCTurretElevator")
using Azimuth as Subpart("PDCTurretAZ") parent Elevator
using Elevation as Subpart("PDCTurretEL") parent Azimuth
using Barrel as Subpart("PDCBarrelSpin") parent Elevation

using DoorLeft as Subpart("PDCTurretLeftDoor")
using DoorRight as Subpart("PDCTurretRightDoor")

var isWorking = true


# Reset Functions
func DoorReset() {
	API.StopDelays()
	DoorLeft.reset()
	DoorRight.reset()
	Elevator.reset()
	Barrel.reset()
}
func DoorResetClose() {
	DoorReset()
	DoorRight.Translate([0, -1.1, 0], 0, Instant).Rotate([0, 0, 1], 90, 0, Instant)
	DoorLeft.Translate([0, -1.1, 0], 0, Instant).Rotate([0, 0, 1], -90, 0, Instant)
	Elevator.Translate([0, 2.1, 0], 0, Instant)
	Barrel.Translate([0, 0, -0.4], 0, Instant)
}

# Animations
func GunTurnOn() {
	if (isWorking == false) {
		DoorResetClose()

		Barrel.delay(100).Translate([0, 0, 0.4], 60, Linear)
		Elevator.Translate([0, -2.1, 0], 120, Linear)

		DoorRight.Rotate([0, 0, 1], -90, 90, Linear).delay(90).Translate([0, 1.1, 0], 60, Linear)
		DoorLeft.Rotate([0, 0, 1], 90, 90, Linear).delay(90).Translate([0, 1.1, 0], 60, Linear)
	}
	isWorking = true
}
func GunTurnOff() {
	if (isWorking == true) {
		DoorReset()

		Azimuth.MoveToOrigin(50, InOutSine)
		Elevation.MoveToOrigin(90, InOutSine)

		Barrel.Translate([0, 0, -0.4], 60, Linear)
		Elevator.delay(80).Translate([0, 2.1, 0], 100, Linear)

		DoorRight.Translate([0, -1.1, 0], 100, Linear).delay(100).Rotate([0, 0, 1], 90, 100, Linear)
		DoorLeft.Translate([0, -1.1, 0], 100, Linear).delay(100).Rotate([0, 0, 1], -90, 100, Linear)
	}
	isWorking = false
}


# Events
Action Block() {
	Built() {
		isWorking = true
	}
	Working() {
		GunTurnOn()
	}
	NotWorking() {
		GunTurnOff()
	}
}






