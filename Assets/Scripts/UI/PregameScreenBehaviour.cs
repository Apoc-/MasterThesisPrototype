using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class PregameScreenBehaviour : ScreenBehaviour
    {
        public SettingHandler SettingHandler;
        public TMP_InputField InputField;
        public Button ToAvatarButton;
        public Button ToTutorialButton;

        public ToggleGroup AvatarToggleGroup;

        public void StartTutorial()
        {
            SceneManager.LoadScene("Scenes/GameScene");
        }
        
        public void StoreNameInSettingsHandler()
        {
            SettingHandler.PlayerName = InputField.text;
        }

        public void HandleToAvatarButtonInteractible()
        {
            ToAvatarButton.interactable = InputField.text != "";
        }

        public void HandleToTutorialButtonInteractible()
        {
            ToTutorialButton.interactable = AvatarToggleGroup.AnyTogglesOn();
        }
        
        public void HandleLucySelection()
        {
            SettingHandler.AvatarId = 0;
        }
        
        public void HandleJackSelection()
        {
            SettingHandler.AvatarId = 1;
        }
    }
}