namespace CustomAudio.Utils
{
    public static class AudioUtils
    {
        public static byte[] readfromEmbberedSource(this Assembly assembly, string resourcename)
        {
            byte[] data = null;
            foreach (var name in assembly.GetManifestResourceNames())
            {

                if (name.Contains(resourcename))
                {
                    using Stream stream = assembly.GetManifestResourceStream(name);
                    data = new byte[stream.Length];
                    stream.Read(data, 0, data.Length);
                    Modding.Logger.LogDebug($"read{name}");
                    return data;
                }
            }
            return data;
        }
        public static void JudgeActionAndReplace(FsmStateAction action, Dictionary<string, AudioClip> audiodic)
        {
            switch (action)
            {
                case AudioPlayerOneShot oneShot:
                    oneShot.replaceAudioPlayeroneShot(audiodic);
                    break;
                case AudioPlay audioPlay:
                    audioPlay.replaceAudioPlay(audiodic);
                    break;
                case AudioPlayerOneShotSingle oneShotSingle:
                    oneShotSingle.replaceAudioPlayeroneShotSingle(audiodic);
                    break;
                case AudioPlayRandom audioPlayRandom:
                    audioPlayRandom.replaceAudioPlayRandom(audiodic);
                    break;
                case AudioPlaySimple simplePlay:
                    simplePlay.replaceAudioPlaySimple(audiodic);
                    break;
                case AudioPlayRandomSingle singlePlayRandom:
                    singlePlayRandom.replaceAudioPlayRandomSingle(audiodic);
                    break;
                case AudioPlayV2 audioPlayV2:
                    audioPlayV2.replaceAudioPlayV2(audiodic);
                    break;
                case PlayRandomSound soundPlayRandom:
                    soundPlayRandom.replacePlayRandomSound(audiodic);
                    break;
                case PlaySound soundPlaySound:
                    soundPlaySound.replacePlaySound(audiodic);
                    break;
                default:
                    break;
            }
        }
        public static AudioClip GetAudioInDic(string name,Dictionary<string,AudioClip> audiodic)
        {
            AudioClip audio = audiodic.Where(x => name==x.Key)
                .FirstOrDefault()
                .Value;
            return audio;
        }
        public static void replaceAudioPlayeroneShot(this AudioPlayerOneShot action, Dictionary<string, AudioClip> audiodic)
        {
            var clips = new AudioClip[action.audioClips.Length];
            var origClips = action.audioClips;
            for (int i = 0; i < action.audioClips.Length; i++)
            {
                AudioClip clip = GetAudioInDic(action.audioClips[i].name, audiodic);
                if(clip != null)
                {
                    clips[i] = clip;
                }
                else
                {
                    clips[i] = origClips[i];
                    Modding.Logger.LogDebug($"orig{clips[i].name}");
                }
            }
            action.audioClips = clips;
        }
        
        public static void replaceAudioPlaySimple( this AudioPlaySimple action, Dictionary<string, AudioClip> audiodic)
        {
            GameObject ownerDefaultTarget = action.Fsm.GetOwnerDefaultTarget(action.gameObject);
            if (ownerDefaultTarget != null)
            {
                AudioSource audioSource = ownerDefaultTarget.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    if (action.oneShotClip == null)
                    {
                        AudioClip audio = GetAudioInDic(audioSource.clip.name, audiodic);
                        if (audio != null)
                        {
                            audioSource.clip = audio;
                        }
                        return;

                    }
                    else
                    {
                        AudioClip clip = GetAudioInDic(action.oneShotClip.Name, audiodic);
                        if (clip != null)
                        {
                            action.oneShotClip.Value = clip;
                        }
                    }
                }
            }
        }
        public static void replaceAudioPlay(this AudioPlay action, Dictionary<string, AudioClip> audiodic)
        {
            GameObject ownerDefaultTarget = action.Fsm.GetOwnerDefaultTarget(action.gameObject);
            if (ownerDefaultTarget != null)
            {
                AudioSource audioSource= ownerDefaultTarget.GetComponent<AudioSource>();
                if (audioSource!=null)
                {
                    if(action.oneShotClip==null)
                    {
                        AudioClip audio = GetAudioInDic(audioSource.clip.name, audiodic);
                        if(audio!=null)
                        {
                            audioSource.clip = audio;
                        }
                        return;

                    }
                    else
                    {
                        AudioClip clip = GetAudioInDic(action.oneShotClip.Name, audiodic);
                        if(clip != null)
                        {
                            action.oneShotClip.Value=clip;
                        }
                    }
                }
            }
        }
        public static void replaceAudioPlayeroneShotSingle(this AudioPlayerOneShotSingle action, Dictionary<string, AudioClip> audiodic)
        {
            AudioClip clip = GetAudioInDic(action.audioClip.Value.name, audiodic);
            if (clip != null)
            {
                action.audioClip.Value = clip;
            }
        }
        public static void replaceAudioPlayRandom(this AudioPlayRandom action, Dictionary<string, AudioClip> audiodic)
        {
            var clips = new AudioClip[action.audioClips.Length];
            var origClips = action.audioClips;
            for (int i = 0; i < action.audioClips.Length; i++)
            {
                AudioClip clip = GetAudioInDic(action.audioClips[i].name, audiodic);
                if (clip != null)
                {
                    clips[i] = clip;
                }
                else
                {
                    clips[i] = origClips[i];
                    Modding.Logger.LogDebug($"orig{clips[i].name}");
                }
            }
            action.audioClips = clips;
        }
        public static void replaceAudioPlayRandomSingle(this AudioPlayRandomSingle action, Dictionary<string, AudioClip> audiodic)
        {
            AudioClip clip = GetAudioInDic(action.audioClip.Value.name, audiodic);
            if (clip != null)
            {
                action.audioClip.Value = clip;
            }
        }
        public static void replaceAudioPlayV2(this AudioPlayV2 action, Dictionary<string, AudioClip> audiodic)
        {
            GameObject ownerDefaultTarget = action.Fsm.GetOwnerDefaultTarget(action.gameObject);
            if (ownerDefaultTarget != null)
            {
                AudioSource audioSource = ownerDefaultTarget.GetComponent<AudioSource>();
                if (audioSource != null)
                {
                    if (action.oneShotClip == null)
                    {
                        AudioClip audio = GetAudioInDic(audioSource.clip.name, audiodic);
                        if (audio != null)
                        {
                            audioSource.clip = audio;
                        }
                        return;

                    }
                    else
                    {
                        AudioClip clip = GetAudioInDic(action.oneShotClip.Name, audiodic);
                        if (clip != null)
                        {
                            action.oneShotClip.Value = clip;
                        }
                    }
                }
            }
        }
        public static void replacePlayRandomSound(this PlayRandomSound action, Dictionary<string, AudioClip> audiodic)
        {
            var clips = new AudioClip[action.audioClips.Length];
            var origClips = action.audioClips;
            for (int i = 0; i < action.audioClips.Length; i++)
            {
                AudioClip clip = GetAudioInDic(action.audioClips[i].name, audiodic);
                if (clip != null)
                {
                    clips[i] = clip;
                }
                else
                {
                    clips[i] = origClips[i];
                    Modding.Logger.LogDebug($"orig{clips[i].name}");
                }
            }
            action.audioClips = clips;
        }
        public static void replacePlaySound(this PlaySound action, Dictionary<string, AudioClip> audiodic)
        {
            AudioClip clip = GetAudioInDic(action.clip.Value.name, audiodic);
            if (clip != null)
            {
                action.clip.Value = clip;
            }
        }


    }
}
