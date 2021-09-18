using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// Скрипт управления AI. Цепляется на префаб корабля.
    /// Реализует управление используя набор примитивных действий.
    /// </summary>
    [RequireComponent(typeof(SpaceShip))]
    public class AIController : MonoBehaviour
    {
        /// <summary>
        /// Типы поведений.
        /// </summary>
        public enum AIBehaviour
        {
            /// <summary>
            /// Ничего не делаем.
            /// </summary>
            Null,

            /// <summary>
            /// Патрулируем и атакуем врагов.
            /// </summary>
            Patrol
        }

        [SerializeField] private AIBehaviour m_AIBehaviour;

        /// <summary>
        /// Как быстро будем летать.
        /// </summary>
        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationLinear;

        /// <summary>
        /// Как быстро будет бот поворачиваться.
        /// </summary>
        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationAngular;

        /// <summary>
        /// Текущая точка патрулирования. Впринципе может быть размером со всю игровую область.
        /// </summary>
        [SerializeField] private AIPointPatrol m_PatrolPoint;

        /// <summary>
        /// Время рандомизации выбора новой точки движения.
        /// Задает значение таймера ActionTimerType.RandomizeDirection
        /// </summary>
        [SerializeField] private float m_RandomSelectMovePointTime;

        /// <summary>
        /// Время между поисками целей. Минимальное внутри реализации 1сек.
        /// </summary>
        [SerializeField] private float m_FindNewTargetTime;

        /// <summary>
        /// Рандомное время между выстрелами.
        /// </summary>
        [SerializeField] private float m_ShootDelay;

        /// <summary>
        /// Дальность обзора для рейкаста вперед.
        /// </summary>
        [SerializeField] private float m_EvadeRayLength;

        /// <summary>
        /// Кеш ссылка на корабль.
        /// </summary>
        private SpaceShip m_SpaceShip;

        /// <summary>
        /// Текущая точка куда бот должен лететь. Может являтся как статичной, так и какой то динамической.
        /// </summary>
        private Vector3 m_MovePosition;

        /// <summary>
        /// Выбранная ботом цель.
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
        /// Метод обновления логики AI.
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
        /// Метод поведения патрулирования.
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
        /// Действие управления кораблем.
        /// </summary>
        private void ActionControlShip()
        {
            m_SpaceShip.ThrustControl = m_NavigationLinear;
            m_SpaceShip.TorqueControl = ComputeAlignTorqueNormalized(m_MovePosition, transform) * m_NavigationAngular;
        }

        private const float MaxAngle = 45.0f;

        /// <summary>
        /// Метод вычисления управляющего нормализованного значения вращательной тяги корабля,
        /// так чтобы навестись на цель.
        /// </summary>
        /// <param name="targetPosition"></param>
        /// <param name="ship"></param>
        /// <returns></returns>
        private static float ComputeAlignTorqueNormalized(Vector3 targetPosition, Transform ship)
        {
            // переводим целевую позицию в систему координат корабля
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);

            // вычисляем знаковый угол между направлением вперед корабля и вектором до цели
            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);

            // тут можно ограничить угол до 45 градусов чтобы нормализованное значение
            // было более шустрым при повороте, если не клемпить то максимальные скорости поворота будут
            // почти всегда недостижим. Или проще если угол до цели больше чем 45 то почему бы сразу не крутануть баранку по максимуму до нее.
            angle = Mathf.Clamp(angle, -MaxAngle, MaxAngle) / MaxAngle;

            // Возвращаем значение.
            return -angle;
        }

        // гизмы для дебага точки куда лететь.
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(m_MovePosition, 1.0f);
        }

        /// <summary>
        /// Метод поиска новой точки движения.
        /// </summary>
        private void ActionFindNewMovePosition()
        {
            if (m_AIBehaviour == AIBehaviour.Patrol)
            {
                // данное условие появится в юните стрельбы, корабль будет лететь до цели.
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
                        // если катаемся внутри зоны патрулирования то выбираем случайную точки внутри.
                        GetNewPoint();
                    }
                    else
                    {
                        // если мы не в зоне патруля то едем до нее.
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
        /// Типы таймеров.
        /// </summary>
        private enum ActionTimerType
        {
            Null,

            /// <summary>
            /// Рандомизация движения.
            /// </summary>
            RandomizeDirection,

            /// <summary>
            /// Стрельба.
            /// </summary>
            Fire,

            /// <summary>
            /// Поиск новый цели.
            /// </summary>
            FindNewTarget,

            /// <summary>
            /// Максимальное кол-во типов таймеров. Немного С стайл можно через 
            /// </summary>
            MaxValues
        }

        private float[] m_ActionTimers;

        /// <summary>
        /// Инициализируем таймеры. Впринципе можно унести в отдельный класс.
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
            return m_ActionTimers[(int)e] <= 0; // ВАЖНО: с нулем сравнивать так потому что юнити может влепить таймер в 0
        }

        #endregion

        /// <summary>
        /// Действие определения новой цели
        /// </summary>
        private void ActionFindNewAttackTarget()
        {
            if (IsActionTimerFinished(ActionTimerType.FindNewTarget))
            {
                m_SelectedTarget = FindNearestDestructibleTarget();

                SetActionTimer(ActionTimerType.FindNewTarget, 1 + UnityEngine.Random.Range(0, m_FindNewTargetTime)); // минимальное значение 1 чтобы не дергать каждый кадр.
            }
        }

        /// <summary>
        /// Стреляем если надо.
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
        /// Метод вычисления точки упреждения.
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
        /// Метод поиска ближайшей цели.
        /// Реализуем в юните стрельбы.
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

                // исключаем полностью нейтральных (например астероиды)
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
        /// Метод установки поведения патрулирования. Например после того как спавнер заинстансит бота.
        /// </summary>
        /// <param name="point"></param>
        public void SetPatrolBehaviour(AIPointPatrol point)
        {
            m_AIBehaviour = AIBehaviour.Patrol;
            m_PatrolPoint = point;
        }

        #region AI collision evade

        /// <summary>
        /// Метод для изменения m_MovePosition так чтобы не вляпаться в коллайдер.
        /// </summary>
        private void ActionEvadeCollision()
        {
            if (Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLength))
            {
                // можно рандомно выбрать полететь влево или вправо, только надо будет выставть таймер
                // иначе каждый кадр будет спамится то влево то вправо и в итоге ничего не выйдет.

                // выставляем точку вдалеке чтобы бот на нее начал немеделнно поворачиваться.
                m_MovePosition = transform.position + transform.right * 100.0f;

                // проверить можно в плеймоде на сцене двигая перед кораблями AI коллайдеры, впринципе любые
            }
        }

        #endregion
    }
}