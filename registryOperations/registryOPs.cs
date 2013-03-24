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

        #region eXceptionEnum
        private enum eXceptionEnum
        { 
            QUERY_OR_WRITE_SUCCESSFUL = 0,
            PERMISSION_DENIED = 2,
            NULL_REFERENCE_EXCEPTION = 3,
            IOEXCEPTION  = 4,
            ARGUMENT_EXCEPTION = 5,
            KEY_DOES_NOT_EXIST = 6,
            KEY_VALUE_NAME_DOES_NOT_EXIST = 7,
            ARGUMENT_NULL_EXCEPTION = 8,
            UNAUTHORIZED_ACCESS_EXCEPTION = 9,
            SECURITY_EXCEPTION = 10 
        };

        #endregion

        #region keyRead

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


                    readStatus = (uint)eXceptionEnum.QUERY_OR_WRITE_SUCCESSFUL;
 
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

        #endregion

        #region keyWrite

        public void keyWrite(
          string keyName,
          string keyValueName,
          string keyValueType,
          string keyValueData,
          out uint writeStatus)
        {
            writeStatus = uint.MinValue;//ZERO
            string writeThis = string.Empty;
            RegistryValueKind keyValueKind;

            switch (keyValueData)
            {

                case "REG_BINARY":
                    keyValueKind = RegistryValueKind.Binary;
                    break;

                case "REG_DWORD":
                    keyValueKind = RegistryValueKind.DWord;
                    break;

                case "REG_EXPAND_SZ":
                    keyValueKind = RegistryValueKind.ExpandString;
                    break;

                case "REG_MULTI_SZ":
                    keyValueKind = RegistryValueKind.MultiString;
                    break;

                case "REG_QWORD":
                    keyValueKind = RegistryValueKind.QWord;
                    break;

                case "REG_SZ":
                    keyValueKind = RegistryValueKind.String;
                    break;

                case "REG_DWORD_LITTLE_ENDIAN":
                    keyValueKind = RegistryValueKind.DWord;
                    break;

                case "REG_QWORD_LITTLE_ENDIAN":
                    keyValueKind = RegistryValueKind.QWord;
                    break;

                case "REG_DWORD_BIG_ENDIAN":
                    keyValueKind = RegistryValueKind.Unknown;
                    break;

                case "REG_LINK":
                    keyValueKind = RegistryValueKind.Unknown;
                    break;

                case "REG_RESOURCE_LIST":
                    keyValueKind = RegistryValueKind.Unknown;
                    break;

                case "REG_NONE":
                    keyValueKind = RegistryValueKind.Unknown;
                    break;

                default:
                    keyValueKind = RegistryValueKind.String;
                    break;
            }

            try
            {
                Registry.SetValue(
                    keyName,
                    keyValueName,
                    keyValueData,
                    keyValueKind
                    );

                writeStatus = (uint)eXceptionEnum.QUERY_OR_WRITE_SUCCESSFUL;

            }


            catch (System.ArgumentNullException) { writeStatus = (uint)eXceptionEnum.ARGUMENT_NULL_EXCEPTION; }
            catch (System.ArgumentException) { writeStatus = (uint)eXceptionEnum.ARGUMENT_EXCEPTION; }
            catch (System.UnauthorizedAccessException) { writeStatus = (uint)eXceptionEnum.UNAUTHORIZED_ACCESS_EXCEPTION; }
            catch (System.Security.SecurityException) { writeStatus = (uint)eXceptionEnum.SECURITY_EXCEPTION; }
            catch (System.NullReferenceException) {writeStatus = (uint)eXceptionEnum.NULL_REFERENCE_EXCEPTION;}

        }
        #endregion



    }
}
