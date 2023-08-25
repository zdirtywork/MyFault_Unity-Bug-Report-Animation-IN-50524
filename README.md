# [My Fault] Unity-Bug-Report-Animation-IN-50524

**The issue doesn't come from Animator.angularVelocity but from calling the wrong function in my code.**

> Resolution Notes: The issue doesn't come from Animator.angularVelocity but from calling the wrong function in the user code :
> ```csharp
> var rotationAngle = _animator.angularVelocity * Mathf.Rad2Deg * deltaTime;
> transform.Rotate(rotationAngle, Space.World);
> ```
> 
> This version of transform.Rotate() takes euler angles as parameters and not an axis and an angle. The user can fix the issue by using the appropriate function that takes an axis and an angle instead :
> ```csharp
> var deltaTime = Time.deltaTime / Time.timeScale; // Time.unscaledDeltaTime is not always accurate
> var rotationAngle = _animator.angularVelocity * Mathf.Rad2Deg * deltaTime;
> transform.Rotate(rotationAngle.normalized, rotationAngle.magnitude, Space.World);
> ```

## About this issue

The `Animator.angularVelocity` property is not working correctly.

The rotation data given by the `Animator.angularVelocity` property does not match the rotation data given by the `Animator.deltaRotation` property.
When manually calculating rotation using the `Animator.angularVelocity` property, the character's Y-axis always gradually aligns with the world space X-axis, Y-axis, or Z-axis, ultimately becoming parallel to it.

Furthermore, the character using Animator.angularVelocity will immediately become different from its initial position and rotation upon entering Play mode. This is also an abnormal behavior.

Please refer to the "ApplyRootMotion.cs".

## How to reproduce

1. Open the "Sample" scene.
2. Enter Play mode.
3. Observer the characters in the Scene view.

Expected result: The characters always remain in the same position and rotation.

Actual result: The characters, when manually rotated, gradually align with the world space X, Y, or Z axis.
