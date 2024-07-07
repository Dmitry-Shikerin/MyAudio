using System;

namespace MyAudios.MyUiFramework.Utils
{
    public static class GuidUtils
    {
        /// <summary>
        /// Creates the SerializedGuid byte array
        /// <para/> Used OnBeforeSerialize
        /// </summary>
        public static byte[] GuidToSerializedGuid(Guid guid) =>
            guid != Guid.Empty ? guid.ToByteArray() : null;

        /// <summary>
        /// Restores the Guid from the SerializedGuid byte array
        /// <para/> Used OnAfterDeserialize 
        /// </summary>
        public static Guid SerializedGuidToGuid(byte[] serializedGuid) =>
            serializedGuid != null && serializedGuid.Length == 16 ? new Guid(serializedGuid) : Guid.Empty;
    }
}