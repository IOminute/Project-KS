using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedState : UnitBaseState
{
    public UnitSelectedState(UnitController unit) : base(unit) { }

    public override void Enter()
    {

    }

    public override void Update()
    {
        if (unit.IsDie) return;

        unit.MoveTo(unit.ClickedPosition);

        if (!unit.IsSelected || Vector3.Distance(unit.transform.position, unit.ClickedPosition) <= 1.5f)
        {
            unit.ChangeState(new UnitIdleState(unit));
        }
    }
}
