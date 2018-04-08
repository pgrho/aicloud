namespace Shipwreck.AICloud
{
    public class SynthesisSpeechParameter
    {
        public string Text { get; set; }
        public string SpeakerName { get; set; }
        public string SpeakerPassword { get; set; }

        public InputType InputType { get; set; }

        public float Volume { get; set; } = 1;
        public float Speed { get; set; } = 1;
        public float Pitch { get; set; } = 1;
        public float Range { get; set; } = 1;

        public bool UseDictionary { get; set; }

        public OutputType OutputType { get; set; }

        public Extension Extension { get; set; }

        public SamplingRate WavSamplingRate { get; set; }
        public BitDepth WavBitDepth { get; set; }
        public Channels WavChannels { get; set; }

        public float Joy { get; set; }

        public float Sadness { get; set; }

        public float Anger { get; set; }
    }
}