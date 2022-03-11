using Modding;
using CustomAudio.Utils;
using Hutoaction =On.HutongGames.PlayMaker.Actions;
namespace CustomAudio
{
    public class CustomAudio:Mod
    {
       new public string GetName()=>nameof(CustomAudio);
        public static readonly string audiodir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "ReplaceAudio");
        public static Dictionary<string, AudioClip> audiodic = new();
        public override string GetVersion()
        {
            return "1.0";
        }
        public override void Initialize()
        {
            On.HeroController.Start += Loadaudio;
        }

        private void Loadaudio(On.HeroController.orig_Start orig, HeroController self)
        {
            orig(self);
            CheckDir();
            LoadAudioAsset();
            Hutoaction.AudioPlayerOneShot.OnEnter += Replaceshot;
            Hutoaction.AudioPlayerOneShotSingle.OnEnter += Replaceshotsingle;
            Hutoaction.AudioPlay.OnEnter += ReplaceAudioPlay;
            Hutoaction.AudioPlayRandom.OnEnter += ReplaceRandom;
            Hutoaction.AudioPlayRandomSingle.OnEnter += Replacerandomsingle;
            Hutoaction.AudioPlaySimple.OnEnter += ReplaceSimple;
            Hutoaction.AudioPlayV2.OnEnter += ReplaceV2;
            Hutoaction.PlaySound.OnEnter += ReplaceSound;
        }

        private void ReplaceSound(Hutoaction.PlaySound.orig_OnEnter orig, PlaySound self)
        {
            self.replacePlaySound(audiodic);
            orig(self);
        }

        private void ReplaceV2(Hutoaction.AudioPlayV2.orig_OnEnter orig, AudioPlayV2 self)
        {
            self.replaceAudioPlayV2(audiodic);
            orig(self);
        }

        private void ReplaceSimple(Hutoaction.AudioPlaySimple.orig_OnEnter orig, AudioPlaySimple self)
        {
            self.replaceAudioPlaySimple(audiodic);
            orig(self);
        }

        private void Replacerandomsingle(Hutoaction.AudioPlayRandomSingle.orig_OnEnter orig, AudioPlayRandomSingle self)
        {
            self.replaceAudioPlayRandomSingle(audiodic);
            orig(self);
        }

        private void ReplaceRandom(Hutoaction.AudioPlayRandom.orig_OnEnter orig, AudioPlayRandom self)
        {
            self.replaceAudioPlayRandom(audiodic);
            orig(self);
        }

       private void ReplaceAudioPlay(Hutoaction.AudioPlay.orig_OnEnter orig, AudioPlay self)
        {
             self.replaceAudioPlay(audiodic);
            orig(self);
        }
        

        private void Replaceshotsingle(Hutoaction.AudioPlayerOneShotSingle.orig_OnEnter orig, AudioPlayerOneShotSingle self)
        {
            self.replaceAudioPlayeroneShotSingle(audiodic);
            orig(self);
        }

        private void Replaceshot(Hutoaction.AudioPlayerOneShot.orig_OnEnter orig, AudioPlayerOneShot self)
        {
            self.replaceAudioPlayeroneShot(audiodic);
            orig(self);
        }

        private void CheckDir()
        {
            if (!Directory.Exists(audiodir))
            {
                LogDebug("Create AudioDir");
                Directory.CreateDirectory(audiodir);
            }
        }
        private void LoadAudioAsset()
        {
            foreach(var file in Directory.GetFiles(audiodir))
            {
                string filename = Path.GetFileNameWithoutExtension(file);
                AudioClip audio = WavUtility.createaudioclipbybyte(File.ReadAllBytes(file), filename);
                audiodic[filename] = audio;
            }
            Log("Audio Load done!");
        }
        
    }
}
