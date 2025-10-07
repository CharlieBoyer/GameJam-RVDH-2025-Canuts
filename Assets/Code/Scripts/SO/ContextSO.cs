using UnityEngine;

namespace Code.Scripts.SO
{
    [CreateAssetMenu(fileName = "New Context Object", menuName = "SO/Context", order = 0)]
    public class ContextSO: ScriptableObject
    {
        [SerializeField] private int _indexID = 0;
        [SerializeField] [TextArea] private string _text = "Sentence to display...";

        public int IndexID => _indexID;
        public string Text => _text;
    }
}
