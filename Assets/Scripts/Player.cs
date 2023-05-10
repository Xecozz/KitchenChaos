using System;
using Counters;
using Kitchen;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    public event EventHandler OnPickedSomething;
    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectCounterChanged;

    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public BaseCounter SelectedCounter;
    }

    private bool isWalking;
    private Vector3 lastInteractPosition;
    private Vector3 lastInteractionDir;
    private BaseCounter selectCounter;
    private KitchenObject kitchenObject;

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("More than one instance of Player found!");
        }

        Instance = this;
    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
            if (selectCounter != null) {
            selectCounter.Interact(this);
        }
    }

    private void GameInput_OnInteractAlternateAction(object sender, EventArgs e) {
        if (!KitchenGameManager.Instance.IsGamePlaying()) return;
        if (selectCounter != null) {
            selectCounter.InteractAlternate(this);
        }
    }

    private void Update() {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking() {
        return isWalking;
    }

    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);


        if (moveDir != Vector3.zero) {
            lastInteractionDir = moveDir;
        }

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, lastInteractionDir, out RaycastHit hit, interactDistance,
                countersLayerMask)) {
            if (hit.transform.TryGetComponent(out BaseCounter baseCounter)) {
                // interact with clear counter
                if (baseCounter != selectCounter) {
                    SelectedCounter(baseCounter);
                }
            }
            else {
                SelectedCounter(null);
            }
        }
        else {
            SelectedCounter(null);
        }
    }


    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        // for moving in xz plane
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = 0.7f;
        float playerHeight = 2f;
        var position = transform.position;
        bool canMove = !Physics.CapsuleCast(position, position + Vector3.up * playerHeight, playerRadius, moveDir,
            moveDistance);

        if (!canMove) {
            //cannot move towards moveDir
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(position, position + Vector3.up * playerHeight,
                playerRadius, moveDirX,
                moveDistance);

            if (canMove) {
                //can move only x
                moveDir = moveDirX;
            }
            else {
                //move only z
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(position, position + Vector3.up * playerHeight,
                    playerRadius,
                    moveDirZ,
                    moveDistance);

                if (canMove) {
                    moveDir = moveDirZ;
                }
                else {
                    //move in xz plane
                }
            }
        }

        if (canMove) {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;

        // smooth rotation
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }


    private void SelectedCounter(BaseCounter baseCounter) {
        selectCounter = baseCounter;

        OnSelectCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { SelectedCounter = selectCounter });
    }


    public Transform GetKitchenObjectFollowTransform() => kitchenObjectHoldPoint;

    public void SetKitchenObject(KitchenObject kitchenObject) {
        this.kitchenObject = kitchenObject;
        
        if (kitchenObject != null) {
            OnPickedSomething?.Invoke(this, EventArgs.Empty);
        }
    }

    public KitchenObject GetKitchenObject() => kitchenObject;

    public void ClearKitchenObject() {
        kitchenObject = null;
    }

    public bool HasKitchenObject() => kitchenObject != null;
}