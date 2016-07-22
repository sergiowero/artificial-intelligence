using System;
using System.Collections.Generic;

namespace Uag.AI.Common.Events
{
    public class EventDispatcher
    {
        public delegate void EventHandler(IEvent _event);
        private Dictionary<int, EventHandler> m_eventHandlers;
        private EventHandler m_generalHandler;

        protected void Initialize(Type _eventsType)
        {
            m_eventHandlers = new Dictionary<int, EventHandler>();

            Array values = Enum.GetValues(_eventsType);

            foreach (int enumValue in values)
            {
                m_eventHandlers[enumValue] = (_e) => { };
            }

            m_generalHandler = (_e) => { };
        }

        public void AddGeneralHandler(EventHandler _handler)
        {
            // Avoid Duplicate handlers
            m_generalHandler -= _handler;
            m_generalHandler += _handler;
        }

        public void RemoveGeneralHandler(EventHandler _handler)
        {
            m_generalHandler -= _handler;
        }

        public void AddEventHandler(Enum _eventType, EventHandler _handler)
        {
            int value = (int)((object)_eventType);

            // Avoid Duplicate handlers
            m_eventHandlers[value] -= _handler;
            m_eventHandlers[value] += _handler;
        }

        public void RemoveEventHandler(Enum _eventType, EventHandler _handler)
        {
            int value = (int)((object)_eventType);

            // Avoid Duplicate handlers
            m_eventHandlers[value] -= _handler;
        }

        public void DispatchEvent(IEvent _event)
        {
            m_generalHandler(_event);
            m_eventHandlers[_event.eventType](_event);
        }
    }
}
