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
        
        ///<summary> Maneja la logica que permite el juego termine </summary>
        IEnumerator Celebration()
        {
            int spawnedFireworks = 0;
            float celebrationFireworks = _celebrationFireworks.Value;

            while (spawnedFireworks < celebrationFireworks)
            {
                float randomDelay = Random.Range(0.05f, 0.7f);

                Debug.Log("spawning after delay: " + randomDelay);

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
        // [SerializeField] GameEvent _fireworkSFX;
        private GameObject GoalParticles => Resources.Load<GameObject>("02_Prefabs/ParticleExplosion");
        
        ///<summary> Instancia fuegos artificiales </summary>
        ///<param name="position"> Posicion deseada para la instancia </param>
        ///<param name="multiColor"> Define si se modificara el color de las particulas </param>
        private void DoFireworks(FireworksParameter parameters)
        {
            GameObject particles = Instantiate(GoalParticles, parameters.SourcePosition, Quaternion.identity);
            if (parameters.RandomColor && particles.TryGetComponent(out ParticleSystem particleSystem))
            {
                ParticleSystem.MainModule main = particleSystem.main;
                main.startColor = _teamColors.Value.colors[Random.Range(0, _teamColors.Value.colors.Length)];
            }
            // _fireworkSFX?.Raise();
            Destroy(particles, 3);
        }
        ///<summary> Retorna las particulas a instanciar en caso de gol </summary>
        private void EndGame(FireworksParameter parameter)
        {
            StartCoroutine(Celebration());
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
