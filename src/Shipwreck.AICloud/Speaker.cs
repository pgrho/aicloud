using System;
using System.Runtime.Serialization;

namespace Shipwreck.AICloud
{
    [DataContract]
    public class Speaker
    {
        public Speaker() { }

        internal Speaker(
            string name, string displayName, bool isMale,
            bool supportsJoy = false, bool supportsSadness = false, bool supportsAnger = false,
            bool isWest = false)
        {
            _Name = name;
            _DisplayName = displayName;
            _IsMale = isMale;
            _SupportsJoy = supportsJoy;
            _SupportsSadness = supportsSadness;
            _SupportsAnger = supportsAnger;
            _IsWest = isWest;

            IsReadOnly = true;
        }

        [IgnoreDataMember]
        public bool IsReadOnly { get; }

        private string _Name;
        private string _DisplayName;
        private bool _IsMale;
        private bool _SupportsJoy;
        private bool _SupportsSadness;
        private bool _SupportsAnger;
        private bool _IsWest;

        [DataMember]
        public string Name
        {
            get => _Name;
            set => ThrowIfLocked()._Name = value;
        }

        [DataMember]
        public string DisplayName
        {
            get => _DisplayName;
            set => ThrowIfLocked()._DisplayName = value;
        }

        [DataMember]
        public bool IsMale
        {
            get => _IsMale;
            set => ThrowIfLocked()._IsMale = value;
        }

        [IgnoreDataMember]
        public bool IsFemale
        {
            get => !IsMale;
            set => ThrowIfLocked().IsMale = !value;
        }

        [DataMember]
        public bool SupportsAnger
        {
            get => _SupportsAnger;
            set => ThrowIfLocked()._SupportsAnger = value;
        }

        [DataMember]
        public bool SupportsSadness
        {
            get => _SupportsSadness;
            set => ThrowIfLocked()._SupportsSadness = value;
        }

        [DataMember]
        public bool SupportsJoy
        {
            get => _SupportsJoy;
            set => ThrowIfLocked()._SupportsJoy = value;
        }

        [DataMember]
        public bool IsWest
        {
            get => _IsWest;
            set => ThrowIfLocked()._IsWest = value;
        }

        private Speaker ThrowIfLocked()
        {
            if (IsReadOnly)
            {
                throw new InvalidOperationException();
            }
            return this;
        }
    }
}