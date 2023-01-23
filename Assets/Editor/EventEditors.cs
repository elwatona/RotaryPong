using UnityEditor;
using Watona.WatonaEditor;
using Watona.Events;
using RotaryPong.Events;

namespace RotaryPong.RPEditor
{
    [CustomEditor(typeof(ScoreEvent))]
    public class GoalEventEditor : GameEventEditor<ScoreParameter>{}
    [CustomEditor(typeof(BallEffectEvent))]
    public class BallEffectEditor : GameEventEditor<BallEffectParameters>{}
    [CustomEditor(typeof(SpinMapInputEvent))]
    public class SpinMapInputEditor : GameEventEditor<SpinMapInputParameter>{}
    [CustomEditor(typeof(FireworksEvent))]
    public class FireworksEditor : GameEventEditor<FireworksParameter>{}
    [CustomEditor(typeof(PaintEvent))]
    public class PaintEventEditor : GameEventEditor<Paint>{}
}