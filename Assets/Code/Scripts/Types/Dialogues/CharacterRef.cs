using System;
using Code.Scripts.SO.Dialogues;
using Code.Scripts.Utils;
using UnityEngine;

namespace Code.Scripts.Types.Dialogues
{
    public static class CharacterRef
    {
        public enum Index
        {
            None = 0,
            Ousama,
            Renier,
            Foulques,
            SenechalAnscherius,
            ConnetableGuillaumeDeBures,
            PayenLeBouteiller,
            MarechalSadon,
            GautierGranier,
            GeraudGranier,
        }

        public static CharacterSO Ousama { get; private set; }
        public static CharacterSO Renier { get; private set; }
        public static CharacterSO Foulques { get; private set; }
        public static CharacterSO SenechalAnscherius { get; private set; }
        public static CharacterSO ConnetableGuillaumeDeBures { get; private set; }
        public static CharacterSO PayenLeBouteiller { get; private set; }
        public static CharacterSO MarechalSadon { get; private set; }
        public static CharacterSO GautierGranier { get; private set; }
        public static CharacterSO GeraudGranier { get; private set; }

        public static bool IsInit { get; private set; } = false;

        /// <summary>
        /// Loads all the <c>CharacterSO</c> and "register" them into <c>CharacterRef</c> class with static readonly access.
        /// </summary>
        /// <remarks>The last character to load assumes a complete loading and marks the class <c>_isInit</c> to <c>true</c></remarks>
        /// <param name="requester">The <c>MonoBehaviour</c> object from which to run the async loads.</param>
        public static void Init(MonoBehaviour requester)
        {
            AddressablesUtils.LoadSingleAsset<CharacterSO>("Ousama", requester, asset => Ousama = asset);
            AddressablesUtils.LoadSingleAsset<CharacterSO>("Renier", requester, asset => Renier = asset);
            AddressablesUtils.LoadSingleAsset<CharacterSO>("Foulques", requester, asset => Foulques = asset);
            AddressablesUtils.LoadSingleAsset<CharacterSO>("SenechalAnscherius", requester, asset => SenechalAnscherius = asset);
            AddressablesUtils.LoadSingleAsset<CharacterSO>("ConnetableGuillaumeDeBures", requester, asset => ConnetableGuillaumeDeBures = asset);
            AddressablesUtils.LoadSingleAsset<CharacterSO>("PayenLeBouteiller", requester, asset => PayenLeBouteiller = asset);
            AddressablesUtils.LoadSingleAsset<CharacterSO>("MarechalSadon", requester, asset => MarechalSadon = asset);
            AddressablesUtils.LoadSingleAsset<CharacterSO>("GautierGranier", requester, asset => GautierGranier = asset);
            AddressablesUtils.LoadSingleAsset<CharacterSO>("GeraudGranier", requester, asset => {
                GeraudGranier = asset;
                IsInit = true;
            });
        }

        public static CharacterSO GetCharacter(Index index)
        {
            return index switch
            {
                Index.None                         => null,
                Index.Ousama                       => Ousama,
                Index.Renier                       => Renier,
                Index.Foulques                     => Foulques,
                Index.SenechalAnscherius           => SenechalAnscherius,
                Index.ConnetableGuillaumeDeBures   => ConnetableGuillaumeDeBures,
                Index.PayenLeBouteiller            => PayenLeBouteiller,
                Index.MarechalSadon                => MarechalSadon,
                Index.GautierGranier               => GautierGranier,
                Index.GeraudGranier                => GeraudGranier,

                _ => throw new ArgumentException($"GetCharacter(): Can't get CharacterSO sheet from index {index.ToString()}")
            };
        }

    }
}
