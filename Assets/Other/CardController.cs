using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class CardController : MonoBehaviour
{
    private UIDocument document;
    private Button levelUp;
    private Button levelDown;
    [SerializeField]
    private CardAttributes viewedCard;

    PlayerInput playerInput;

    private void Awake() {
        UnityEngine.Cursor.visible = false;
        document = GetComponent<UIDocument>();
        document.rootVisualElement.Q("PARENT").style.display = DisplayStyle.None;
        levelUp = document.rootVisualElement.Q<Button>("levelUp");
        levelDown = document.rootVisualElement.Q<Button>("levelDown");

        levelUp.clicked += levelUpClicked;
        levelDown.clicked += levelDownClicked;
        playerInput = new PlayerInput();

    }

    private void OnEnable() {
        playerInput.Enable();
    }

    private void OnDisable() {
        playerInput.Disable();
    }

    private void Start() {
        playerInput.Something.Up.performed += UpPressed;
        playerInput.Something.Down.performed += DownPressed;
    }

    void UpPressed(InputAction.CallbackContext context){
        levelUpClicked();
    }

    void DownPressed(InputAction.CallbackContext context){
        levelDownClicked();
    }

    private void levelUpClicked(){
        print("level up");
        if(!viewedCard.card.rarity.Equals(Rarity.Infinity)){
            viewedCard.card.rarity++;
        }
    }

    private void levelDownClicked(){
        print("level down");
        if(!viewedCard.card.rarity.Equals(Rarity.Common)){
            viewedCard.card.rarity--;
        }
    }

    
}
