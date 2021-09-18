using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// ������ ���������� AI. ��������� �� ������ �������.
    /// ��������� ���������� ��������� ����� ����������� ��������.
    /// </summary>
    [RequireComponent(typeof(SpaceShip))]
    public class AIController : MonoBehaviour
    {
        /// <summary>
        /// ���� ���������.
        /// </summary>
        public enum AIBehaviour
        {
            /// <summary>
            /// ������ �� ������.
            /// </summary>
            Null,

            /// <summary>
            /// ����������� � ������� ������.
            /// </summary>
            Patrol
        }

        [SerializeField] private AIBehaviour m_AIBehaviour;

        /// <summary>
        /// ��� ������ ����� ������.
        /// </summary>
        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationLinear;

        /// <summary>
        /// ��� ������ ����� ��� ��������������.
        /// </summary>
        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationAngular;

        /// <summary>
        /// ������� ����� ��������������. ��������� ����� ���� �������� �� ��� ������� �������.
        /// </summary>
        [SerializeField] private AIPointPatrol m_PatrolPoint;

        /// <summary>
        /// ����� ������������ ������ ����� ����� ��������.
        /// ������ �������� ������� ActionTimerType.RandomizeDirection
        /// </summary>
        [SerializeField] private float m_RandomSelectMovePointTime;

        /// <summary>
        /// ����� ����� �������� �����. ����������� ������ ���������� 1���.
        /// </summary>
        [SerializeField] private float m_FindNewTargetTime;

        /// <summary>
        /// ��������� ����� ����� ����������.
        /// </summary>
        [SerializeField] private float m_ShootDelay;

        /// <summary>
        /// ��������� ������ ��� �������� ������.
        /// </summary>
        [SerializeField] private float m_EvadeRayLength;

        /// <summary>
        /// ��� ������ �� �������.
        /// </summary>
        private SpaceShip m_SpaceShip;

        /// <summary>
        /// ������� ����� ���� ��� ������ ������. ����� ������� ��� ���������, ��� � ����� �� ������������.
        /// </summary>
        private Vector3 m_MovePosition;

        /// <summary>
        /// ��������� ����� ����.
        /// </summary>
        private Destructible m_SelectedTarget;

        #region Unity events

        private void Start()
        {
            m_SpaceShip = GetComponent<SpaceShip>();

            InitActionTimers();
        }

        private void Update()
        {
            UpdateActionTimers();
            UpdateAI();
        }

        #endregion


        /// <summary>
        /// ����� ���������� ������ AI.
        /// </summary>
        private void UpdateAI()
        {
            switch (m_AIBehaviour)
            {
                case AIBehaviour.Null:
                    break;

                case AIBehaviour.Patrol:
                    UpdateBehaviourPatrol();
                    break;
            }
        }

        /// <summary>
        /// ����� ��������� ��������������.
        /// </summary>
        private void UpdateBehaviourPatrol()
        {
            ActionFindNewMovePosition();
            ActionControlShip();
            ActionFindNewAttackTarget();
            ActionFire();
            ActionEvadeCollision();
        }

        /// <summary>
        /// �������� ���������� ��������.
        /// </summary>
        private void ActionControlShip()
        {
            m_SpaceShip.ThrustControl = m_NavigationLinear;
            m_SpaceShip.TorqueControl = ComputeAlignTorqueNormalized(m_MovePosition, transform) * m_NavigationAngular;
        }

        private const float MaxAngle = 45.0f;

        /// <summary>
        /// ����� ���������� ������������ ���������������� �������� ������������ ���� �������,
        /// ��� ����� ��������� �� ����.
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <param name="ship"></param>
        /// <returns></returns>
        private static float ComputeAlignTorqueNormalized(Vector3 targetPosition, Transform ship)
        {
            // ��������� ������� ������� � ������� ��������� �������
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);

            // ��������� �������� ���� ����� ������������ ������ ������� � �������� �� ����
            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);

            // ��� ����� ���������� ���� �� 45 �������� ����� ��������������� ��������
            // ���� ����� ������� ��� ��������, ���� �� �������� �� ������������ �������� �������� �����
            // ����� ������ ����������. ��� ����� ���� ���� �� ���� ������ ��� 45 �� ������ �� ����� �� ��������� ������� �� ��������� �� ���.
            angle = Mathf.Clamp(angle, -MaxAngle, MaxAngle) / MaxAngle;

            // ���������� ��������.
            return -angle;
        }

        // ����� ��� ������ ����� ���� ������.
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(m_MovePosition, 1.0f);
        }

        /// <summary>
        /// ����� ������ ����� ����� ��������.
        /// </summary>
        private void ActionFindNewMovePosition()
        {
            if (m_AIBehaviour == AIBehaviour.Patrol)
            {
                // ������ ������� �������� � ����� ��������, ������� ����� ������ �� ����.
                if (m_SelectedTarget != null)
                {
                    m_MovePosition = m_SelectedTarget.transform.position;
                }
                else
                if (m_PatrolPoint != null)
                {
                    bool isInsidePatrolZone = (m_PatrolPoint.transform.position - transform.position).sqrMagnitude < m_PatrolPoint.Radius * m_PatrolPoint.Radius;

                    if (isInsidePatrolZone)
                    {
                        // ���� �������� ������ ���� �������������� �� �������� ��������� ����� ������.
                        GetNewPoint();
                    }
                    else
                    {
                        // ���� �� �� � ���� ������� �� ���� �� ���.
                        m_MovePosition = m_PatrolPoint.transform.position;
                    }
                }

            }

        }

        protected virtual void GetNewPoint()
        {
            if (IsActionTimerFinished(ActionTimerType.RandomizeDirection))
            {
                Vector2 newPoint = UnityEngine.Random.onUnitSphere * m_PatrolPoint.Radius + m_PatrolPoint.transform.position;
                m_MovePosition = newPoint;


                SetActionTimer(ActionTimerType.RandomizeDirection, m_RandomSelectMovePointTime);
            }
        }

        #region Action timers

        /// <summary>
        /// ���� ��������.
        /// </summary>
        private enum ActionTimerType
        {
            Null,

            /// <summary>
            /// ������������ ��������.
            /// </summary>
            RandomizeDirection,

            /// <summary>
            /// ��������.
            /// </summary>
            Fire,

            /// <summary>
            /// ����� ����� ����.
            /// </summary>
            FindNewTarget,

            /// <summary>
            /// ������������ ���-�� ����� ��������. ������� � ����� ����� ����� 
            /// </summary>
            MaxValues
        }

        private float[] m_ActionTimers;

        /// <summary>
        /// �������������� �������. ��������� ����� ������ � ��������� �����.
        /// </summary>
        private void InitActionTimers()
        {
            m_ActionTimers = new float[(int)ActionTimerType.MaxValues];
        }

        private void UpdateActionTimers()
        {
            for (int i = 0; i < m_ActionTimers.Length; i++)
            {
                if (m_ActionTimers[i] > 0)
                    m_ActionTimers[i] -= Time.deltaTime;
            }
        }

        private void SetActionTimer(ActionTimerType e, float time)
        {
            m_ActionTimers[(int)e] = time;
        }

        private bool IsActionTimerFinished(ActionTimerType e)
        {
            return m_ActionTimers[(int)e] <= 0; // �����: � ����� ���������� ��� ������ ��� ����� ����� ������� ������ � 0
        }

        #endregion

        /// <summary>
        /// �������� ����������� ����� ����
        /// </summary>
        private void ActionFindNewAttackTarget()
        {
            if (IsActionTimerFinished(ActionTimerType.FindNewTarget))
            {
                m_SelectedTarget = FindNearestDestructibleTarget();

                SetActionTimer(ActionTimerType.FindNewTarget, 1 + UnityEngine.Random.Range(0, m_FindNewTargetTime)); // ����������� �������� 1 ����� �� ������� ������ ����.
            }
        }

        /// <summary>
        /// �������� ���� ����.
        /// </summary>
        private void ActionFire()
        {
            if (m_SelectedTarget != null)
            {
                if (IsActionTimerFinished(ActionTimerType.Fire))
                {
                    m_SpaceShip.Fire(TurretMode.Primary);

                    SetActionTimer(ActionTimerType.Fire, UnityEngine.Random.Range(0, m_ShootDelay));
                }
            }

        }

        /// <summary>
        /// ����� ���������� ����� ����������.
        /// </summary>
        /// <param name="launchPoint"></param>
        /// <param name="launchVelocity"></param>
        /// <param name="targetPos"></param>
        /// <param name="targetVelocity"></param>
        /// <returns></returns>
        public static Vector3 MakeLead(
        Vector3 launchPoint,
        Vector3 launchVelocity,
        Vector3 targetPos,
        Vector3 targetVelocity)
        {
            Vector3 V = targetVelocity;
            Vector3 D = targetPos - launchPoint;
            float A = V.sqrMagnitude - launchVelocity.sqrMagnitude;
            float B = 2 * Vector3.Dot(D, V);
            float C = D.sqrMagnitude;

            if (A >= 0)
                return targetPos;

            float rt = Mathf.Sqrt(B * B - 4 * A * C);
            float dt1 = (-B + rt) / (2 * A);
            float dt2 = (-B - rt) / (2 * A);
            float dt = (dt1 < 0 ? dt2 : dt1);
            return targetPos + V * dt;
        }

        /// <summary>
        /// ����� ������ ��������� ����.
        /// ��������� � ����� ��������.
        /// </summary>
        /// <returns></returns>
        private Destructible FindNearestDestructibleTarget()
        {
            float dist2 = -1;

            Destructible potentialTarget = null;

            foreach (var v in Destructible.AllDestructibles)
            {
                if (v.GetComponent<SpaceShip>() == m_SpaceShip)
                    continue;

                // ��������� ��������� ����������� (�������� ���������)
                if (Destructible.TeamIdNeutral == v.TeamId)
                    continue;

                if (m_SpaceShip.TeamId == v.TeamId)
                    continue;

                float d2 = (m_SpaceShip.transform.position - v.transform.position).sqrMagnitude;

                if (dist2 < 0 || d2 < dist2)
                {
                    potentialTarget = v;
                    dist2 = d2;
                }
            }

            return potentialTarget;
        }

        /// <summary>
        /// ����� ��������� ��������� ��������������. �������� ����� ���� ��� ������� ����������� ����.
        /// </summary>
        /// <param name="point"></param>
        public void SetPatrolBehaviour(AIPointPatrol point)
        {
            m_AIBehaviour = AIBehaviour.Patrol;
            m_PatrolPoint = point;
        }

        #region AI collision evade

        /// <summary>
        /// ����� ��� ��������� m_MovePosition ��� ����� �� ��������� � ���������.
        /// </summary>
        private void ActionEvadeCollision()
        {
            if (Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLength))
            {
                // ����� �������� ������� �������� ����� ��� ������, ������ ���� ����� �������� ������
                // ����� ������ ���� ����� �������� �� ����� �� ������ � � ����� ������ �� ������.

                // ���������� ����� ������� ����� ��� �� ��� ����� ���������� ��������������.
                m_MovePosition = transform.position + transform.right * 100.0f;

                // ��������� ����� � �������� �� ����� ������ ����� ��������� AI ����������, ��������� �����
            }
        }

        #endregion
    }
}