using System;
using System.Linq;
using BepInEx.Configuration;
using UnityEngine;

public class AssetSwapper : MonoBehaviour {
    private ConfigEntry<bool> config;
    private Sprite[] target;
    private Sprite[] oldSprites;
    private Sprite[] newSprites;
    
    public bool Initialized { get; private set; }
    public bool Swapped { get; private set; }
    
    public void Initialize(Sprite[] target, Sprite[] sprites, ConfigEntry<bool> config) {
        if(Initialized) {
            RevertSprites();
            if(config != null) {
                config.SettingChanged -= HandleConfigSettingChanged;
            }
        }
        
        this.target = target ?? throw new ArgumentNullException(nameof(target));
        oldSprites = target.ToArray(); // make a copy
        newSprites = sprites ?? throw new ArgumentNullException(nameof(newSprites));
        
        if(oldSprites.Length != newSprites.Length) {
            throw new ArgumentException($"Supplied set of sprites is not the same length as the target. Expected {target.Length}, got {newSprites.Length}.", nameof(sprites));
        }
        
        this.config = config;
        Swapped = false;
        Initialized = true;
        
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
        if(!Initialized) {
            throw new InvalidOperationException("Tried to swap sprites before initializing AssetSwapper!");
        }
        
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
}
