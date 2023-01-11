using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Inst { get; private set; }

    // [SerializeField] private int FixedUpdateCount = 0;

    [SerializeField] private int DelayConsecutive = 25;

    protected PlayerInputs _playerInputs = new();
    public static PlayerInputs PlayerInputs => Inst._playerInputs;

    private InputActions _playerActions;
    private InputAction _move, _moveUp, _moveDown, 
        _cursorPosition, _cursorPositionDelta, _scroll, 
        _action1, _interaction, _inventory;

    private void OnEnable()
    {
        Inst = this;
        SetupAction();
        _playerActions.Enable();
    }
    private void Update()
    {        
        // Debug.Log($"{_playerInputs.Interact.Live} {_playerInputs.Interact.Last}");
        // if (_playerInputs.Interact.OnUp) Debug.Log($"Unfixed OnUp");
        UpdateData();
    }
    private void FixedUpdate()
    {
        UpdateTimer();
        // Debug.Log($"{_playerInputs.CursorPosition} {PlayerInputs.CursorPosition}");
    }

    private void SetupAction()
    {
        _playerActions = new InputActions();
        
        _move = _playerActions.Player.Move;
        _moveUp = _playerActions.Player.MoveUp;
        _moveDown = _playerActions.Player.MoveDown;
        
        _cursorPosition = _playerActions.Player.CursorPosition;
        _cursorPositionDelta = _playerActions.Player.CursorPositionDelta;
        _scroll = _playerActions.Player.Scroll;
        
        _action1 = _playerActions.Player.Action1;
        _interaction = _playerActions.Player.Interact;
        _inventory = _playerActions.Player.Inventory;
    }
    
    public void UpdateData()
    {
        Vector2 hor = _move.ReadValue<Vector2>();
        float vect = _moveUp.ReadValue<float>() * 1 + _moveDown.ReadValue<float>() * -1;
        Vector3InputDataUpdate(ref _playerInputs.Movement, new Vector3(hor.x, vect, hor.y));
        
        Vector2InputDataUpdate(ref _playerInputs.Movement2d, hor);

        Vector2InputDataUpdate(ref _playerInputs.CursorPositionDelta, _cursorPositionDelta.ReadValue<Vector2>());

        _playerInputs.CursorPosition = _cursorPosition.ReadValue<Vector2>();
        
        KeyInputDataUpdate(ref _playerInputs.Jump, _moveUp.ReadValue<float>() != 0f);

        KeyInputDataUpdate(ref _playerInputs.Interact, _interaction.ReadValue<float>() != 0f);
        
        KeyInputDataUpdate(ref _playerInputs.Inventory, _inventory.ReadValue<float>() != 0f);
        
        KeyInputDataUpdate(ref _playerInputs.Action1, _action1.ReadValue<float>() != 0f);
    }

    public void UpdateTimer()
    {
        Vector3InputDataUpdateTimer(ref _playerInputs.Movement);
        
        Vector2InputDataUpdateTimer(ref _playerInputs.Movement2d);
        
        Vector2InputDataUpdateTimer(ref _playerInputs.CursorPositionDelta);
        
        //
        
        KeyInputDataUpdateTimer(ref _playerInputs.Jump);
        
        KeyInputDataUpdateTimer(ref _playerInputs.Interact);
        
        KeyInputDataUpdateTimer(ref _playerInputs.Inventory);
        
        KeyInputDataUpdateTimer(ref _playerInputs.Action1);
    }

    #region Checks and Sets
    
    private void Vector3InputDataUpdate(ref Vector3Input data, Vector3 liveInput)
    {
        data.Last = data.Live;
        data.Live = liveInput;

        data.OnDown.x = data.Live.x != 0f && data.Last.x == 0f ? data.Live.x : 0f;
        data.OnDown.y = data.Live.y != 0f && data.Last.y == 0f ? data.Live.y : 0f;
        data.OnDown.z = data.Live.z != 0f && data.Last.z == 0f ? data.Live.z : 0f;
        
        data.OnUp.x = data.Last.x != 0f && data.Live.x == 0f ? data.Last.x : 0f;
        data.OnUp.y = data.Last.y != 0f && data.Live.y == 0f ? data.Last.y : 0f;
        data.OnUp.z = data.Last.z != 0f && data.Live.z == 0f ? data.Last.z : 0f;
        
        data.Timer.x = data.Live.x != 0f ? 0 : data.Timer.x;
        data.Timer.y = data.Live.y != 0f ? 0 : data.Timer.y;
        data.Timer.z = data.Live.z != 0f ? 0 : data.Timer.z;
        
        if (data.ConsecutiveTapsStep.x is 0 or 2 && data.OnDown.x != 0f && data.Timer.x < DelayConsecutive) data.ConsecutiveTapsStep.x++;
        if (data.ConsecutiveTapsStep.x is 1 or 3 && data.OnUp.x != 0f && data.Timer.x < DelayConsecutive) data.ConsecutiveTapsStep.x++;
        
        if (data.ConsecutiveTapsStep.y is 0 or 2 && data.OnDown.y != 0f && data.Timer.y < DelayConsecutive) data.ConsecutiveTapsStep.y++;
        if (data.ConsecutiveTapsStep.y is 1 or 3 && data.OnUp.y != 0f && data.Timer.y < DelayConsecutive) data.ConsecutiveTapsStep.y++;
        
        if (data.ConsecutiveTapsStep.z is 0 or 2 && data.OnDown.z != 0f && data.Timer.z < DelayConsecutive) data.ConsecutiveTapsStep.z++;
        if (data.ConsecutiveTapsStep.z is 1 or 3 && data.OnUp.z != 0f && data.Timer.z < DelayConsecutive) data.ConsecutiveTapsStep.z++;

        if (data.Timer.x >= DelayConsecutive) data.ConsecutiveTapsStep.x = 0;
        if (data.Timer.y >= DelayConsecutive) data.ConsecutiveTapsStep.y = 0;
        if (data.Timer.z >= DelayConsecutive) data.ConsecutiveTapsStep.z = 0;
        
        data.ConsecutiveTaps.x = data.ConsecutiveTapsStep.x > 3 ? data.Last.x : 0f;
        data.ConsecutiveTaps.y = data.ConsecutiveTapsStep.y > 3 ? data.Last.y : 0f;
        data.ConsecutiveTaps.z = data.ConsecutiveTapsStep.z > 3 ? data.Last.z : 0f;

        data.ConsecutiveTapsStep.x = data.ConsecutiveTapsStep.x > 3 ? 0 : data.ConsecutiveTapsStep.x;
        data.ConsecutiveTapsStep.y = data.ConsecutiveTapsStep.y > 3 ? 0 : data.ConsecutiveTapsStep.y;
        data.ConsecutiveTapsStep.z = data.ConsecutiveTapsStep.z > 3 ? 0 : data.ConsecutiveTapsStep.z;
    }
    private void Vector2InputDataUpdate(ref Vector2Input data, Vector2 liveInput)
    {
        data.Last = data.Live;
        data.Live = liveInput;

        data.OnDown.x = data.Live.x != 0f && data.Last.x == 0f ? data.Live.x : 0f;
        data.OnDown.y = data.Live.y != 0f && data.Last.y == 0f ? data.Live.y : 0f;
        
        data.OnUp.x = data.Last.x != 0f && data.Live.x == 0f ? data.Last.x : 0f;
        data.OnUp.y = data.Last.y != 0f && data.Live.y == 0f ? data.Last.y : 0f;
        
        data.Timer.x = data.Live.x != 0f ? 0 : data.Timer.x;
        data.Timer.y = data.Live.y != 0f ? 0 : data.Timer.y;
        
        if (data.ConsecutiveTapsStep.x is 0 or 2 && data.OnDown.x != 0f && data.Timer.x < DelayConsecutive) data.ConsecutiveTapsStep.x++;
        if (data.ConsecutiveTapsStep.x is 1 or 3 && data.OnUp.x != 0f && data.Timer.x < DelayConsecutive) data.ConsecutiveTapsStep.x++;
        
        if (data.ConsecutiveTapsStep.y is 0 or 2 && data.OnDown.y != 0f && data.Timer.y < DelayConsecutive) data.ConsecutiveTapsStep.y++;
        if (data.ConsecutiveTapsStep.y is 1 or 3 && data.OnUp.x != 0f && data.Timer.y < DelayConsecutive) data.ConsecutiveTapsStep.y++;
        
        if (data.Timer.x >= DelayConsecutive) data.ConsecutiveTapsStep.x = 0;
        if (data.Timer.y >= DelayConsecutive) data.ConsecutiveTapsStep.y = 0;
        
        data.ConsecutiveTaps.x = data.ConsecutiveTapsStep.x > 3 ? data.Last.x : 0f;
        data.ConsecutiveTaps.y = data.ConsecutiveTapsStep.y > 3 ? data.Last.y : 0f;

        data.ConsecutiveTapsStep.x = data.ConsecutiveTapsStep.x > 3 ? 0 : data.ConsecutiveTapsStep.x;
        data.ConsecutiveTapsStep.y = data.ConsecutiveTapsStep.y > 3 ? 0 : data.ConsecutiveTapsStep.y;
    }
    private void KeyInputDataUpdate(ref KeyInput data, bool liveInput)
    {
        data.Last = data.Live;
        
        data.Live = liveInput;

        data.OnDown = data.Live && !data.Last;
        data.OnDownTimer = data.Live && !data.Last ? 0 : data.OnDownTimer;
        data.FixedOnDown = data.OnDownTimer < 1;
        
        data.OnUp = !data.Live && data.Last;
        data.OnUpTimer = !data.Live && data.Last ? 0 : data.OnUpTimer;
        data.FixedOnUp = data.OnUpTimer < 1;
        
        data.ConsecutiveTapsTimer = data.Live ? 0 : data.ConsecutiveTapsTimer;

        if (data.ConsecutiveTapsStep is 0 or 2 && data.OnDownTimer == 0 && data.ConsecutiveTapsTimer < DelayConsecutive) data.ConsecutiveTapsStep++;
        if (data.ConsecutiveTapsStep is 1 or 3 && data.OnUpTimer == 0 && data.ConsecutiveTapsTimer < DelayConsecutive) data.ConsecutiveTapsStep++;
        
        if (data.ConsecutiveTapsTimer >= DelayConsecutive) data.ConsecutiveTapsStep = 0;
        
        data.ConsecutiveTaps = data.ConsecutiveTapsStep > 3;
        
        data.ConsecutiveTapsStep = data.ConsecutiveTapsStep > 3 ? 0 : data.ConsecutiveTapsStep;
    }

    private void Vector2InputDataUpdateTimer(ref Vector2Input data)
    {
        data.Timer.x += 1;
        data.Timer.y += 1;
    }
    private void Vector3InputDataUpdateTimer(ref Vector3Input data)
    {
        data.Timer.x += 1;
        data.Timer.y += 1;
        data.Timer.z += 1;
    }
    private void KeyInputDataUpdateTimer(ref KeyInput data)
    {
        data.OnDownTimer += 1;
        data.OnUpTimer += 1;
        data.ConsecutiveTapsTimer += 1;
    }

    #endregion

}

public struct PlayerInputs
{
    public Vector3Input Movement;
    public Vector2Input Movement2d;
    public Vector2Input CursorPositionDelta;
    public Vector2 CursorPosition;
    // public KeyInput Dash;
    public KeyInput Jump;
    public KeyInput Interact;
    public KeyInput Inventory;
    public KeyInput Action1;
}

public struct Vector2Input
{
    public Vector2 OnDown;
    public Vector2 OnUp;
    
    public Vector2 Last;
    public Vector2 Live;
    
    public Vector2Int Timer;
    public Vector2Int ConsecutiveTapsStep;
    public Vector2 ConsecutiveTaps;
}
public struct Vector3Input
{
    public Vector3 OnDown;
    public Vector3 OnUp;
    
    public Vector3 Last;
    public Vector3 Live;
    
    public Vector3Int Timer;
    public Vector3Int ConsecutiveTapsStep;
    public Vector3 ConsecutiveTaps;
}
public struct KeyInput
{
    public int OnDownTimer;
    public bool FixedOnDown;
    public bool OnDown;
    
    public int OnUpTimer;
    public bool FixedOnUp;
    public bool OnUp;
    
    public bool Last;
    public bool Live;
    
    public int ConsecutiveTapsTimer;
    public int ConsecutiveTapsStep;
    public bool ConsecutiveTaps;
}
