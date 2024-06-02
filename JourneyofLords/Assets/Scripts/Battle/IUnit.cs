public interface IUnit
{
    int Health { get; set; }
    int Atk { get; }
    int Def { get; }
    bool IsAlive();
}