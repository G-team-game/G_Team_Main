using UnityEngine;

public class SightChecker : MonoBehaviour
{
    [SerializeField] private Transform _self;
    [SerializeField] private Transform _target;
    [SerializeField] private float _sightAngle;
    [SerializeField] private float _maxDistance = float.PositiveInfinity;
    [SerializeField] private float _rotationSpeed = 5.0f;

    private bool isPlayerInSight = false;
    private float timeSincePlayerLost = 0.0f;
    [SerializeField] private float timeToKeepFacingPlayer = 1.0f; // �v���C���[�̕������������鎞��
    [SerializeField] private float _minDistanceToReact = 2.0f; // ���ߋ����Ŕ�������ŏ�����
    [SerializeField] private LayerMask _obstacleLayer;

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(_self.position, _target.position);

        if (distanceToPlayer <= _minDistanceToReact)
        {
            // �v���C���[�����ߋ����ɂ���ꍇ�A�^�[�Q�b�g�̕�������
            Vector3 targetDirection = (_target.position - _self.position).normalized;
            // ���C�L���X�g���g�p���ď�Q�����Ȃ����m�F
            if (!IsObstacleBetween(_self.position, _target.position))
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                _self.rotation = Quaternion.Slerp(_self.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
            }
        }
        else
        {
            // �v���C���[�����ߋ����ɂ��Ȃ��ꍇ�A���E������s��
            bool isVisible = IsVisible();
            timeSincePlayerLost += Time.deltaTime;
            if (isVisible)
            {
                // �^�[�Q�b�g�������Ă���ꍇ�A���E���̒ʏ�̏������s��
                // ���C�L���X�g���g�p���ď�Q�����Ȃ����m�F
                if (!IsObstacleBetween(_self.position, _target.position))
                {
                    timeSincePlayerLost = 0.0f;
                    isPlayerInSight = true;
                    Vector3 targetDirection = (_target.position - _self.position).normalized;
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    _self.rotation = Quaternion.Slerp(_self.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
                }
            }
            else if (isPlayerInSight&& !IsObstacleBetween(_self.position, _target.position))
            {
                // �^�[�Q�b�g�������Ă��Ȃ����A�ŏ��̉��b�Ԃ��̓v���C���[�̕�������������
                if (timeSincePlayerLost <= timeToKeepFacingPlayer)
                {
                    Vector3 playerDirection = (_target.position - _self.position).normalized;
                    Quaternion playerRotation = Quaternion.LookRotation(playerDirection);
                    _self.rotation = Quaternion.Slerp(_self.rotation, playerRotation, Time.deltaTime * _rotationSpeed);
                }
                else
                {
                    isPlayerInSight = false;
                }
            }
            else
            {
                // �^�[�Q�b�g�������Ă��Ȃ��ꍇ�A�������Ȃ����A�ʂ̍s����ǉ�����
            }
        }
    }
    #region Logic
    public bool IsVisible()
    {
        // ���g�̈ʒu
        var selfPos = _self.position;
        // �^�[�Q�b�g�̈ʒu
        var targetPos = _target.position;

        // ���g�̌����i���K�����ꂽ�x�N�g���j
        var selfDir = _self.forward;

        // �^�[�Q�b�g�܂ł̌����Ƌ����v�Z
        var targetDir = targetPos - selfPos;
        var targetDistance = targetDir.magnitude;

        // cos(��/2)���v�Z
        var cosHalf = Mathf.Cos(_sightAngle / 2 * Mathf.Deg2Rad);

        // ���g�ƃ^�[�Q�b�g�ւ̌����̓��όv�Z
        // �^�[�Q�b�g�ւ̌����x�N�g���𐳋K������K�v�����邱�Ƃɒ���
        var innerProduct = Vector3.Dot(selfDir, targetDir.normalized);

        // ���E����
        return innerProduct > cosHalf && targetDistance < _maxDistance;
    }

    private bool IsObstacleBetween(Vector3 start, Vector3 end)
    {
        // ���C�L���X�g���g�p���đΏۂƂ̊Ԃɏ�Q�������邩�m�F
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        if (Physics.Raycast(start, direction, out RaycastHit hit, distance, _obstacleLayer))
        {
            // ��Q��������ꍇ�� true ��Ԃ�
            return true;
        }

        // ��Q�����Ȃ��ꍇ�� false ��Ԃ�
        return false;
    }
    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // �J���[��ݒ�i�ԐF�Ȃǁj
        Gizmos.DrawWireSphere(transform.position, _maxDistance); // �ő勗���̉~��`��
        Gizmos.DrawLine(transform.position, _target.position); // ���g����^�[�Q�b�g�ւ̃��C����`��
        Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.LookRotation(_self.forward), Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, _sightAngle, 0f, _maxDistance, 1f);
    }
    #region Debug

    // ���E����̌��ʂ�GUI�o��
    private void OnGUI()
    {
        // ���E����
        var isVisible = IsVisible();

        // ���ʕ\��
        GUI.Box(new Rect(20, 20, 150, 23), $"isVisible = {isVisible}");
    }

    #endregion
}