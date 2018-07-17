using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OMDbAPIClient.ViewModel
{
    public class Mediator
    {
        //The mediator(singlton) will be contructor injected as a dependency and we will use it to communicate between viewmodels
        static readonly Mediator instance = new Mediator();

        public static Mediator Instance
        {
            get
            {
                return instance;
            }
        }

        private Mediator()
        {}

        private static Dictionary<string, Action<object>> subscribers = new Dictionary<string, Action<object>>();

        public void Register(string message, Action<object> action)
        {
            if(!subscribers.ContainsKey(message))
             subscribers.Add(message, action);
        }

        public void Notify(string message, Object param)
        {
            foreach (var item in subscribers)
            {

                if (item.Key.Equals(message))
                {
                    Action<object> method = item.Value;
                    method.Invoke(param);
                }
            }
        }
    }
}
