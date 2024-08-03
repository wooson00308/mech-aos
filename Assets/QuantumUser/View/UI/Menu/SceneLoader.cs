using System.Collections;
using Quantum;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static event System.Action OnSceneLoadBegin;
    public static event System.Action OnSceneLoadDone;

    Quantum.DispatcherSubscription sub;
    private void Awake()
    {
        sub = QuantumEvent.Subscribe<Quantum.EventGameStateChanged>(this, GameStateChanged);
    }

    private void OnDestroy()
    {
        QuantumEvent.Unsubscribe(sub);
    }

    void GameStateChanged(Quantum.EventGameStateChanged evt)
    {
        if (evt.NewState == Quantum.GameState.Countdown)
        {
            Debug.Log("들어와용~!!");
            StartCoroutine(LoadScene(QuantumRunner.Default.Game.Frames.Predicted.Map.Scene));
        }
    }

    IEnumerator LoadScene(string scene)
    {
        QuantumCallback.Dispatcher.Publish(new Quantum.CallbackUnitySceneLoadBegin(QuantumRunner.Default.Game));
        OnSceneLoadBegin?.Invoke();
        AsyncOperation op = SceneManager.LoadSceneAsync(scene);
        yield return new WaitUntil(() => op.isDone);
        QuantumCallback.Dispatcher.Publish(new Quantum.CallbackUnitySceneLoadDone(QuantumRunner.Default.Game));
        OnSceneLoadDone?.Invoke();
    }
}