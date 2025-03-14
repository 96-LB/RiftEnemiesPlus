using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using UnityEngine;

public static class AssetSwapper {
    private class AssetSwap : IDisposable {
        private readonly ConfigEntry<bool> config;
        private readonly Sprite[] target;
        private readonly Sprite[] oldSprites;
        private readonly Sprite[] newSprites;
        
        public bool Swapped { get; private set; }
        
        public AssetSwap(Sprite[] target, Sprite[] sprites, ConfigEntry<bool> config) {
            this.target = target ?? throw new ArgumentNullException(nameof(target));
            oldSprites = target.ToArray(); // make a copy
            newSprites = sprites ?? throw new ArgumentNullException(nameof(newSprites));
            
            if(oldSprites.Length != newSprites.Length) {
                throw new ArgumentException($"Supplied set of sprites is not the same length as the target. Expected {target.Length}, got {newSprites.Length}.", nameof(sprites));
            }
            
            this.config = config;
            Swapped = false;
            
            if(config != null) {
                config.SettingChanged += HandleConfigSettingChanged;
                HandleConfigSettingChanged(null, null); // swap sprites if config is turned on
            } else {
                SwapSprites();
            }
        }
        
        private void HandleConfigSettingChanged(object sender, EventArgs e) {
            if(config == null) {
                throw new InvalidOperationException("No configuration entry has been set.");
            }
            
            if(config.Value) {
                SwapSprites();
            } else {
                RevertSprites();
            }
        }
        
        private void SwapSprites(bool revert = false) {
            if(Swapped == revert) {
                for(int i = 0; i < oldSprites.Length; i++) {
                    target[i] = revert ? oldSprites[i] : newSprites[i];
                }
                Swapped = !revert;
            }
        }
        
        public void SwapSprites() {
            SwapSprites(revert: false);
        }
        
        public void RevertSprites() {
            SwapSprites(revert: true);
        }
        
        public void Dispose() {
            RevertSprites();
            if(config != null) {
                config.SettingChanged -= HandleConfigSettingChanged;
            }
        }
    }
    
    private static readonly Dictionary<Sprite[], AssetSwap> swaps = new();
    public static int SwapCount => swaps.Count;
    
    public static bool TryAddSwap(Sprite[] target, Sprite[] sprites, ConfigEntry<bool> config = null) {
        if(HasSwapFor(target)) {
            return false;
        }
        swaps.Add(target, new(target, sprites, config));
        return true;
    }
    
    public static void ClearSwaps() {
        foreach(var (_, swap) in swaps) {
            swap.Dispose();
        }
        swaps.Clear();
    }
    
    public static bool HasSwapFor(Sprite[] target) {
        return swaps.ContainsKey(target);
    }
}
