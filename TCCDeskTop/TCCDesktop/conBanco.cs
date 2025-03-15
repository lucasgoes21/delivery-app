using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace TCCDesktop
{
    class conBanco
    {
        public IFirebaseConfig config = new FirebaseConfig { AuthSecret = "Dy6SnW1fMI5ubSF5wD58YOfSJJjD6BueqeuTpWbX", BasePath = "https://beer-grill-default-rtdb.firebaseio.com/" };

        public IFirebaseClient liga;
        public FirebaseResponse resonde;


    }
}
