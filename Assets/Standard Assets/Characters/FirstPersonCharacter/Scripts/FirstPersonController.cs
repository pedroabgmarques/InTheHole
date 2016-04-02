using System;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField] private bool mIsWalking;
        [SerializeField] private float mWalkSpeed;
        [SerializeField] private float mRunSpeed;
        [SerializeField] [Range(0f, 1f)] private float mRunstepLenghten;
        [SerializeField] private float mJumpSpeed;
        [SerializeField] private float mStickToGroundForce;
        [SerializeField] private float mGravityMultiplier;
        [SerializeField] private MouseLook mMouseLook;
        [SerializeField] private bool mUseFovKick;
        [SerializeField] private FOVKick mFovKick = new FOVKick();
        [SerializeField] private bool mUseHeadBob;
        [SerializeField] private CurveControlledBob mHeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob mJumpBob = new LerpControlledBob();
        [SerializeField] private float mStepInterval;

        [SerializeField] private AudioClip[] mFootstepSounds;
        // an array of footstep sounds that will be randomly selected from.

        [SerializeField] private AudioClip mJumpSound; // the sound played when character leaves the ground.
        [SerializeField] private AudioClip mLandSound; // the sound played when character touches back on ground.

        private Camera mCamera;
        private bool mJump;
        private float mYRotation;
        private Vector2 mInput;
        private Vector3 mMoveDir = Vector3.zero;
        private CharacterController mCharacterController;
        private CollisionFlags mCollisionFlags;
        private bool mPreviouslyGrounded;
        private Vector3 mOriginalCameraPosition;
        private float mStepCycle;
        private float mNextStep;
        private bool mJumping;
        private AudioSource mAudioSource;



        public RawImage FadeImage;
        private bool isAtTheDoor = false;

        #region Métodos MonoBehaviour

        // Start
        private void Start()
        {
            mCharacterController = GetComponent<CharacterController>();
            mCamera = Camera.main;
            mOriginalCameraPosition = mCamera.transform.localPosition;
            mFovKick.Setup(mCamera);
            mHeadBob.Setup(mCamera, mStepInterval);
            mStepCycle = 0f;
            mNextStep = mStepCycle/2f;
            mJumping = false;
            mAudioSource = GetComponent<AudioSource>();
            mMouseLook.Init(transform, mCamera.transform);

            FadeImage.CrossFadeAlpha(0.0f, 0.00001f, true);
        }

        // Update
        private void Update()
        {
            RotateView();
            // the jump state needs to read here to make sure it is not missed
            if (!mJump)
            {
                mJump = CrossPlatformInputManager.GetButtonDown("Jump");
            }

            if (!mPreviouslyGrounded && mCharacterController.isGrounded)
            {
                StartCoroutine(mJumpBob.DoBobCycle());
                PlayLandingSound();
                mMoveDir.y = 0f;
                mJumping = false;
            }
            if (!mCharacterController.isGrounded && !mJumping && mPreviouslyGrounded)
            {
                mMoveDir.y = 0f;
            }

            mPreviouslyGrounded = mCharacterController.isGrounded;
        }

        // Fixed Update
        private void FixedUpdate()
        {
            float _speed;
            GetInput(out _speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 _desiredMove = transform.forward*mInput.y + transform.right*mInput.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit _hitInfo;
            Physics.SphereCast(transform.position, mCharacterController.radius, Vector3.down, out _hitInfo,
                mCharacterController.height/2f, ~0, QueryTriggerInteraction.Ignore);
            _desiredMove = Vector3.ProjectOnPlane(_desiredMove, _hitInfo.normal).normalized;

            mMoveDir.x = _desiredMove.x*_speed;
            mMoveDir.z = _desiredMove.z*_speed;


            if (mCharacterController.isGrounded)
            {
                mMoveDir.y = -mStickToGroundForce;

                if (mJump)
                {
                    mMoveDir.y = mJumpSpeed;
                    PlayJumpSound();
                    mJump = false;
                    mJumping = true;
                }
            }
            else
            {
                mMoveDir += Physics.gravity*mGravityMultiplier*Time.fixedDeltaTime;
            }
            mCollisionFlags = mCharacterController.Move(mMoveDir*Time.fixedDeltaTime);

            ProgressStepCycle(_speed);
            UpdateCameraPosition(_speed);

            mMouseLook.UpdateCursorLock();
        }

        // On Trigger Enter
        public void OnTriggerEnter(Collider other)
        {
            Debug.Log("FADE CARALHO!");
            FadeImage.CrossFadeAlpha(1.0f, 3, true);
        }

        #endregion

        private void PlayLandingSound()
        {
            mAudioSource.clip = mLandSound;
            mAudioSource.Play();
            mNextStep = mStepCycle + .5f;
        }

        private void PlayJumpSound()
        {
            mAudioSource.clip = mJumpSound;
            mAudioSource.Play();
        }

        private void ProgressStepCycle(float speed)
        {
            if (mCharacterController.velocity.sqrMagnitude > 0 && (mInput.x != 0 || mInput.y != 0))
            {
                mStepCycle += (mCharacterController.velocity.magnitude + (speed*(mIsWalking ? 1f : mRunstepLenghten)))*
                              Time.fixedDeltaTime;
            }

            if (!(mStepCycle > mNextStep))
            {
                return;
            }

            mNextStep = mStepCycle + mStepInterval;

            PlayFootStepAudio();
        }

        private void PlayFootStepAudio()
        {
            if (!mCharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int _n = Random.Range(1, mFootstepSounds.Length);
            mAudioSource.clip = mFootstepSounds[_n];
            mAudioSource.PlayOneShot(mAudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            mFootstepSounds[_n] = mFootstepSounds[0];
            mFootstepSounds[0] = mAudioSource.clip;
        }

        private void UpdateCameraPosition(float speed)
        {
            Vector3 _newCameraPosition;
            if (!mUseHeadBob)
            {
                return;
            }
            if (mCharacterController.velocity.magnitude > 0 && mCharacterController.isGrounded)
            {
                mCamera.transform.localPosition =
                    mHeadBob.DoHeadBob(mCharacterController.velocity.magnitude +
                                       (speed*(mIsWalking ? 1f : mRunstepLenghten)));
                _newCameraPosition = mCamera.transform.localPosition;
                _newCameraPosition.y = mCamera.transform.localPosition.y - mJumpBob.Offset();
            }
            else
            {
                _newCameraPosition = mCamera.transform.localPosition;
                _newCameraPosition.y = mOriginalCameraPosition.y - mJumpBob.Offset();
            }
            mCamera.transform.localPosition = _newCameraPosition;
        }

        private void GetInput(out float speed)
        {
            // Read input
            float _horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float _vertical = CrossPlatformInputManager.GetAxis("Vertical");

            bool _waswalking = mIsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            mIsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
            // set the desired speed to be walking or running
            speed = mIsWalking ? mWalkSpeed : mRunSpeed;
            mInput = new Vector2(_horizontal, _vertical);

            // normalize input if it exceeds 1 in combined length:
            if (mInput.sqrMagnitude > 1)
            {
                mInput.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (mIsWalking != _waswalking && mUseFovKick && mCharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!mIsWalking ? mFovKick.FOVKickUp() : mFovKick.FOVKickDown());
            }
        }

        private void RotateView()
        {
            mMouseLook.LookRotation(transform, mCamera.transform);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody _body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (mCollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (_body == null || _body.isKinematic)
            {
                return;
            }
            _body.AddForceAtPosition(mCharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }
    }
}
