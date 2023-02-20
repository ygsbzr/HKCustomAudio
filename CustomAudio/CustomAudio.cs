using System.Reflection;
using Modding;
using CustomAudio.Utils;
using CustomKnight;
using MonoMod.RuntimeDetour;
using UnityEngine;

namespace CustomAudio;

public class CustomAudio : Mod
{
    public static string audiodir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "ReplaceAudio");
    public static Dictionary<string, AudioClip> audiodic = new();
        
    public new string GetName() => nameof(CustomAudio);
    internal static CustomAudio Instance;
    public override string GetVersion() => "1.4.1";
        
    public override void Initialize()
    {
        Instance = this;
        On.HeroController.Start += LoadAudio;
        
        if (ModHooks.GetMod("CustomKnight") is Mod)
        {
            AddCKHandle();
        }
    }
    
    private void AddCKHandle()
    {
        SkinManager.OnSetSkin += ResetAudio;
    }
    
    private void ResetAudio(object sender, EventArgs e)
    {
        string currentSkinPath = SkinManager.GetCurrentSkin().getSwapperPath();
        if (Directory.Exists(Path.Combine(currentSkinPath, "Audio")))
        {
            audiodic.Clear();
            audiodir = Path.Combine(currentSkinPath, "Audio");
            AudioUtils.LoadAudioAsset();
        }
    }

    private void LoadAudio(On.HeroController.orig_Start orig, HeroController self)
    {
        orig(self);
        
        CheckDir();
        AudioUtils.LoadAudioAsset();
        
        _ = new Hook(typeof(AudioSource).GetMethod(nameof(AudioSource.Play), Array.Empty<Type>()), ReplaceAudio_Play);
        _ = new Hook(typeof(AudioSource).GetMethod(nameof(AudioSource.Play), new [] { typeof(ulong) }), ReplaceAudio_Play_ulong);
        _ = new Hook(typeof(AudioSource).GetMethod(nameof(AudioSource.PlayDelayed)), ReplaceAudio_PlayDelayed);
        _ = new Hook(typeof(AudioSource).GetMethod(nameof(AudioSource.PlayScheduled)), ReplaceAudio_PlayScheduled);
        _ = new Hook(typeof(AudioSource).GetMethod(nameof(AudioSource.PlayOneShot), new [] { typeof(AudioClip), typeof(float)}), ReplaceAudio_PlayOneShot);
        _ = new Hook(typeof(AudioSource).GetMethod(nameof(AudioSource.PlayClipAtPoint), new [] { typeof(AudioClip), typeof(Vector3), typeof(float)}), ReplaceAudio_PlayClipAtPoint);

        On.GameManager.OnNextLevelReady += ChangePlayAwakeSources;
        
        On.HeroController.Start -= LoadAudio; //unhook so stuff doesnt get hooked twice for no reason 
    }

    private void CheckDir()
    {
        if (!Directory.Exists(audiodir))
        {
            LogDebug("Create AudioDir");
            Directory.CreateDirectory(audiodir);
        }
    }

    private static void ReplaceAudio_Play(Action<AudioSource> orig, AudioSource self)
    {
        self.clip = AudioUtils.ReplaceAudio(self);
        orig(self);
    }
    private static void ReplaceAudio_Play_ulong(Action<AudioSource, ulong> orig, AudioSource self, ulong delay)
    {
        self.clip = AudioUtils.ReplaceAudio(self);
        orig(self, delay);
    }
    private static void ReplaceAudio_PlayDelayed(Action<AudioSource, float> orig, AudioSource self, float delay)
    {
        self.clip = AudioUtils.ReplaceAudio(self);
        orig(self, delay);
    }
    private static void ReplaceAudio_PlayScheduled(Action<AudioSource, double> orig, AudioSource self, double time)
    {
        self.clip = AudioUtils.ReplaceAudio(self);
        orig(self, time);
    }
    private static void ReplaceAudio_PlayOneShot(Action<AudioSource, AudioClip, float> orig, AudioSource self, AudioClip clip, float volumeScale)
    {
        orig(self, AudioUtils.ReplaceAudio(clip), volumeScale);
    }
    private static void ReplaceAudio_PlayClipAtPoint(Action<AudioClip, Vector3, float> orig, AudioClip clip, Vector3 position, float volume)
    {
        orig(AudioUtils.ReplaceAudio(clip), position, volume);
    }
    
    private static void ChangePlayAwakeSources(On.GameManager.orig_OnNextLevelReady orig, GameManager self)
    {
        orig(self);

        foreach (var source in Resources.FindObjectsOfTypeAll<AudioSource>()
                     .Where(a => a.playOnAwake && a.isPlaying))
        {
            source.Play(); //force play to be called, the OnHooks will deal with the rest
            //we can't just replace the source.clip, it wont cause the new clip to play. 
            //we'd have to call play anyways and that leads to the replacing happening twice
        }
    }
}