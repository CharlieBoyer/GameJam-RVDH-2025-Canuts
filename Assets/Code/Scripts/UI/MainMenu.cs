using Code.Scripts.Singleton;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Main Menu")]
        [SerializeField] private RectTransform _panelMainMenu;
        [SerializeField] private Button _buttonPlay;
        [SerializeField] private Button _buttonOptions;
        [SerializeField] private Button _buttonCredits;
        [SerializeField] private Button _buttonExit;

        [Header("Options")]
        [SerializeField] private RectTransform _panelOptions;
        [SerializeField] private Button _returnButtonOptions;
        [SerializeField] private Slider _bgmSlider;
        [SerializeField] private Slider _sfxSlider;

        [Header("Credits")]
        [SerializeField] private RectTransform _panelCredits;
        [SerializeField] private Button _returnButtonCredits;
        [SerializeField] private TMP_Text _versionBox;

        private void Awake()
        {
            _versionBox.text = "v" + Application.version;
        }

        private void OnEnable()
        {
            _buttonPlay.onClick.AddListener(Play);
            _buttonOptions.onClick.AddListener(() => ShowOptions(true));
            _buttonCredits.onClick.AddListener(() => ShowCredits(true));
            _buttonExit.onClick.AddListener(Exit);

            _returnButtonOptions.onClick.AddListener(() => ShowOptions(false));
            _returnButtonCredits.onClick.AddListener(() => ShowCredits(false));

            // _bgmSlider.onValueChanged.AddListener();
            // _sfxSlider.onValueChanged.AddListener();
        }

        private void OnDisable()
        {
            _buttonPlay.onClick.RemoveAllListeners();
            _buttonOptions.onClick.RemoveAllListeners();
            _buttonCredits.onClick.RemoveAllListeners();
            _buttonExit.onClick.RemoveAllListeners();

            _returnButtonOptions.onClick.RemoveAllListeners();
            _returnButtonCredits.onClick.RemoveAllListeners();

            // _bgmSlider.onValueChanged.RemoveAllListeners();
            // _sfxSlider.onValueChanged.RemoveAllListeners();
        }

        // ---------- //

        private void Play()
        {
            SceneLoader.Instance.Load("Game");
        }

        private void ShowOptions(bool enable)
        {
            if (enable)
            {
                _panelMainMenu.gameObject.SetActive(false);
                _panelOptions.gameObject.SetActive(true);
            }
            else
            {
                _panelMainMenu.gameObject.SetActive(true);
                _panelOptions.gameObject.SetActive(false);
            }
        }

        private void ShowCredits(bool enable)
        {
            if (enable)
            {
                _panelMainMenu.gameObject.SetActive(false);
                _panelCredits.gameObject.SetActive(true);
            }
            else
            {
                _panelMainMenu.gameObject.SetActive(true);
                _panelCredits.gameObject.SetActive(false);
            }
        }

        private void Exit()
        {
            SceneLoader.Instance.Exit();
        }
    }
}
