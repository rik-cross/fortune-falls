using System.Collections.Generic;
using System;

namespace Engine
{
    public class Event
    {
        List<int> ids;
        Action action;
        //List<Tuple<object>> args;
        List<object> args;
    }

    /*
    public class Event
    {
        private Entity entitySource;
        private Entity entityTarget;
        private float value;
        private string message;

    }
    */

}
