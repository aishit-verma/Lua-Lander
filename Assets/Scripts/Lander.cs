using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    private const float GRAVITY_NORMAL = 0.7f;
    public static Lander Instance { get; private set; }
    private Rigidbody2D landerRigidbody2D;
    public event EventHandler OnUpForce;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickup;
    public event EventHandler OnFuelPickup;
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public event EventHandler<OnLandedEventArgs> OnLanded;
    public class OnLandedEventArgs : EventArgs
    {
        public LandingType landingType;
        public int score;
        public float dotVector;
        public float landingSpeed;
        public float scoreMultiplier;
    }
    public enum LandingType
    {
        Successful,
        Crashed,
        TooSteepAngle,
        TooFast,
    }
    public enum State
    {
        WaitingToStart,
        Normal,
        GameOver,
    }
    private float fuelAmountMax = 10f;
    private float fuelAmount;
    private State state;

    private void Awake()
    {
        Instance = this;
        fuelAmount = fuelAmountMax;
        state = State.WaitingToStart;
        landerRigidbody2D = GetComponent<Rigidbody2D>();
        landerRigidbody2D.gravityScale = 0f;
    }
    

    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        switch (state)
        {
            default:
            case State.WaitingToStart:
                if (GameInputs.Instance.IsUpPressed() || GameInputs.Instance.IsRightPressed()|| GameInputs.Instance.IsLeftPressed()
                ||GameInputs.Instance.GetMovementVector() != Vector2.zero)
                {
                    landerRigidbody2D.gravityScale = GRAVITY_NORMAL;
                    state = State.Normal;
                    SetState(State.Normal);
                }
                break;
            case State.Normal:
                if (fuelAmount <= 0f)
                {
                    return;
                }
                if (GameInputs.Instance.IsUpPressed() || GameInputs.Instance.IsRightPressed()|| GameInputs.Instance.IsLeftPressed()
                ||GameInputs.Instance.GetMovementVector() != Vector2.zero)
                {
                    ConsumeFuel();
                }
                float deadzone = 0.2f;
                if (GameInputs.Instance.IsUpPressed()|| GameInputs.Instance.GetMovementVector().y > deadzone)
                {
                    float force = 700f;
                    landerRigidbody2D.AddForce(force * transform.up * Time.deltaTime);
                    OnUpForce?.Invoke(this, EventArgs.Empty);
                }
                if (GameInputs.Instance.IsLeftPressed()|| GameInputs.Instance.GetMovementVector().x < -deadzone)
                {
                    float turnspeed = 100f;
                    landerRigidbody2D.AddTorque(turnspeed * Time.deltaTime);
                    OnLeftForce?.Invoke(this, EventArgs.Empty);
                }
                if (GameInputs.Instance.IsRightPressed()|| GameInputs.Instance.GetMovementVector().x > deadzone)
                {
                    float turnspeed = 100f;
                    landerRigidbody2D.AddTorque(-turnspeed * Time.deltaTime);
                    OnRightForce?.Invoke(this, EventArgs.Empty);
                }
                break;
                case State.GameOver:
                break;
        }

    }
    private void SetState(State state)
    {
        this.state = state;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
    }



    void OnCollisionEnter2D(Collision2D collision)
    {
        float minDotVector = .9f;
        float dotProduct = Vector2.Dot(transform.up, Vector2.up);
        float softLandingVelocityMagnitude = 4f;
        if (collision.gameObject.TryGetComponent(out LandingPad landingPad) == false)
        {
            Debug.Log("Crash! on Terrain");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.Crashed,
                score = 0,
                dotVector = 0f,
                landingSpeed = 0f,
                scoreMultiplier = 0f
            });
            SetState(State.GameOver);
            return;
        }

        if (collision.relativeVelocity.magnitude > softLandingVelocityMagnitude)
        {
            Debug.Log("Crash!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooFast,
                score = 0,
                dotVector = 0f,
                landingSpeed = 0f,
                scoreMultiplier = 0f
            });
            SetState(State.GameOver);
            return;
        }


        if (dotProduct < minDotVector)
        {
            Debug.Log("Landed at an steep angle!");
            OnLanded?.Invoke(this, new OnLandedEventArgs
            {
                landingType = LandingType.TooSteepAngle,
                score = 0,
                dotVector = dotProduct,
                landingSpeed = collision.relativeVelocity.magnitude,
                scoreMultiplier = landingPad.GetScoreMultiplier()
            });
            SetState(State.GameOver);
            return;
        }
        Debug.Log("Successful Landing!");
        int score = 50 * landingPad.GetScoreMultiplier();
        Debug.Log($"Score: {score}");
        OnLanded?.Invoke(this, new OnLandedEventArgs
        {
            landingType = LandingType.Successful,
            score = score,
            dotVector = dotProduct,
            landingSpeed = collision.relativeVelocity.magnitude,
            scoreMultiplier = landingPad.GetScoreMultiplier()
        });
        SetState(State.GameOver);


    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out FuelPickup fuelPickup))
        {
            float fuelAmountToAdd = 10f;
            fuelAmount += fuelAmountToAdd;
            if (fuelAmount > fuelAmountMax)
            {
                fuelAmount = fuelAmountMax;
            }
            OnFuelPickup?.Invoke(this, EventArgs.Empty);
            fuelPickup.DestroySelf();
        }
        if (collision.gameObject.TryGetComponent(out CoinPickup coinPickup))
        {
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            coinPickup.DestroySelf();
        }

    }
    private void ConsumeFuel()
    {
        float fuelConsumptionRate = 1f;
        fuelAmount -= fuelConsumptionRate * Time.deltaTime;
    }

    public float GetSpeedX()
    {
        return landerRigidbody2D.linearVelocityX;
    }
    public float GetSpeedY()
    {
        return landerRigidbody2D.linearVelocityY;
    }
    public float GetFuelAmount()
    {
        return fuelAmount;
    }
    public float GetFuelAmountNormalized()
    {
        return fuelAmount / fuelAmountMax;
    }
}
