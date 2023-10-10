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
    [SerializeField] private float timeToKeepFacingPlayer = 1.0f; // プレイヤーの方を向き続ける時間
    [SerializeField] private float _minDistanceToReact = 2.0f; // 至近距離で反応する最小距離
    [SerializeField] private LayerMask _obstacleLayer;

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(_self.position, _target.position);

        if (distanceToPlayer <= _minDistanceToReact)
        {
            // プレイヤーが至近距離にいる場合、ターゲットの方を向く
            Vector3 targetDirection = (_target.position - _self.position).normalized;
            // レイキャストを使用して障害物がないか確認
            if (!IsObstacleBetween(_self.position, _target.position))
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                _self.rotation = Quaternion.Slerp(_self.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
            }
        }
        else
        {
            // プレイヤーが至近距離にいない場合、視界判定を行う
            bool isVisible = IsVisible();
            timeSincePlayerLost += Time.deltaTime;
            if (isVisible)
            {
                // ターゲットが見えている場合、視界内の通常の処理を行う
                // レイキャストを使用して障害物がないか確認
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
                // ターゲットが見えていないが、最初の何秒間かはプレイヤーの方を向き続ける
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
                // ターゲットが見えていない場合、何もしないか、別の行動を追加する
            }
        }
    }
    #region Logic
    public bool IsVisible()
    {
        // 自身の位置
        var selfPos = _self.position;
        // ターゲットの位置
        var targetPos = _target.position;

        // 自身の向き（正規化されたベクトル）
        var selfDir = _self.forward;

        // ターゲットまでの向きと距離計算
        var targetDir = targetPos - selfPos;
        var targetDistance = targetDir.magnitude;

        // cos(θ/2)を計算
        var cosHalf = Mathf.Cos(_sightAngle / 2 * Mathf.Deg2Rad);

        // 自身とターゲットへの向きの内積計算
        // ターゲットへの向きベクトルを正規化する必要があることに注意
        var innerProduct = Vector3.Dot(selfDir, targetDir.normalized);

        // 視界判定
        return innerProduct > cosHalf && targetDistance < _maxDistance;
    }

    private bool IsObstacleBetween(Vector3 start, Vector3 end)
    {
        // レイキャストを使用して対象との間に障害物があるか確認
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        if (Physics.Raycast(start, direction, out RaycastHit hit, distance, _obstacleLayer))
        {
            // 障害物がある場合は true を返す
            return true;
        }

        // 障害物がない場合は false を返す
        return false;
    }
    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green; // カラーを設定（赤色など）
        Gizmos.DrawWireSphere(transform.position, _maxDistance); // 最大距離の円を描画
        Gizmos.DrawLine(transform.position, _target.position); // 自身からターゲットへのラインを描画
        Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.LookRotation(_self.forward), Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, _sightAngle, 0f, _maxDistance, 1f);
    }
    #region Debug

    // 視界判定の結果をGUI出力
    private void OnGUI()
    {
        // 視界判定
        var isVisible = IsVisible();

        // 結果表示
        GUI.Box(new Rect(20, 20, 150, 23), $"isVisible = {isVisible}");
    }

    #endregion
}