using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using ProtoBuf;
using VRageMath;
using VRage.Serialization;

namespace VRage
{
    [ProtoContract]
    public struct SerializableVector3
    {

        /// <summary>
        /// Used to determine if a serialized field was actually deserialized or left at default values.
        /// </summary>
        /// <seealso cref="IsUninitialized"/>
        /// <seealso cref="GetOrDefault"/>
        public readonly static SerializableVector3 NotInitialized = new SerializableVector3(float.NaN, float.NaN, float.NaN);

        public float X;
        public float Y;
        public float Z;

        public bool ShouldSerializeX() { return false; }
        public bool ShouldSerializeY() { return false; }
        public bool ShouldSerializeZ() { return false; }

        public SerializableVector3(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        [ProtoMember, XmlAttribute]
        [NoSerialize]
        public float x { get { return X; } set { X = value; } }

        [ProtoMember, XmlAttribute]
        [NoSerialize]
        public float y { get { return Y; } set { Y = value; } }

        [ProtoMember, XmlAttribute]
        [NoSerialize]
        public float z { get { return Z; } set { Z = value; } }

        public bool IsZero { get { return X == 0.0f && Y == 0.0f && Z == 0.0f; } }

        /// <summary>
        /// Used to determine if a serialized field was actually deserialized or left at default values.
        /// 
        /// To use, set the desired field to SerializableVector3.NotInitialized in the cube block builder.
        /// </summary>
        public bool IsUninitialized 
        { 
            get 
            {
                return float.IsNaN(X) && float.IsNaN(Y) && float.IsNaN(Z); 
            } 
        }

        /// <summary>
        /// Get our value if initialized, otherwise return defaultValue.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public Vector3 GetOrDefault(Vector3 defaultValue)
        {
            return !IsUninitialized ? (Vector3)this : defaultValue;
        }

        public static implicit operator Vector3(SerializableVector3 v)
        {
            if (v.IsUninitialized)
                return new Vector3(0f,0f,0f);
            else
                return new Vector3((float)v.X, (float)v.Y, (float)v.Z);
        }

        public static implicit operator SerializableVector3(Vector3 v)
        {
            return new SerializableVector3(v.X, v.Y, v.Z);
        }

        public static bool operator ==(SerializableVector3 a, SerializableVector3 b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
    }

        public static bool operator !=(SerializableVector3 a, SerializableVector3 b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        public override bool Equals(object obj)
        {
            if (obj is SerializableVector3)
            {
                return (SerializableVector3)obj == this;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() * 1610612741 ^ Y.GetHashCode() * 24593 ^ Z.GetHashCode();
        }
    }
}
