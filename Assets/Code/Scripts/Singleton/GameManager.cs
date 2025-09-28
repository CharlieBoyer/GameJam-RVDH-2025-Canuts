using System;
using System.Collections.Generic;
using Code.Scripts.Entities;
using Code.Scripts.SO;
using Code.Scripts.Utils;
using UnityEngine;

namespace Code.Scripts.Singleton
{
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        [Header("Game settings")]
        [SerializeField] private int _judgesMaxConviction;

        [Header("References")]
        [SerializeField] private GameObject _choicePrefab;
        [SerializeField] private List<PlayerChoice> _choicePool;
        [SerializeField] private RectTransform _playerChoicePanel;

        public int JudgesMaxConviction => _judgesMaxConviction;

        private Judge[] _judges;

        private void Awake()
        {
            Judge.InitializeJudges();
        }

        private void Start()
        {
            AudioManager.Instance.PlayMusic();
        }
    }
}
