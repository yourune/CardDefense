using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardDefense.Core.Events
{
    /// <summary>
    /// 게임 내 모든 중요한 사건(데미지, 사망, 카드 사용 등)을 관리하는 전역 이벤트 버스입니다.
    /// 코루틴 대기열 방식을 폐기하고, 즉각적인 이벤트 브로드캐스팅을 지원합니다.
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, Delegate> _events = new Dictionary<Type, Delegate>();

        /// <summary>
        /// 특정 타입의 이벤트를 구독합니다.
        /// </summary>
        public static void Subscribe<T>(Action<T> onEvent) where T : IGameEvent
        {
            if (!_events.TryGetValue(typeof(T), out Delegate currentDelegate))
            {
                _events[typeof(T)] = onEvent;
            }
            else
            {
                _events[typeof(T)] = Delegate.Combine(currentDelegate, onEvent);
            }
        }

        /// <summary>
        /// 특정 타입의 이벤트를 구독 해지합니다.
        /// </summary>
        public static void Unsubscribe<T>(Action<T> onEvent) where T : IGameEvent
        {
            if (_events.TryGetValue(typeof(T), out Delegate currentDelegate))
            {
                Delegate newDelegate = Delegate.Remove(currentDelegate, onEvent);
                if (newDelegate == null)
                {
                    _events.Remove(typeof(T));
                }
                else
                {
                    _events[typeof(T)] = newDelegate;
                }
            }
        }

        /// <summary>
        /// 즉시 이벤트를 발생시키고 구독자들에게 데이터를 전달합니다.
        /// (예: 데미지 100이 들어왔다는 단순 데이터 전달 -> 구독중인 VFX 시스템, UI 시스템이 각자 자기 할 일을 함)
        /// </summary>
        public static void Publish<T>(T gameEvent) where T : IGameEvent
        {
            if (_events.TryGetValue(typeof(T), out Delegate currentDelegate))
            {
                // 모든 구독자에게 동기적으로 이벤트를 실행
                (currentDelegate as Action<T>)?.Invoke(gameEvent);
            }
        }

        /// <summary>
        /// 씬 전환 시 강제로 모든 구독을 해제합니다. 메모리 릭 방지용.
        /// </summary>
        public static void ClearAll()
        {
            _events.Clear();
        }
    }

    /// <summary>
    /// 모든 이벤트 데이터의 기본 인터페이스
    /// </summary>
    public interface IGameEvent { }
}
