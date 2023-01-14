using UnityEditor;
using Watona.WatonaEditor;
using RotaryPong.Events;

namespace RotaryPong.RPEditor
{
    [CustomEditor(typeof(GoalEvent))]
    public class GoalEventEditor : GameEventEditor<Paint>{}
    [CustomEditor(typeof(BallEffectEvent))]
    public class BallEffectEditor : GameEventEditor<BallEffectParameters>{}
    [CustomEditor(typeof(SpinMapInputEvent))]
    public class SpinMapInputEditor : GameEventEditor<SpinMapInputParameter>{}
}