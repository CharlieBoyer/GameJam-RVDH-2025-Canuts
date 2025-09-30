using Code.Scripts.GameFSM.States;

namespace Code.Scripts.GameFSM
{
    /// <summary>
    /// Compound class holding all game state instances.
    /// </summary>
    public class GameStateInstances
    {
        public GameStateIntro Intro { get; } = new();
        public GameStateDialogue Dialogue { get; } = new();
        public GameStateTrial Trial { get; } = new();
        public GameStateTrialResolution TrialResolution { get; } = new();
        public GameStateOutro Outro { get; } = new();
    }
}
