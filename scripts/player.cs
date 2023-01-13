using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;


public class player : MonoBehaviour
{
    // Player Body
    Rigidbody2D _player;
    float _walkSpeed = 7f;
    float _jumpSpeed = 10f;

    float _inputHorizontal;
    // get ground object to checked it's in groundLayer
    public Transform GroundCheck;
    // size of objectChecked
    public float GroundCheckRadius;
    // layer of ground
    public LayerMask groundLayer;
    bool _isTouchGround;

    // sprite rander (For player setting)
    private SpriteRenderer _spriteRendere;

    // Animator
    private Animator _anim;
    private Animator tst;

    // Fall Detector
    public GameObject fallDetector;

    // Player Respawn if he get fall
    private Vector2 _respawnPlace;
    private Vector3 _startPlace;

    // UI Text For score
    public Text scoreText;
    public Text TimerText;
    float currentTime = 150f;


    private bool _isCheckpointed = false;

    private List<GameObject> Enemy = new List<GameObject>();


    /* Sounds */
    [SerializeField] private AudioSource _BackgroundSound; // SerializeField => Force Unity to serialize a private field. to access it
    [SerializeField] private AudioSource _JumpEffect; // SerializeField => Force Unity to serialize a private field. to access it
    [SerializeField] private AudioSource _CollectionItemsEffect;
    [SerializeField] private AudioSource _EnemyDeath;
    [SerializeField] private AudioSource _CheckpointEffect;
    [SerializeField] private AudioSource _PlayerDeadEffect;
    [SerializeField] private AudioSource _FinishLevelEffect;
    [SerializeField] private AudioSource _FinishGameEffect = null;

    //[SerializeField] private AudioMixer _myMixer;



    // getter / setter
    public float WalkSpeed
    {
        get
        {
            return _walkSpeed;
        }
        set
        {
            _walkSpeed = value;
        }
    }
    public float JumpSpeed
    {
        get
        {
            return _jumpSpeed;
        }
        set
        {
            _jumpSpeed = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        _player = gameObject.GetComponent<Rigidbody2D>();
        _spriteRendere = gameObject.GetComponent<SpriteRenderer>();
        _anim = gameObject.GetComponent<Animator>();
        _startPlace = transform.position;
        _respawnPlace = transform.position;
        scoreText.text = "Score: " + Score.totalScore;
        TimerText.text = currentTime.ToString();
        _BackgroundSound.Play();
        // SaveData SData = SaveDataSystem.LoadPlayerData();
        // Vector2 Plposition;
        // Plposition.x = SData.Positions[0];
        // Plposition.y = SData.Positions[1];
        // transform.position = Plposition;


    }

    // Update is called once per frame
    void Update()
    {
        // checking if player touch the ground using OverlapCircle 
        _isTouchGround = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, groundLayer);
        // _inputHorizontal => return right 1 || stopped 0 || left -1
        _inputHorizontal = Input.GetAxisRaw("Horizontal");
        // check if player moved
        if (_inputHorizontal > 0)
        {
            // flip player to right 
            _spriteRendere.flipX = false;
            // Add Force to player by moving right and left (Horizontal direction)
            _player.velocity = new Vector2(_inputHorizontal * _walkSpeed, _player.velocity.y);
            // _player.AddForce(new Vector2(_inputHorizontal * _walkSpeed, 0f));
            _anim.SetBool("IsMove", true);
        }
        else if (_inputHorizontal < 0)
        {
            _anim.SetBool("IsMove", true);
            // flip player to left 
            _spriteRendere.flipX = true;
            _player.velocity = new Vector2(_inputHorizontal * _walkSpeed, _player.velocity.y);

        }
        else
        {
            _anim.SetBool("IsMove", false);
            _player.velocity = new Vector2(0, _player.velocity.y);
        }
        // _anim.SetFloat("Speed", Mathf.Abs(_player.velocity.x));
        if (Input.GetButtonDown("Jump"))
        {
            if (_isTouchGround)
            {
                // when player press the Jump button (edit/project setting/Input Manager) add force to Y position
                _player.velocity = new Vector2(_player.velocity.x, JumpSpeed * 5);
                // set true to Animator Condition for jamping animation
                _JumpEffect.Play();
            }
            _anim.SetBool("isJump", true);
        }
        else
        {

            _anim.SetBool("isJump", false);

        }
        // That line for fallDetector object to follow the player in only x poistion and stick on y position
        fallDetector.transform.position = new Vector2(transform.position.x, fallDetector.transform.position.y);

        // Change TimerText to +1 every Seconds not frame (That why we use Time.deltaTime)
        currentTime -= 1 * Time.deltaTime;
        TimerText.text = currentTime.ToString("000");

        if (currentTime < 0)
        {
            DeathPlayer();
            _anim.SetTrigger("Death");
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Enemy"))
        {
            if (!_isTouchGround && _player.velocity.y < 0)
            {
                _EnemyDeath.Play();
                collision.gameObject.SetActive(false);
                Enemy.Add(collision.gameObject);
                Score.totalScore += 1;
                scoreText.text = "Score: " + Score.totalScore;
            }
            else
            {
                _anim.SetTrigger("Death");
                DeathPlayer();


            }
        }
        else if (collision.CompareTag("DeathMachine"))
        {
            _anim.SetTrigger("Death");
            DeathPlayer();

        }
        else if (collision.CompareTag("FallDetector"))
        {
            DeathPlayer();
            _anim.SetTrigger("Death");


        }
        else if (collision.CompareTag("checkpoint"))
        {
            if (!_isCheckpointed)
            {
                _CheckpointEffect.Play();
            }
            _respawnPlace = transform.position;
            _isCheckpointed = true;
            SaveData SData = SaveDataSystem.LoadPlayerData();
            Debug.Log(SData.death);
        }
        else if (collision.CompareTag("Portal"))
        {
            Levels.Level += 1;
            // Finish Level Sound
            _FinishLevelEffect.Play();
            _player.bodyType = RigidbodyType2D.Static;
            // Invokes the method methodName in time seconds. (methodName, seconds)
            Invoke("CompleteLevel", 1.5f);
            _respawnPlace = transform.position;


        }
        else if (collision.CompareTag("CarrotItem"))
        {
            Destroy(collision.gameObject);
            Score.totalScore += 4;
            scoreText.text = "Score: " + Score.totalScore;
            _CollectionItemsEffect.Play();

        }
        else if (collision.CompareTag("gem"))
        {
            Destroy(collision.gameObject);
            Score.totalScore += 2;
            scoreText.text = "Score: " + Score.totalScore;
            _CollectionItemsEffect.Play();

        }
        else if (collision.CompareTag("star"))
        {
            Destroy(collision.gameObject);
            Score.totalScore += 50;
            scoreText.text = "Score: " + Score.totalScore;
            _CollectionItemsEffect.Play();

        }
        else if (collision.CompareTag("Finish"))
        {
            _FinishGameEffect.Play();
            _BackgroundSound.Pause();
            _player.bodyType = RigidbodyType2D.Static;
            // Invokes the method methodName in time seconds. (methodName, seconds)
            Invoke("CompleteLevel", 5.5f);

        }


    }

    void Respawn()
    {
        _player.transform.position = _respawnPlace;
        _player.bodyType = RigidbodyType2D.Dynamic;
        for (int i = 0; i < Enemy.Count; i++)
        {
            Enemy[i].SetActive(true);
        }
    }
    private void CompleteLevel()
    {
        // Gets the currently active Scene + 1 //=> To next Level!!
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SaveDataSystem.SavePlayerData(this);
        _player.bodyType = RigidbodyType2D.Dynamic;
        _respawnPlace = transform.position;
    }
    private void DeathPlayer()
    {
        _PlayerDeadEffect.Play();
        _player.bodyType = RigidbodyType2D.Static;
        currentTime = 150f;
        DeathTime.TotalDeath += 1;
    }
    public void LoadDataPlayer()
    {
        Time.timeScale = 1f;
        SaveData SData = SaveDataSystem.LoadPlayerData();
        Debug.Log(SData.level);
        DeathTime.TotalDeath = SData.death;
        Score.totalScore = SData.Tscore;
        if (SData.level == 1)
        {
            DeathTime.TotalDeath = 0;
            Score.totalScore = 0;
        }
        SceneManager.LoadScene(SData.level);
    }

}



