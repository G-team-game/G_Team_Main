using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class PlayerCam : MonoBehaviour
{
    private float sensX;
    private float sensY;

    PlayerInputSystem inputSystem;

    public Transform orientation;

    public Vector2 camDirection;

    float xRotation;
    float yRotation;

    private void Awake()
    {
        inputSystem = new PlayerInputSystem();
    }

    private void Start()
    {
        startPos = transform.localPosition;
    }

    private void OnEnable()
    {
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

    [SerializeField] private ParticleSystem effect;
    [SerializeField] private Volume volume;
    private ChromaticAberration chromaticAberration;
    private LensDistortion lensDistortion;
    Tweener tweener, tweener1, tweener2;
    Vector3 startPos;
    public void CameraShake(bool isStop)
    {
        if (isStop == true)
        {
            if (tweener != null) tweener.Kill();
            if (tweener1 != null) tweener1.Kill();
            if (tweener2 != null) tweener2.Kill();

            transform.localPosition = startPos;
            effect.gameObject.SetActive(false);

            if (volume.profile.TryGet<ChromaticAberration>(out chromaticAberration))
            {
                chromaticAberration.intensity.value = 0;
            }

            if (volume.profile.TryGet<LensDistortion>(out lensDistortion))
            {
                lensDistortion.intensity.value = 0;
            }
            return;
        }

        tweener = transform.DOShakePosition(0.1f, 0.3f, 20).SetLoops(-1, LoopType.Restart);

        if (volume.profile.TryGet<ChromaticAberration>(out chromaticAberration))
        {
            tweener1 = DOTween.To(() => chromaticAberration.intensity.value, (v) => chromaticAberration.intensity.value = v, 1, 0.3f);
        }

        if (volume.profile.TryGet<LensDistortion>(out lensDistortion))
        {
            tweener2 = DOTween.To(() => lensDistortion.intensity.value, (v) => lensDistortion.intensity.value = v, -0.5f, 1.0f);
        }

        effect.gameObject.SetActive(true);
    }

    public void Look(Vector2 camDirection)
    {
        if (Gamepad.current != null)
        {
            sensX = 200f;
            sensY = 200f;
        }
        else
        {
            sensX = 100f;
            sensY = 100f;
        }

        // get mouse input
        float X = camDirection.x * Time.deltaTime * sensX;
        float Y = camDirection.y * Time.deltaTime * sensY;

        yRotation += X;

        xRotation -= Y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}