using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

[RequireComponent(typeof(Animator))]
public class CreateAnimationPlayable : MonoBehaviour
{
    public AnimationClip clip;

    private PlayableGraph _graph;

    private void Start()
    {
        _graph = PlayableGraph.Create("Test Playable RootMotion");
        _graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        var playableOutput = AnimationPlayableOutput.Create(_graph, "Animation", GetComponent<Animator>());
        var playable = AnimationClipPlayable.Create(_graph, clip);
        playableOutput.SetSourcePlayable(playable);

        _graph.Play();
    }

    private void OnDestroy()
    {
        if (_graph.IsValid())
        {
            _graph.Destroy();
        }
    }
}
