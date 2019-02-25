## CHANGELOG

### 1.1.0
- The part name is now the same as the one on the right click menu (ex. : Mk1 Command Pod) and is on the window title.
- Added new informations on the collider bounds of the current part regarding its position, extends and size.
- Added new informations on the distance between the current part and the part under the cursor.
  Also we have the distance between collider bounds and if they intersect.
  This is useful to measure the X, Y or Z distance between 2 parts.

### 1.0.3
- Fixed a mirror symmetry counterpart bug.
- Tweakable fields are now found dynamically instead of having a fixed list (Thanks to Canisin for that contribution).
- Position and Rotation become Absolute Position and Absolute Rotation.

### 1.0.2
- Support for KSP-AVC.
- Fixed a NullReferenceException when used with mods that change part modules. Window was not opening in that case.
- Do not reset Part Edition Window position when pressing P on a part while the Part Edition Window is already opened.

### 1.0.1
- Adjusted keyboard control lock.
- Fixed the Z SetRotation rotating around the Y axis.
- Update editor gizmo position and rotation.
- Fixed a mirror symmetry rotation bug.
- Fixed a NullReferenceException when clicking the new or load craft button while the Part Edition Window was open.

### 1.0.0
- Recompiled for KSP 1.6.0
- Part Edition Window can edit Authority Limiter, Thrust Limiter, Gimbal Limit, Decouple Force Percent, Deploy Limit, Light RGB,
  Parachute Min Pressure, Altitude, Spread Angle, Wheels Brakes, Friction, Spring, Damper, Drive Limiter, Traction Control with
  greater precision.

### 0.2.0
- Recompiled for KSP 1.5.1

### 0.1.2
- Updated Licence to GPL 3.0

### 0.1.1
- Fixed a Null Reference Exception when the part in the Part Edition Window was deleted. Now the Part Edition Window is automatically closed if its part is detached or the vessel root is deleted.

### 0.1.0
- First release