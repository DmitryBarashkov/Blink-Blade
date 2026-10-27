using BlinkBlade.Camera;
using BlinkBlade.Game;
using UnityEngine;
using Zenject;

namespace BlinkBlade.Players
{
    public class Aimer
    {
        private CameraBoundsInstaller _cameraBoundsInstaller;
        private CameraOffsetChanger _cameraOffsetChanger;
        private Transform _playerTransform;
        private AimingArrow _aimingArrow;
        private PlayerAnimator _animator;
        private Weapon _weapon;
        private LevelState _levelState;

        private float _maxTurnAngle = 100f;

        private Vector3 _targetDir;
        private float _currentAngle;

        [Inject]
        public void Construct(CameraBoundsInstaller cameraBoundsInstaller, CameraOffsetChanger cameraOffsetChanger, AimingArrow aimingArrow, LevelState levelState)
        {
            _cameraBoundsInstaller = cameraBoundsInstaller;
            _cameraOffsetChanger = cameraOffsetChanger;
            _aimingArrow = aimingArrow;
            _levelState = levelState;
        }

        public void Initialize(Transform playerTransform, PlayerAnimator animator)
        {
            _animator = animator;
            _playerTransform = playerTransform;

            _cameraOffsetChanger.ClearOffset(true);
            _cameraBoundsInstaller.SetAim(_playerTransform, true);
        }

        public void ChangeWeapon(Weapon weapon)
        {
            _weapon = weapon;
        }

        public void StartAim()
        {
            if (_levelState.IsFirstAimingStarted.Value == false)
                _levelState.IsFirstAimingStarted.Value = true;

            _cameraBoundsInstaller.SetAim(_playerTransform);

            _animator.SetAiming(true);

            _aimingArrow.SetPosition(_playerTransform.position);
            _aimingArrow.Show();
        }

        public void StopAim(bool isNeedWeaponThrow = true)
        {
            if (isNeedWeaponThrow)
            {
                _weapon.Throw(_aimingArrow.Direction, _playerTransform.rotation.y);
                _cameraBoundsInstaller.SetAim(_weapon.transform);
                _levelState.IsFirstThrow.Value = true;
            }
            else
            {
                _cameraBoundsInstaller.SetAim(_playerTransform, true);
            }

            _cameraOffsetChanger.ClearOffset(isNeedWeaponThrow == false);
            _animator.SetAiming(false);
            _aimingArrow.Hide();
        }

        public void PerformAim()
        {
            _targetDir = Vector3.ProjectOnPlane(_aimingArrow.Direction, Vector3.up);
            _currentAngle = Vector3.Angle(_playerTransform.forward, _targetDir);
            _cameraOffsetChanger.SetOffset(_aimingArrow.Direction.normalized);

            if (_currentAngle > _maxTurnAngle)
            {
                _playerTransform.rotation = Quaternion.LookRotation(_targetDir);
            }
        }
    }
}