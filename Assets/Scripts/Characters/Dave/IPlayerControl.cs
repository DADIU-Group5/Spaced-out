using UnityEngine;

public interface IPlayerControl
{
    void Aim(Vector3 point);

    // 0 = min power, 1 = max power
    void SetPower(float power);

    void Launch();
}
