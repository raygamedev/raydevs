namespace Raydevs
{
    public enum CollectableType
    {
        PythonSword,
        SudoHammer,
        ReactThrowingStar,
    }
    public interface ICollectable
    {
        void SetCollectableInteracted(CollectableType interacted);
    }
}