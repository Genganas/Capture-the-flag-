using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    public Camera playerCamera;
    public GameObject bulletPrefab; // Prefab for the bullet
    public Transform bulletSpawnPoint; // The position from where the bullet will spawn
    public float bulletSpeed = 30f; // Speed of the bullet

    public int maxAmmo = 30;
    public int currentAmmo;
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;

    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;

    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    public int maxHealth = 100;
    public int currentHealth;

    private float originalWalkSpeed;
    private float originalRunSpeed;
    private bool speedPowerUpActive = false;
    private float speedPowerUpEndTime = 0f;
    private float speedIncreaseAmount = 2f; 
    public AudioSource shootingSound;
    public AudioSource reloadSound;
    public AudioSource speedSound;

    public Text AmmoText;

    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;

    CharacterController characterController;

    private Flag carriedFlag; 


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
      
        currentHealth = maxHealth;
        originalWalkSpeed = walkSpeed;
        originalRunSpeed = runSpeed;
        currentAmmo = maxAmmo; 
    }

    void Update()
    {
        #region Handles Movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Handles jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
        #endregion

        #region Handles Rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        #endregion

        #region Handles Shooting with Ammo
        if (Input.GetButton("Fire1") && Time.time >= nextFireTime && currentAmmo > 0)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
        #endregion

        #region Health System
        if (currentHealth <= 0)
        {
            Die();
        }
        #endregion

        #region Speed Power-Up
        if (speedPowerUpActive && Time.time < speedPowerUpEndTime)
        {
            walkSpeed = originalWalkSpeed + speedIncreaseAmount;
            runSpeed = originalRunSpeed + speedIncreaseAmount;
        }
        else
        {
            walkSpeed = originalWalkSpeed;
            runSpeed = originalRunSpeed;
            speedPowerUpActive = false;
        }
        #endregion
    }

    [SerializeField] private Transform gunModel; // Reference to the gun model transform
    [SerializeField] private float recoilDistance = 0.1f; // Distance the gun moves during recoil
    [SerializeField] private float recoilSpeed = 10f; // Speed at which the gun returns to its original position

    void Shoot()
    {
        // Instantiate a bullet at the bullet spawn point
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

        // Get the Rigidbody component of the bullet
        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
        // Play the shooting sound
        if (shootingSound != null)
        {
            shootingSound.Play();
        }

        // Ensure bulletRigidbody is not null and apply velocity
        if (bulletRigidbody != null)
        {
            // Calculate bullet direction based on camera's forward direction
            Vector3 bulletDirection = playerCamera.transform.forward;

            // Apply velocity to the bullet
            bulletRigidbody.velocity = bulletDirection * bulletSpeed;

            // Reduce ammo count
            currentAmmo--;
            AmmoText.text = currentAmmo.ToString();
            // Debug for ammo count
          

            // Apply recoil effect to the gun model
            StartCoroutine(RecoilEffect());
        }
    }

    IEnumerator RecoilEffect()
    {
        // Move the gun model back
        Vector3 originalPosition = gunModel.localPosition;
        Vector3 recoilPosition = originalPosition - new Vector3(0, 0, recoilDistance);

        float elapsedTime = 0f;

        while (elapsedTime < 0.1f) // Adjust this time duration as needed
        {
            elapsedTime += Time.deltaTime;
            gunModel.localPosition = Vector3.Lerp(recoilPosition, originalPosition, elapsedTime * recoilSpeed);
            yield return null;
        }
    }


    // Method to reload ammo (called by the ammo power-up)
    public void ReloadAmmo()
    {
        reloadSound.Play();
        currentAmmo = maxAmmo;
       
    }

    void Die()
    {
        // Implement your game over logic here, such as restarting the level or showing a game over screen.
 
    }

    public void ApplySpeedPowerUp(float speedIncreaseAmount, float duration)
    {
        speedSound.Play();
        this.speedIncreaseAmount = speedIncreaseAmount;
        this.speedPowerUpActive = true;
        this.speedPowerUpEndTime = Time.time + duration;
    }

    void OnTriggerEnter(Collider other)
    {
        if (carriedFlag == null) // Only pick up the flag if not already carrying one
        {
            Flag flag = other.GetComponent<Flag>();
            if (flag != null)
            {
                if ((flag.flagType == Flag.FlagType.Blue && tag == "PlayerBlue") ||
                    (flag.flagType == Flag.FlagType.Red && tag == "PlayerRed"))
                {
                    PickUpFlag(flag);
                }
            }
        }
        else // Drop the flag if the player enters their base area
        {
            if ((other.CompareTag("BlueBase") && tag == "PlayerBlue" && carriedFlag.flagType == Flag.FlagType.Blue) ||
                (other.CompareTag("RedBase") && tag == "PlayerRed" && carriedFlag.flagType == Flag.FlagType.Red))
            {
                DropFlag();
            }
        }
    }


    void PickUpFlag(Flag flag)
    {
        flag.transform.parent = transform; // Attach flag to the player
        flag.transform.localPosition = new Vector3(0f, 1f, 2f); // Adjust flag position relative to player
        carriedFlag = flag;
    }

    public void DropFlag()
    {
        if (carriedFlag != null)
        {
            carriedFlag.transform.parent = null; // Detach flag from the player
            carriedFlag = null;
        }
    }

}
