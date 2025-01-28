using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// [ExecuteInEditMode]
public class CardAttributes : MonoBehaviour
{
    public Card card;
    [SerializeField]
    [ColorUsage(true, true)]
    private Color[] rarityColors;

    [SerializeField]
    private Color shineColor;
    [SerializeField]
    private Texture shine;
    [SerializeField]
    private Texture ultraTexture;
    [SerializeField]
    private Texture infinityTexture;
    
    private SpriteRenderer frame;
    private SpriteRenderer background;
    private SpriteRenderer characterBase;
    private SpriteRenderer characterAddon;
    private SpriteRenderer title;
    private SpriteRenderer effect1;
    private SpriteRenderer effect2;
    private Transform particles;
    private ParticleSystem particle1;
    private ParticleSystem particle2;
    private Rarity rarity;
    private TextMeshPro power;
    private TextMeshPro cost;
    private Animation anim;
    public Volume volume;
    public float LerpSpeed;
    private Bloom bloom;

    private void Awake() {
        anim = GetComponent<Animation>();
        volume = GameObject.Find("Global Volume").GetComponent<Volume>();
        volume.profile.TryGet<Bloom>(out bloom);
        SetupCardReferences();
        UpdateCard();
    }

    private void SetupCardReferences(){
        frame = this.transform.Find("FrameMask/Frame").GetComponent<SpriteRenderer>();
        background = this.transform.Find("MiddleMask/Background").GetComponent<SpriteRenderer>();
        characterBase = this.transform.Find("MiddleMask/Character_Base").GetComponent<SpriteRenderer>();
        characterAddon = this.transform.Find("MiddleMask/Character_Move").GetComponent<SpriteRenderer>();
        title = this.transform.Find("MiddleMask/Title").GetComponent<SpriteRenderer>();
        effect1 = this.transform.Find("MiddleMask/Effect1").GetComponent<SpriteRenderer>();
        effect2 = this.transform.Find("MiddleMask/Effect2").GetComponent<SpriteRenderer>();
        particles = this.transform.Find("MiddleMask/Particles");
        power = this.transform.Find("Frame/Power/PowerText").GetComponent<TextMeshPro>();
        cost = this.transform.Find("Frame/Cost/CostText").GetComponent<TextMeshPro>();

        foreach (Transform child in particles) {
            GameObject.Destroy(child.gameObject);
        }

        if(card.particle1){
            particle1 = Instantiate(card.particle1, particles);
        }
        
        if(card.particle2){
            particle2 = Instantiate(card.particle2, particles);
        }
    }

    private void SetCardValues(){
        if(card){
            characterAddon.sprite = null;
            background.sprite = card.background.sprite;
            characterBase.sprite = card.characterBase.sprite;
            characterBase.material.SetTexture("_MoveMap", card.characterBaseMap.texture);
            characterBase.sprite = card.characterBase.sprite;
            if(card.characterAddon.sprite != null){
                characterAddon.sprite = card.characterAddon.sprite;
                characterAddon.sortingOrder = card.characterAddon.orderInLayer;
                characterAddon.material.SetTexture("_MoveMap", card.characterAddonMap.texture);
            }
            title.sprite = card.title.sprite;
            rarity = card.rarity;
            power.text = card.power.ToString();
            cost.text = card.cost.ToString();
            effect1.sprite = card.effect1.sprite;
            effect2.sprite =  card.effect2.sprite;
        }
    }

    private void setFrameBreak(){
        if(card){
            characterBase.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            characterAddon.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            effect1.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            effect2.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
            characterBase.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
            characterAddon.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
            if(rarity >= Rarity.Uncommon){
                characterBase.transform.localScale = new Vector3(0.54f, 0.54f, 0.54f);
                characterBase.maskInteraction = card.characterBase.frameBreak ? SpriteMaskInteraction.None : SpriteMaskInteraction.VisibleInsideMask;
                characterAddon.transform.localScale = new Vector3(0.54f, 0.54f, 0.54f);
                characterAddon.maskInteraction = card.characterAddon.frameBreak ? SpriteMaskInteraction.None : SpriteMaskInteraction.VisibleInsideMask;
                effect1.maskInteraction = card.effect1.frameBreak ? SpriteMaskInteraction.None : SpriteMaskInteraction.VisibleInsideMask;
                effect2.maskInteraction = card.effect2.frameBreak ? SpriteMaskInteraction.None : SpriteMaskInteraction.VisibleInsideMask;
            }
        }
    }
    
    private void Set3DZ(){
        if(card){
            float characterZ = 0;
            float characterMoveZ = 0;
            float backgroundZ = 0;
            float titleZ = 0;
            float effect1Z = 0;
            float effect2Z =  0;
            if(rarity >= Rarity.Rare){
                characterZ = card.characterBase.animationZ;
                characterMoveZ = card.characterAddon.animationZ;
                backgroundZ = card.background.animationZ;
                titleZ = card.title.animationZ ;
                effect1Z = card.effect1.animationZ;
                effect2Z = card.effect2.animationZ;
            }
            characterBase.transform.localPosition = new Vector3(characterBase.transform.localPosition.x, characterBase.transform.localPosition.y, characterZ);
            characterAddon.transform.localPosition = new Vector3(characterAddon.transform.localPosition.x, characterAddon.transform.localPosition.y, characterMoveZ);
            background.transform.localPosition = new Vector3(background.transform.localPosition.x, background.transform.localPosition.y, backgroundZ);
            effect1.transform.localPosition = new Vector3(effect1.transform.localPosition.x, effect1.transform.localPosition.y, effect1Z);
            effect2.transform.localPosition = new Vector3(effect2.transform.localPosition.x, effect2.transform.localPosition.y, effect2Z);
            title.transform.localPosition = new Vector3(title.transform.localPosition.x, title.transform.localPosition.y, titleZ);
        }
    }

    private void SetFrameColor(){
        frame.material.SetColor("_Color", rarityColors[(int) rarity]);
        frame.material.SetTexture("_FrameTexture", null);
        if(rarity == Rarity.Ultra){
            frame.material.SetTexture("_FrameTexture", ultraTexture);
        }else if(rarity == Rarity.Infinity){
            frame.material.SetTexture("_FrameTexture", infinityTexture);
        }
    }

    private void SetTitleShine(){
        title.material.SetTexture("_Shine", null);
        float intensity = 1;
        if(rarity >= Rarity.Legendary){
            intensity = 2;
            title.material.SetTexture("_Shine", shine);
        }
        
        float factor = Mathf.Pow(2,intensity);
        Color newColor = new Color(shineColor.r*factor,shineColor.g*factor,shineColor.b*factor);
        title.material.SetColor("_Color", newColor);
    }

    private void SetAnimation(){
        float characterBaseMapSpeed = 0f;
        float characterBaseMapStrength = 0f;
        float characterMoveMapSpeed = 0f;
        float characterMoveMapStrength = 0f;
        if(particle1){
            particle1.Stop();
        }
        if(particle2){
            particle2.Stop();
        }
        if(rarity >= Rarity.Epic){
                characterBaseMapSpeed = card.characterBaseMap.speed;
                characterBaseMapStrength = card.characterBaseMap.strength;
                characterMoveMapSpeed = card.characterAddonMap.speed;
                characterMoveMapStrength = card.characterAddonMap.strength;
                if(card.particle1){
                    particle1.Play();
                }

                if(card.particle2){
                    particle2.Play();
                }
        }
        characterBase.material.SetFloat("_Speed", characterBaseMapSpeed);
        characterBase.material.SetFloat("_Strength", characterBaseMapStrength);
        characterAddon.material.SetFloat("_Speed", characterMoveMapSpeed);
        characterAddon.material.SetFloat("_Strength", characterMoveMapStrength);
    }

    private void UpdateCard(){
        SetCardValues();
        setFrameBreak();
        Set3DZ();
        SetTitleShine();
        SetAnimation();
        SetFrameColor();
    }

    private void IncreaseBloom(){
        if(bloom){
            StopCoroutine(LerpBloom());
            StartCoroutine(LerpBloom(1000f));
            bloom.tint.value = rarityColors[(int) card.rarity];
        }
    }

    IEnumerator LerpBloom(float intensity = 1)
    {
        float time = 0;
        float startingIntensity = bloom.intensity.value;
        while (time < 500){
            bloom.intensity.value = Mathf.Lerp(startingIntensity, intensity, time);
            time += LerpSpeed * Time.deltaTime;
            yield return null;
        }
        bloom.intensity.value = intensity;
    }

    private void DecreaseBloom(){
        if(bloom){
            StopCoroutine(LerpBloom());
            StartCoroutine(LerpBloom(1f));
            bloom.tint.value = Color.white;
        }
    }

    private void Update() {
        if(card && card.rarity != rarity){
            // Play Upgrade/Downgrade effect
            anim.Play();
        }
    }


}
