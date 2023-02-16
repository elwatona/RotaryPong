using UnityEngine;
using UnityEngine.SceneManagement;
using Watona.Variables;
using Watona.Events;
using RotaryPong.Events;

namespace RotaryPong
{
    public class VolumenHandler : MonoBehaviour
    {
        [SerializeField] CodedGameEventListener<Paint> _playerHit;
        [SerializeField] CodedGameEventListener<FireworksParameter> _fireworkSFX;
        [SerializeField] FloatVariable _matchTimer;
        [SerializeField] FloatVariable _musicVolumen;
        [SerializeField] FloatVariable _sfxVolumen;
        [SerializeField, Space] AudioSource _musicSource;
        [SerializeField] AudioSource _sfxSource;
        [SerializeField, Space] AudioClip[] _sfxClips;
        [SerializeField] AudioClip[] _musicClips;
        private int _randomSFXClip => Random.Range(0, _sfxClips.Length);

        private void Awake()
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("SoundManager");
            if(objs.Length > 1) { print("Ya existia este objeto"); Destroy(this.gameObject); }
            DontDestroyOnLoad(this.gameObject);

            _musicSource.volume = _musicVolumen.Value / 100;
            _sfxSource.volume = _sfxVolumen.Value / 100;
        }
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            _musicVolumen.PropertyChanged += ChangeVolumenValue;
            _sfxVolumen.PropertyChanged += ChangeVolumenValue;

            _playerHit?.OnEnable(PlayerHit);
        }
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            _musicVolumen.PropertyChanged -= ChangeVolumenValue;
            _sfxVolumen.PropertyChanged -= ChangeVolumenValue;

            _fireworkSFX?.OnDisable();
            _playerHit?.OnDisable();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // _musicSource.Stop();
            switch(scene.name)
            {
                case "GameScene":
                    this.transform.parent = GameObject.Find("///Managers")?.transform;
                    if(_matchTimer.Value > 90)
                    {
                        _musicSource.clip = _musicClips[0];
                        break;
                    }
                    _musicSource.clip = _musicClips[1];
                    break;
                case "MainMenu":
                    _musicSource.clip = _musicClips[2];
                    break;
            }
            _musicSource.Play();
        }
        private void ChangeVolumenValue(object sender, System.ComponentModel.PropertyChangedEventArgs args)
        {
            if(sender.Equals(_musicVolumen))
            {
                _musicSource.volume = _musicVolumen.Value / 100;
                return;
            }
            _sfxSource.volume = _sfxVolumen.Value / 100;
            PlaySFX(_randomSFXClip, true);
        }
        ///<summary> Reproduce la pista de audio cuyo index es <paramref name="who"/> </summary>
        private void PlayerHit(Paint team)
        {
            int who = (int)team - 1;
            PlaySFX(who);
        }
        private void PlaySFX(int index, bool wait = false)
        {
            _sfxSource.clip = _sfxClips[index];
            if(!wait) { _sfxSource.Play(); return;}
            _sfxSource.PlayDelayed(0.1f);
        }
    }
}
