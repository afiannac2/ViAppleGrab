using System.Speech.Synthesis;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;

namespace ViToolkit.SoundManagement
{
    public enum ViSoundState
    {
        Inactive = 0,
        FadeIn = 1,
        Active = 2,
        FadeOut = 3
    }

    public class Sound
    {
        #region SOUND EFFECT FIELDS AND PROPERTIES

        SoundEffect _effect;
        SoundEffectInstance _instance;
        ViSoundState _state = ViSoundState.Inactive;

        public float Volume
        {
            get { return _instance.Volume; }

            set
            {
                //The volume must be bounded between 0.0f and 1.0f
                if (value < 0.0f)
                    value = 0.0f;

                else if (value > 1.0f)
                    value = 1.0f;

                _instance.Volume = value;
            }
        }
        public float Pitch
        {
            get { return _instance.Pitch; }

            set
            {
                //The pitch must be bounded between -1.0f and 1.0f
                if (value > 1.0f)
                    value = 1.0f;

                else if (value < -1.0f)
                    value = -1.0f;

                _instance.Pitch = value;
            }
        }
        public bool IsPlaying
        {
            get
            {
                if (_instance.IsDisposed)
                    return false;
                else
                    return (_instance.State == SoundState.Playing);
            }
        }
        private bool _isLooped = false;
        public bool IsLooped
        {
            get
            {
                return _isLooped;
            }
            private set
            {
                _isLooped = value;
            }
        }
        public bool IsLooping
        {
            get
            {
                return (IsPlaying && IsLooped == true);
            }
        }
        public bool IsFadingIn
        {
            get
            {
                return _state == ViSoundState.FadeIn;
            }
        }
        public bool IsFadingOut
        {
            get
            {
                return _state == ViSoundState.FadeOut;
            }
        }

        private float maxVol;

        #endregion

        #region SOUND EFFECTS

        /// <summary>
        /// Constructs a sound object. Currently only handles .wav files
        /// </summary>
        /// <param name="_fileName">Name of the sound effect file</param>
        /// <param name="_cm">The game's content manager</param>
        public Sound(string _fileName, ContentManager _cm, float initialVol)
        {
            _effect = _cm.Load<SoundEffect>("Audio\\"+_fileName);
            _instance = _effect.CreateInstance();
            _instance.Stop();
            maxVol = initialVol;
            _instance.Volume = initialVol;
        }

        /// <summary>
        /// This function should only be called on sounds that are played in a loop and 
        /// need to be faded in or out
        /// </summary>
        public void UpdateFade()
        {
            switch(_state)
            {
                case ViSoundState.FadeIn:
                    Volume += 0.02f;
                    if (Volume >= maxVol)
                    {
                        _state = ViSoundState.Active;
                    }
                    break;

                case ViSoundState.FadeOut:
                    Volume -= 0.02f;
                    if (Volume <= 0.0f)
                    {
                        _instance.Stop();
                        _state = ViSoundState.Inactive;
                    }
                    break;
            }
        }

        public void BlockPlay()
        {
            _state = ViSoundState.Active;

            _instance.Play();

            while (_instance.State == SoundState.Playing) { }
        }

        public void PlayOnce()
        {
            _state = ViSoundState.Active;
            _instance.Play();
        }

        public void PlayLoop()
        {
            _state = ViSoundState.FadeIn;
            _instance.Volume = 0.0f;
            if (!IsLooped)
            {
                IsLooped = true;
                _instance.IsLooped = true;
            }
            _instance.Play();
        }

        public void Restart()
        {
            if(_instance.State == SoundState.Playing)
                _instance.Stop();

            PlayLoop();
        }

        public void Pause()
        {
            if (_instance.State == SoundState.Playing)
                _instance.Pause();
        }

        public void Resume()
        {
            _state = ViSoundState.Active;

            if (_instance.State == SoundState.Paused)
                _instance.Resume();
        }

        public void Stop()
        {
            if (IsPlaying && IsLooping)
                _state = ViSoundState.FadeOut;
            else
            {
                _state = ViSoundState.Inactive;
                _instance.Stop();
            }
        }

        public void StopNoFade()
        {
            if (_instance.State == SoundState.Playing)
            {
                _state = ViSoundState.Inactive;
                _instance.Stop();
            }
        }

        #endregion        
    }

    public static class Speech
    {
        #region SPEECH SYNTHESIS FIELDS

        private static SpeechSynthesizer spVoice = new SpeechSynthesizer();

        #endregion

        #region SPEECH SYNTHESIS

        public static void SpeakAsync(int Volume, string text)
        {
            //Volume must be bounded between 0 and 100
            Volume = Math.Min(Volume, 100);
            Volume = Math.Max(Volume, 0);

            spVoice.Volume = Volume;
            spVoice.SpeakAsync(text);
        }

        public static void Speak(int Volume, string text)
        {
            //Volume must be bounded between 0 and 100
            if (Volume > 100)
                Volume = 100;
            else if (Volume < 0)
                Volume = 0;

            spVoice.Volume = Volume;
            spVoice.Speak(text);
        }

        #endregion    
    }
}
