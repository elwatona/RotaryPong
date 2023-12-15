namespace RotaryPong
{
    public interface State
    {
        State SetState(PlayerController player);
        void Movement(PlayerMovement player);
        void Rotation(PlayerMovement player);
    }
}