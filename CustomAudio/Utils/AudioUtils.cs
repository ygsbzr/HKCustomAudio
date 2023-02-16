using UnityEngine;
using WavLib;

namespace CustomAudio.Utils;
public static class AudioUtils
{
    public static AudioClip GetAudioInDic(string name,Dictionary<string,AudioClip> audiodic)
    {
        if(name == null)
        {
            return null;
        }
        AudioClip audio = audiodic.FirstOrDefault(x => name==x.Key).Value;
        return audio;
    }
    
    internal static void LoadAudioAsset()
    {
        foreach(var file in Directory.GetFiles(CustomAudio.audiodir))
        {
            string filename = Path.GetFileNameWithoutExtension(file);
            
            FileStream stream = File.OpenRead(file);
            WavData wavData = new WavData();
            wavData.Parse(stream);
            stream.Close();

            float[] wavSoundData = wavData.GetSamples();
            AudioClip audioClip = AudioClip.Create(filename, wavSoundData.Length / wavData.FormatChunk.NumChannels, wavData.FormatChunk.NumChannels, (int) wavData.FormatChunk.SampleRate, false);
            audioClip.SetData(wavSoundData, 0);
            
            CustomAudio.audiodic[filename] = audioClip;
        }
        CustomAudio.Instance.Log("Audio Load done!");
    }
    
    public static AudioClip ReplaceAudio(AudioSource source) => ReplaceAudio(source.clip);
    public static AudioClip ReplaceAudio(AudioClip origClip)
    {
        if (origClip != null)
        {
            AudioClip newClip = GetAudioInDic(origClip.name, CustomAudio.audiodic);

            if (newClip != null)
            {
                Modding.Logger.Log($"Replacing {origClip.name}");
                return newClip;
            }
            
            return origClip;
        }

        return origClip;
    }
}