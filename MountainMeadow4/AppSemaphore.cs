using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MountainMeadow4
{
    /// <summary>
    /// A class that counts instances of itself to perform the functionality
    /// of the App class (originally  in VB6).
    /// Application should create the class when first started and query
    /// it to find out of other instances of the application have already
    /// made other instances of it.
    /// </summary>
    internal class AppSemaphore
    {
        // Written by Francesco Balena for his blog, downloaded from
        // http://www.dotnet2themax.com/blogs/fbalena/PermaLink,guid,669b012d-46d6-402f-b8d9-b7553b44451c.aspx


        // the default instance 
        private static readonly AppSemaphore DefValue = new();
        // the system-wide semaphore
        private readonly System.Threading.Semaphore semaphore;
        // initial count for the semaphore (very high value)
        private const int MAXCOUNT = 10000;


        private AppSemaphore()
        {
            // create a named (system-wide semaphore)
#pragma warning disable IDE0018 // Inline variable declaration
            bool ownership;  //false
#pragma warning restore IDE0018 // Inline variable declaration
                               // create the semaphore or get a reference to an existing semaphore

            string name = "AppSemaphore_" +
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

            //;string name = "VB6App_" + 
            //;    System.Reflection.Assembly.GetExecutingAssembly()
            //;    .Location.Replace(":", "").Replace("\\", "");

            semaphore = new System.Threading.Semaphore(MAXCOUNT, MAXCOUNT, name, out ownership);
            // decrement its value 
            semaphore.WaitOne(); //adding timespan has no effect
            //semaphore.WaitOne();
            // if we got ownership, this app has no previous instances
            m_PrevInstance = !ownership;
        }


        // the PrevInstance property returns True if there was a previous instance running 
        // when the default instance was created


        private static bool m_PrevInstance;


        public static bool PrevInstance
        {
            get
            {
                return m_PrevInstance;
            }
        }


        // return the total number of instances of the same application that are currently running 


        public static int InstanceCount
        {
            get
            {
                // release the semaphore and grab the previous count 
                int prevCount = DefValue.semaphore.Release();
                // acquire the semaphore again
                DefValue.semaphore.WaitOne(); // adding timespan doesn't wait because of large capacity
                // eval the number of other instances that are currently running 
                return MAXCOUNT - prevCount;
            }
        }


        ~AppSemaphore()
        {
            // increment the semaphore when the application terminates
            semaphore.Release();
        }
    }
}
