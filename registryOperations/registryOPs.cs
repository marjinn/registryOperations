using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Win32;
namespace registryOperations
{
    class registryOPs
    {

        public registryOPs()
        {
        }

        private enum eXceptionEnum
        { 
            QUERY_SUCCESSFUL = 0,
            PERMISSION_DENIED = 2,
            NULL_REFERENCE_EXCEPTION = 3,
            IOEXCEPTION  = 4,
            ARGUMENT_EXCEPTION = 5,
            KEY_DOES_NOT_EXIST = 6,
            KEY_VALUE_NAME_DOES_NOT_EXIST = 7 
        };

        public string keyRead(
            string keyName,
            string keyValueName,
            out uint readStatus)
        {
            readStatus = uint.MinValue;//ZERO
            string keyValueData = string.Empty;
            try
            {
                keyValueData =
                       Registry.GetValue(
                       keyName, keyValueName, "<Not Found>").ToString();

                //value returned will be "<Not Found>" if keyValueName not found
                //value returned will be "null" if key not found

                if (keyValueData.Equals(null))    
                {
                    System.Diagnostics.Debug.WriteLine("\n"
                     + "Requested registry key does not exist."
                     + "\n"
                     + "RegistryKey : " + keyName
                     + "\n"
                     + "keyValueName : " + keyValueName
                     + "\n"

                     );

                    keyValueData = "key does not exist";
                    readStatus = (uint)eXceptionEnum.KEY_DOES_NOT_EXIST;
                }


                if (keyValueData.Equals("<Not Found>"))
                {
                    System.Diagnostics.Debug.WriteLine("\n"
                     + "Requested registry keyValueName does not exist."
                     + "\n"
                     + "RegistryKey : " + keyName
                     + "\n"
                     + "keyValueName : " + keyValueName
                     + "\n"
                     + "keyValueData : " + keyValueData
                    + "\n"

                     );

                    readStatus = (uint)eXceptionEnum.KEY_VALUE_NAME_DOES_NOT_EXIST;
                }//could also mean the keyValueName is (Default) and keyValueData is (value not set)
                    //basically could be a blank entry
                //meaning if i query (Default) valuename it will return <Not Found>

                else
                {

                    System.Diagnostics.Debug.WriteLine("\n"
                    + "Requested registry keyValueName is : "
                    + "\n"
                    + "RegistryKey : " + keyName
                    + "\n"
                    + "keyValueName : " + keyValueName
                    + "\n"
                    + "keyValueData : " + keyValueData
                    + "\n"
                    );


                    readStatus = (uint)eXceptionEnum.QUERY_SUCCESSFUL;
 
                }

            }

            catch (System.Security.SecurityException)
            {
                System.Diagnostics.Debug.WriteLine("\n"
                    + "Requested registry access is not allowed."
                    + "\n"
                    + "RegistryKey : " + keyName
                    + "\n"
                    + "keyValueName : " + keyValueName
                    + "\n"

                    );

                keyValueData = "Permission Denied";
                readStatus = (uint)eXceptionEnum.PERMISSION_DENIED;
            }


            catch (System.NullReferenceException)
            {
                System.Diagnostics.Debug.WriteLine("\n"
                    + "Requested registry key not found."
                    + "\n"
                    + "RegistryKey : " + keyName
                    + "\n"
                    + "keyValueName : " + keyValueName
                    + "\n"

                    );
                keyValueData = "NullReferenceException";
                readStatus = (uint)eXceptionEnum.NULL_REFERENCE_EXCEPTION;
            }


            catch (System.IO.IOException)
            {
                System.Diagnostics.Debug.WriteLine("\n"
                    + "Requested RegistryKey that contains the specified value has been marked for deletion."
                    + "\n"
                    + "RegistryKey : " + keyName
                    + "\n"
                    + "keyValueName : " + keyValueName
                    + "\n"

                    );
                keyValueData = "IOException";
                readStatus = (uint)eXceptionEnum.IOEXCEPTION;
            }

            catch (System.ArgumentException)
            {
                System.Diagnostics.Debug.WriteLine("\n"
                    + "Requested RegistryKey does not begin with a valid registry root."
                    + "\n"
                    + "RegistryKey : " + keyName
                    + "\n"
                    + "keyValueName : " + keyValueName
                    + "\n"

                    );
                keyValueData = "ArgumentException";
                readStatus = (uint)eXceptionEnum.ARGUMENT_EXCEPTION;
            }
                
                
                
                
                return keyValueData;
        }
    }
}
