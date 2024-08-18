public abstract class UnitBaseState
{
    protected UnitController unit;

    public UnitBaseState(UnitController unit)
    {
        this.unit = unit;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}