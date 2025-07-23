using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


namespace RiftEnemiesPlus {
    public class AnimationSampler : MonoBehaviour {
        private bool sampling = false;
        private readonly Dictionary<Func<GameObject, float>, List<float>> sampleFunctions = new(0);
        
        public void Awake() {
            AddSampleFunctions(
                gameObject => gameObject.transform.localPosition.x,
                gameObject => gameObject.transform.localPosition.y,
                gameObject => gameObject.transform.localPosition.z,
                gameObject => gameObject.transform.localRotation.eulerAngles.x,
                gameObject => gameObject.transform.localRotation.eulerAngles.y,
                gameObject => gameObject.transform.localRotation.eulerAngles.z,
                gameObject => gameObject.transform.localScale.x,
                gameObject => gameObject.transform.localScale.y,
                gameObject => gameObject.transform.localScale.z
            );
        }
        
        public void AddSampleFunction(Func<GameObject, float> sampleFunction) {
            if(sampleFunction == null) {
                throw new ArgumentNullException(nameof(sampleFunction), "Sample function cannot be null.");
            }
            if(sampling) {
                throw new InvalidOperationException("Cannot add sample functions while sampling is in progress.");
            }
            sampleFunctions[sampleFunction] = new List<float>();
        }
        
        public void AddSampleFunctions(params Func<GameObject, float>[] sampleFunctions) {
            if(sampleFunctions == null) {
                throw new ArgumentNullException(nameof(sampleFunctions), "Sample functions cannot be null.");
            }
            foreach(var sampleFunction in sampleFunctions) {
                AddSampleFunction(sampleFunction);
            }
        }
        
        public void SampleAnimation(AnimationClip clip, float sampleRate = 0.001f) {
            if(sampling) {
                Plugin.Log.LogError("Sampling is already in progress.");
                return;
            }
            if(clip == null) {
                Plugin.Log.LogError("Animation clip cannot be null.");
                return;
            }
            if(!clip.legacy) {
                Plugin.Log.LogError("Animation clip must be legacy.");
                return;
            }
            if(sampleRate <= 0) {
                Plugin.Log.LogError("Sample rate must be greater than zero.");
                return;
            }
            sampling = true;
            foreach(var data in sampleFunctions.Values) {
                data.Clear();
            }
            for(float time = 0; time <= clip.length; time += sampleRate) {
                clip.SampleAnimation(gameObject, time);
                foreach(var (function, data) in sampleFunctions) {
                    try {
                        data.Add(function(gameObject));
                    } catch {
                        data.Add(float.NaN);
                    }
                }
            }
            File.WriteAllLines(
                $"{clip.name}.txt",
                sampleFunctions.Values.Select(data => string.Join(";", data))
            );
            sampling = false;
            Plugin.Log.LogMessage($"Outputted animation data to {clip.name}.txt");
        }
    }
}
