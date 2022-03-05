using UnityEngine;
using System.Collections;

public class CustomHorScroll : CustomScroll
{
    protected override void Awake() {
        horizontal = true;
        vertical = false;
        base.Awake();
    }

    protected override float GetDimension(Vector2 vector) {
        return vector.x;
    }

    protected override Vector2 GetVector(float value) {
        return new Vector2(value, 0);
    }

    protected override float GetPos(Vector2 vector) {
        return vector.x;
    }
}
