using UnityEngine.Events;

public static class GameEvents 
{
    public static UnityEvent OnGenerationEnded = new UnityEvent();
    public static UnityEvent OnTilesChanged = new UnityEvent();
    public static UnityEvent<bool> OnAutoSolvingStateChange = new UnityEvent<bool>();


    public static void EndGeneration() => OnGenerationEnded.Invoke();
    public static void ChangeTiles() => OnTilesChanged.Invoke();
    public static void AutosolvingStateChange(bool state) => OnAutoSolvingStateChange.Invoke(state);

}
