using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneManager : MonoBehaviour
{
    //TODO
    // Prevent mic playback
    private const int frequency = 48000;    // Wavelength
    private const int sampleCount = 1024;   // Sample Count.
    private const float refValue = 0.1f;    // RMS value for 0 dB.
    private const float threshold = 0.02f;  // Minimum amplitude to extract pitch
    private const float alpha = 0.05f;      // The alpha for the low pass filter

    public int recordedLength = 50;    // How many previous frames of sound are analyzed.
    public int requiedBlowTime = 4;    // How long a blow must last to be classified as a blow (and not a sigh for instance).
    public int clamp = 160;            // Used to clamp dB

    private float rmsValue;            // Volume in RMS
    private float dbValue;             // Volume in DB
    private float pitchValue;          // Pitch - Hz (is this frequency?)
    private int blowingTime;           // How long each blow has lasted

    private float lowPassResults;      // Low Pass Filter result
    private float peakPowerForChannel; //

    private float[] samples;           // Samples
    private float[] spectrum;          // Spectrum
    private List<float> dbValues;      // Used to average recent volume.
    private List<float> pitchValues;   // Used to average recent pitch.
    private AudioSource microphoneInput;

    private void Start()
    {
        samples = new float[sampleCount];
        spectrum = new float[sampleCount];
        dbValues = new List<float>();
        pitchValues = new List<float>();
        microphoneInput = GetComponent<AudioSource>();
        microphoneInput.loop = true;
        microphoneInput.playOnAwake = false;
        microphoneInput.mute = false;

        StartMicListener();
    }

    public void Update()
    {
        // If the audio has stopped playing, this will restart the mic play the clip.
        if (!microphoneInput.isPlaying)
        {
            StartMicListener();
        }

        // Gets volume and pitch values
        AnalyzeSound();

        // Runs a series of algorithms to decide whether a blow is occuring.
        DeriveBlow();
    }

    // Starts the Mic, and plays the audio back in (near) real-time.
    private void StartMicListener()
    {
        microphoneInput.clip = Microphone.Start(null, true, 1, AudioSettings.outputSampleRate);

        // HACK - Forces the function to wait until the microphone has started, before moving onto the play function.
        while (!(Microphone.GetPosition(null) > 0)) { }
        microphoneInput.Play();
    }

    // Analyzes the sound, to get volume and pitch values.
    private void AnalyzeSound()
    {
        // Get all of our samples from the mic.
        microphoneInput.GetOutputData(samples, 0);

        // Sums squared samples
        float sum = 0;
        for (int i = 0; i < sampleCount; i++)
        {
            sum += Mathf.Pow(samples[i], 2);
        }

        // RMS is the square root of the average value of the samples.
        rmsValue = Mathf.Sqrt(sum / sampleCount);
        dbValue = 20 * Mathf.Log10(rmsValue / refValue);

        // Clamp it to {clamp} min
        if (dbValue < -clamp)
        {
            dbValue = -clamp;
        }

        // Gets the sound spectrum.
        microphoneInput.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        float maxV = 0;
        int maxN = 0;

        // Find the highest sample.
        for (int i = 0; i < sampleCount; i++)
        {
            if (spectrum[i] > maxV && spectrum[i] > threshold)
            {
                maxV = spectrum[i];
                maxN = i; // maxN is the index of max
            }
        }

        // Pass the index to a float variable
        float freqN = maxN;

        // Interpolate index using neighbours
        if (maxN > 0 && maxN < sampleCount - 1)
        {
            float dL = spectrum[maxN - 1] / spectrum[maxN];
            float dR = spectrum[maxN + 1] / spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }

        // Convert index to frequency
        pitchValue = freqN * 24000 / sampleCount;
    }

    private void DeriveBlow()
    {
        UpdateRecords(dbValue, dbValues);
        UpdateRecords(pitchValue, pitchValues);

        // Find the average pitch in our records (used to decipher against whistles, clicks, etc).
        float sumPitch = 0;
        foreach (float num in pitchValues)
        {
            sumPitch += num;
        }
        sumPitch /= pitchValues.Count;

        // Run our low pass filter.
        lowPassResults = LowPassFilter(dbValue);

        // Decides whether this instance of the result could be a blow or not.
        if (lowPassResults > -30 && sumPitch == 0)
        {
            blowingTime += 1;
        }

        else
        {
            blowingTime = 0;
        }

        // Once enough successful blows have occured over the previous frames (requiredBlowTime), the blow is triggered.
        if (blowingTime > requiedBlowTime)
        {
            transform.position = transform.position + (new Vector3(0, 5, 0) * Time.deltaTime);
            Debug.Log("Blowing");
        }
        else
        {
            Debug.Log("Not BLowing");
        }
    }

    // Updates a record, by removing the oldest entry and adding the newest value (val).
    private void UpdateRecords(float val, List<float> record)
    {
        if (record.Count > recordedLength)
        {
            record.RemoveAt(0);
        }
        record.Add(val);
    }

    // Gives a result based on the peak volume of the record
    // and the previous low pass results.
    private float LowPassFilter(float peakVolume)
    {
        return alpha * peakVolume + (1.0f - alpha) * lowPassResults;
    }
}

// References:
// https://answers.unity.com/questions/157940/getoutputdata-and-getspectrumdata-they-represent-t.html
// https://forum.unity.com/threads/blow-detection-using-ios-microphone.118215/

