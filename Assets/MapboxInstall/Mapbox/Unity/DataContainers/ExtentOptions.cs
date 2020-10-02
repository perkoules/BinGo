﻿namespace Mapbox.Unity.Map
{
    public interface ITileProviderOptions
    {
    }

    public interface ICameraBoundsExtentOptions : ITileProviderOptions
    {
        void SetOptions();
    }

    public class ExtentOptions : ITileProviderOptions
    {
        public virtual void SetOptions(ExtentOptions extentOptions)
        {
        }
    }
}