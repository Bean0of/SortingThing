using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace SortingThing.Audio;

public static class Oscillator
{
    private const float MinFrequency = 55;
    private const float MaxFrequency = 1100;

    public static WaveOutEvent Wave;
    public static SignalGenerator Signal;

    static Oscillator()
    {
        Signal = new()
        {
            Frequency = MinFrequency,
            Gain = 0.2,
            Type = SignalGeneratorType.SawTooth
        };

        Wave = new();
        Wave.DesiredLatency = 50;
        Wave.Init(Signal);
    }

    public static void Play()
    {
        Wave.Play();
    }

    public static void Stop()
    {
        Wave.Stop();
    }

    public static void UpdateFrequency(int i, SortingArray array)
    {
        Signal.Frequency = Raymath.Lerp(MinFrequency, MaxFrequency, i / (float)array.Length);
    }
}