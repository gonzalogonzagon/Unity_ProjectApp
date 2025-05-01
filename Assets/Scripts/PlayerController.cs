using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float turnSpeed = 3f;
    [SerializeField] private float verticalSpeed = 3f;
    [SerializeField] private float jumpForce = 5f;
    
    private Camera playerCamera;
    private CharacterController controller;
    private float verticalVelocity = 0f;
    private float rotationX = 0f;
    private Vector3 moveDirection = Vector3.zero;
    
    void Start()
    {
        // Obtener la cámara hijo
        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            Debug.LogError("No se encontró una cámara hijo en el objeto del jugador");
        }
        
        // Obtener el CharacterController si está presente
        controller = GetComponent<CharacterController>();
    }
    
    void Update()
    {
        // Rotar la cámara para mirar arriba/abajo
        float mouseY = Input.GetAxis("Mouse Y");
        rotationX -= mouseY * turnSpeed;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        
        // Rotar el jugador completo según la rotación horizontal de la cámara
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(0, mouseX * turnSpeed, 0);
        
        // Calcular movimiento hacia adelante/atrás y lateral basado en la dirección del jugador
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        
        // Movimiento relativo a la orientación del jugador
        Vector3 move = transform.forward * vertical + transform.right * horizontal;
        
        // Normalizar para evitar velocidad diagonal más rápida
        if (move.magnitude > 1f)
            move = move.normalized;
        
        // Movimiento vertical (p.e. salto o gravedad)
        if (controller != null)
        {
            // Con CharacterController
            HandleVerticalMovementWithController(move);
        }
        else
        {
            // Sin CharacterController, movimiento básico 
            HandleBasicMovement(move);
        }
    }
    
    private void HandleVerticalMovementWithController(Vector3 horizontalMove)
    {
        // Aplicar gravedad
        if (controller.isGrounded)
        {
            verticalVelocity = -0.5f; // Forzar al controlador a estar en el suelo
            
            // Saltar si se presiona el botón
            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            // Aplicar gravedad mientras esté en el aire
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        
        // Movimiento vertical adicional con teclas Q/E si se desea
        if (Input.GetKey(KeyCode.Q))
            verticalVelocity = verticalSpeed;
        else if (Input.GetKey(KeyCode.E))
            verticalVelocity = -verticalSpeed;
        
        // Aplicar movimiento final al CharacterController
        Vector3 finalMove = horizontalMove * moveSpeed;
        finalMove.y = verticalVelocity;
        
        controller.Move(finalMove * Time.deltaTime);
    }
    
    private void HandleBasicMovement(Vector3 horizontalMove)
    {
        // Movimiento sencillo con Transform para objetos sin CharacterController
        transform.position += horizontalMove * moveSpeed * Time.deltaTime;
        
        // Movimiento vertical con Q/E
        float verticalMovement = 0f;
        if (Input.GetKey(KeyCode.Q))
            verticalMovement = 1f;
        else if (Input.GetKey(KeyCode.E))
            verticalMovement = -1f;
            
        transform.position += Vector3.up * verticalMovement * verticalSpeed * Time.deltaTime;
    }
}