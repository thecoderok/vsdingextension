using Microsoft.VisualStudio.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace VitaliiGanzha.VsDingExtension
{
    public class Players : IDisposable
    {
        private readonly Dictionary<EventType, IList<SoundPlayer>> eventTypeToSoundPlayerMapping;

        public Players()
        {
            eventTypeToSoundPlayerMapping = new Dictionary<EventType, IList<SoundPlayer>>();
            eventTypeToSoundPlayerMapping[EventType.BuildCompleted] = new List<SoundPlayer>() { new SoundPlayer(Resources.build) };
            eventTypeToSoundPlayerMapping[EventType.BreakpointHit] = new List<SoundPlayer>() { new SoundPlayer(Resources.debug) };
            eventTypeToSoundPlayerMapping[EventType.TestsCompletedSuccess] = new List<SoundPlayer>() { new SoundPlayer(Resources.ding) };
            eventTypeToSoundPlayerMapping[EventType.TestsCompletedFailure] = new List<SoundPlayer>() { new SoundPlayer(Resources.ding) }; // TODO: different sound for failed tests
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
        BuildCompleted,
        BreakpointHit,
        TestsCompletedSuccess,
        TestsCompletedFailure
    }
}
