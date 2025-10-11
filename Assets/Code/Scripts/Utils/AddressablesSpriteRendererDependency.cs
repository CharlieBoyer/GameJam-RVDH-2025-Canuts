// ReSharper disable InconsistentNaming

using System;
using System.Collections;
using Code.Scripts.SO.Dialogues;
using Code.Scripts.Types.Dialogues;
using UnityEngine;

namespace Code.Scripts.Utils
{
    /// <summary>
    /// Intend to fix duplicated referenced assets from Addressables builds.<br/>
    /// This class handles <c>SpriteRenderer</c> dependencies by uncoupling editor references and
    /// dynamically load the necessary asset at runtime.
    /// </summary>
    public class AddressablesSpriteRendererDependency: MonoBehaviour
    {
        public enum SpriteDependency
        {
            SP_Ousama_Objection,
            SP_Foulques_Neutral,
            SP_Payen_Le_Bouteiller,
            SP_Guillaume_De_Bures,
            SP_Gautier_Granier,
            SP_Geraud_Granier,
            SP_Marechal_Sadon,
            SP_Generic
        }

        [SerializeField] private SpriteRenderer _target;
        [SerializeField] private SpriteDependency _dependencyToBind;

        private void Awake()
        {
            (string, Emotion) characterAddress = GetCharacterAddressablesID(_dependencyToBind);

            StartCoroutine(LoadAssetToBind(characterAddress));
        }

        private IEnumerator LoadAssetToBind((string address, Emotion emotion) assetInfo)
        {
            CharacterSO character;

            yield return AddressablesUtils.LoadSingleAsset<CharacterSO>(assetInfo.address, this, asset =>
            {
                character = asset;
                _target.sprite = character.GetSprite(assetInfo.emotion);
            });
        }

        private (string, Emotion) GetCharacterAddressablesID(SpriteDependency type)
        {
            return type switch
            {
                SpriteDependency.SP_Ousama_Objection    => new ValueTuple<string, Emotion>("Ousama", Emotion.Objection),
                SpriteDependency.SP_Foulques_Neutral    => new ValueTuple<string, Emotion>("Foulques", Emotion.Neutral),
                SpriteDependency.SP_Payen_Le_Bouteiller   => new ValueTuple<string, Emotion>("PayenLeBouteiller", Emotion.Neutral),
                SpriteDependency.SP_Guillaume_De_Bures    => new ValueTuple<string, Emotion>("ConnetableGuillaumeDeBures", Emotion.Neutral),
                SpriteDependency.SP_Gautier_Granier      => new ValueTuple<string, Emotion>("GautierGranier", Emotion.Neutral),
                SpriteDependency.SP_Geraud_Granier       => new ValueTuple<string, Emotion>("GeraudGranier", Emotion.Neutral),
                SpriteDependency.SP_Marechal_Sadon       => new ValueTuple<string, Emotion>("MarechalSadon", Emotion.Neutral),
                SpriteDependency.SP_Generic             => new ValueTuple<string, Emotion>("SenechalAnscherius", Emotion.Neutral),

                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}
