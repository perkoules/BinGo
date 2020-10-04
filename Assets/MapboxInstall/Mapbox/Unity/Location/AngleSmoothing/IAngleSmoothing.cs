namespace Mapbox.Unity.Location
{
    public interface IAngleSmoothing
    {
        void Add(double angle);

        double Calculate();
    }
}