using System;
using System.Windows;

namespace GenerationsLib.WPF.Themes
{
    public class SkinResourceDictionary : ResourceDictionary
    {
        public static Skin CurrentTheme { get; set; } = GetDefaultTheme();


        public SkinResourceDictionary() : base()
        {
            UpdateSource();
        }

        private static Skin GetDefaultTheme()
        {
            return Skin.Dark;
        }

        #region Themes
        private Uri _DarkSource;
        private Uri _LightSource;
        private Uri _BetaSource;
        private Uri _ShardSource;
        private Uri _CarJemSource;
        private Uri _GammaSource;
        private Uri _SparksSource;


        public Uri SparksSource
        {
            get { return _SparksSource; }
            set
            {
                _SparksSource = value;
                UpdateSource();
            }
        }

        public Uri GammaSource
        {
            get { return _GammaSource; }
            set
            {
                _GammaSource = value;
                UpdateSource();
            }
        }
        public Uri DarkSource
        {
            get { return _DarkSource; }
            set
            {
                _DarkSource = value;
                UpdateSource();
            }
        }
        public Uri LightSource
        {
            get { return _LightSource; }
            set
            {
                _LightSource = value;
                UpdateSource();
            }
        }
        public Uri BetaSource
        {
            get { return _BetaSource; }
            set
            {
                _BetaSource = value;
                UpdateSource();
            }
        }
        public Uri ShardSource
        {
            get { return _ShardSource; }
            set
            {
                _ShardSource = value;
                UpdateSource();
            }
        }
        public Uri CarJemSource
        {
            get { return _CarJemSource; }
            set
            {
                _CarJemSource = value;
                UpdateSource();
            }
        }
        #endregion

        #region General
        public void UpdateSource()
        {
            var val = GetSkin();
            if (val != null && base.Source != val)
                base.Source = val;
        }
        public Uri GetSkin()
        {
            switch (CurrentTheme)
            {
                case Skin.Light:
                    return LightSource;
                case Skin.Dark:
                    return DarkSource;
                case Skin.Beta:
                    return BetaSource;
                case Skin.Shard:
                    return ShardSource;
                case Skin.CarJem:
                    return CarJemSource;
                case Skin.Gamma:
                    return GammaSource;
                case Skin.Sparks:
                    return SparksSource;
                default:
                    return DarkSource;
            }
        }

        public static void ChangeSkin(Skin newSkin, System.Collections.ObjectModel.Collection<ResourceDictionary> AppResources)
        {
            CurrentTheme = newSkin;

            foreach (ResourceDictionary dict in AppResources)
            {

                if (dict is SkinResourceDictionary skinDict)
                    skinDict.UpdateSource();
                else
                    dict.Source = dict.Source;
            }
        }
        #endregion
    }
}
