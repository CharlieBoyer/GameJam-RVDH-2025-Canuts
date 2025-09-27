using Code.Scripts.Singleton;
using Code.Scripts.Types;
using Code.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Entities
{
    public class Judge: MonoBehaviour
    {
        // Statics
        public static Judge[] Instances { get; private set; }

        [Header("Identity")]
        [SerializeField] [Tooltip("Be cautious of setting only one flag per judge!")] private JudgeRef _refID;
        [SerializeField] private Sensibilities _sensibilities;

        [Header("UI Module")]
        [SerializeField] private TMP_Text _nameBox;
        [SerializeField] private Slider _convictionGauge;

        public static void InitializeJudges()
        {
            Instances = FindObjectsByType<Judge>(FindObjectsSortMode.InstanceID);

            foreach (Judge judge in Instances)
            {
                judge.Init();
            }
        }

        public void Init()
        {
            gameObject.name = name + "_" + _refID;
            _nameBox.text = _refID.ToString().AddSpacesToEnum();
            _convictionGauge.wholeNumbers = true;
            _convictionGauge.maxValue = GameManager.Instance.JudgesMaxConviction;
            _convictionGauge.value = 0;
        }
    }
}
