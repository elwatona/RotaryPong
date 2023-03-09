using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Watona.Events;
using Watona.Variables;
using RotaryPong.Events;

namespace RotaryPong
{
    public class Fireworks : MonoBehaviour
    {
        IEnumerator Celebration()
        {
            int spawnedFireworks = 0;
            float celebrationFireworks = _celebrationFireworks.Value;

            while (spawnedFireworks < celebrationFireworks)
            {
                float randomDelay = Random.Range(0.05f, 0.7f);

                yield return new WaitForSeconds(randomDelay);

                float randomX = Random.Range(-25f, 25f);
                float randomY = Random.Range(-12f, 12f);
                Vector3 spawnPos = new Vector3(randomX, randomY, 0);

                FireworksParameter endGameParameters = new FireworksParameter {SourcePosition = spawnPos, RandomColor = true};

                DoFireworks(endGameParameters);

                spawnedFireworks++;

                yield return null;
            }
            yield return new WaitForSeconds(3);

            _backToMenu?.Raise();

            yield return null;
        }
        [SerializeField] CodedGameEventListener<FireworksParameter> _fireworkListener;
        [SerializeField] CodedEventListener _endGameListener;
        [SerializeField] TeamColorVariable _teamColors;
        [SerializeField] FloatVariable _celebrationFireworks;
        [SerializeField] GameEvent _backToMenu;
        private GameObject GoalParticles => Resources.Load<GameObject>("02_Prefabs/ParticleExplosion");
        
        private void DoFireworks(FireworksParameter parameters)
        {
            GameObject particles = Instantiate(GoalParticles, parameters.SourcePosition, Quaternion.identity);
            if (parameters.RandomColor && particles.TryGetComponent(out ParticleSystem particleSystem))
            {
                ParticleSystem.MainModule main = particleSystem.main;
                main.startColor = _teamColors.Value.colors[Random.Range(0, _teamColors.Value.colors.Length)];
            }
            Destroy(particles, 3);
        }

        private void OnEnable()
        {
            _fireworkListener?.OnEnable(DoFireworks);
            _endGameListener?.OnEnable(() =>  StartCoroutine(Celebration()));

        }
        private void OnDisable()
        {
            _fireworkListener?.OnDisable();
            _endGameListener?.OnDisable();
        }
    }
}
