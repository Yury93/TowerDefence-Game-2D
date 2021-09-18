using UnityEngine;


/// <summary>
/// Абстрактный базовый класс для всех сущностей
/// </summary>
public abstract class Entity : MonoBehaviour
{
    /// <summary>
    /// Название объекта для пользователя
    /// </summary>
    [SerializeField]
    private string m_Nickname;
    public string Nickname => m_Nickname;

}

