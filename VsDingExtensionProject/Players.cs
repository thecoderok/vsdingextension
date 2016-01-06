namespace VitaliiGanzha.VsDingExtension
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Media;
    using Microsoft.VisualStudio.Shell;

    public sealed class Players : IDisposable
    {
        private readonly SoundsSelectOptionsPage overridesSettings;
        private Dictionary<EventType, IList<SoundPlayer>> eventTypeToSoundPlayerMapping;

        public Players(SoundsSelectOptionsPage overridesSettings)
        {
            this.overridesSettings = overridesSettings;
            this.SetupSounds();
        }

        private void SetupSounds()
        {
            // Add regular sounds
            var tempMapping = new Dictionary<EventType, IList<SoundPlayer>>();
            tempMapping[EventType.BuildCompleted] = new List<SoundPlayer>() { new SoundPlayer(Resources.build) };
            tempMapping[EventType.BreakpointHit] = new List<SoundPlayer>() { new SoundPlayer(Resources.debug) };
            tempMapping[EventType.TestsCompletedSuccess] = new List<SoundPlayer>()
            {
                new SoundPlayer(Resources.ding)
            };
            tempMapping[EventType.TestsCompletedFailure] = new List<SoundPlayer>()
            {
                new SoundPlayer(Resources.test_failed)
            };

            // Add custom sounds
            this.AddSoundOverrideIf(tempMapping, this.overridesSettings.OverrideOnBuildSound, this.overridesSettings.CustomOnBuildSoundLocation, EventType.BuildCompleted);
            this.AddSoundOverrideIf(tempMapping, this.overridesSettings.OverrideOnBreakpointHitSound, this.overridesSettings.CustomOnBreakpointHitSoundLocation, EventType.BreakpointHit);
            this.AddSoundOverrideIf(tempMapping, this.overridesSettings.OverrideOnTestCompleteFailureSound, this.overridesSettings.CustomOnTestCompleteFailureSoundLocation, EventType.TestsCompletedFailure);
            this.AddSoundOverrideIf(tempMapping, this.overridesSettings.OverrideOnTestCompleteSuccesSound, this.overridesSettings.CustomOnTestCompleteSuccesSoundLocation, EventType.TestsCompletedSuccess);

            this.eventTypeToSoundPlayerMapping = tempMapping;
        }

        private void AddSoundOverrideIf(Dictionary<EventType, IList<SoundPlayer>> tempMapping, bool shouldAdd, string fileLocation, EventType eventType)
        {
            if (shouldAdd)
            {
                if (!string.IsNullOrWhiteSpace(fileLocation) &&
                    File.Exists(fileLocation))
                {
                    tempMapping[eventType].Insert(0, new SoundPlayer(fileLocation));
                }
            }
        }

        public void SoundSettingsChanged()
        {
            this.SetupSounds();
        }

        public void PlaySoundSafe(EventType eventType)
        {
            foreach (var soundPlayer in this.eventTypeToSoundPlayerMapping[eventType])
            {
                try
                {
                    soundPlayer.Play();

                    // When sound played, break.
                    // Attempt to play using another player only in case of failure (file gets deleted, etc.)
                    break; 
                }
                catch (Exception ex)
                {
                    ActivityLog.LogError(GetType().FullName, ex.Message);
                }
            }
        }

        private void SafeDispose(SoundPlayer soundPlayer)
        {
            try
            {
                soundPlayer.Dispose();
            }
            catch (Exception ex)
            {
                ActivityLog.LogError(this.GetType().FullName, "Error when disposing player: " + ex.Message);
            }
        }

        public void Dispose()
        {
            foreach (var players in this.eventTypeToSoundPlayerMapping.Values)
            {
                foreach (var player in players)
                {
                    this.SafeDispose(player);
                }
            }
        }
    }

    public enum EventType
    {
        None,
        BuildCompleted,
        BreakpointHit,
        TestsCompletedSuccess,
        TestsCompletedFailure
    }
}
