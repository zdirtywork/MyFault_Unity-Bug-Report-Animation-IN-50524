using System.Collections;
using UnityEngine;

// About this issue:
// 
// The Animator.angularVelocity property is not working correctly.
// 
// The rotation data given by the `Animator.angularVelocity` property does not match
// the rotation data given by the `Animator.deltaRotation` property.
// When manually calculating rotation using the `Animator.angularVelocity` property,
// the character's Y-axis always gradually aligns with the world space X-axis, Y-axis, or Z-axis,
// ultimately becoming parallel to it.
// 
// Furthermore, the character using Animator.angularVelocity will immediately become
// different from its initial position and rotation upon entering Play mode.
// This is also an abnormal behavior.
// 
// How to reproduce:
// 
// 1. Open the "Sample" scene.
// 2. Enter Play mode.
// 3. Observer the characters in the Scene view.
// 
// Expected result: The characters always remain in the same position and rotation.
// Actual result: The characters, when manually rotated, gradually align with the world space X, Y, or Z axis.

public enum ApplyMode : byte
{
    BuiltIn,
    DeltaValue,
    Manual,

    // When entering Play mode in Manual mode directly, the character immediately deviates
    // from its original position, making it inconvenient for comparison.
    // Therefore, you can choose to delay for one second before switching to Manual mode.
    BuiltInThenManual
}

[RequireComponent(typeof(Animator))]
public class ApplyRootMotion : MonoBehaviour
{
    public ApplyMode applyMode;

    private Animator _animator;
    private bool _waiting;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.applyRootMotion = true;

        if (applyMode == ApplyMode.BuiltInThenManual)
        {
            StartCoroutine(ChangeToManualMode());
        }
    }

    private void OnAnimatorMove()
    {
        switch (applyMode)
        {
            case ApplyMode.BuiltIn:
                _animator.ApplyBuiltinRootMotion();
                break;

            case ApplyMode.DeltaValue:
                transform.rotation *= _animator.deltaRotation;
                transform.position += _animator.deltaPosition;
                break;

            case ApplyMode.Manual:
                var deltaTime = Time.unscaledDeltaTime;
                var rotationAngle = _animator.angularVelocity * Mathf.Rad2Deg * deltaTime;
                transform.Rotate(rotationAngle, Space.World);
                var motionOffset = _animator.velocity * deltaTime;
                transform.Translate(motionOffset, Space.World);
                break;

            case ApplyMode.BuiltInThenManual:
                if (_waiting)
                {
                    goto case ApplyMode.BuiltIn;
                }
                else
                {
                    goto case ApplyMode.Manual;
                }
        }
    }

    private IEnumerator ChangeToManualMode()
    {
        _waiting = true;
        yield return new WaitForSeconds(1);
        _waiting = false;
    }
}
