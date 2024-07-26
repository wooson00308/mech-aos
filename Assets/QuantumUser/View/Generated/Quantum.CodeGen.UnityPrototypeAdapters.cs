// <auto-generated>
// This code was auto-generated by a tool, every time
// the tool executes this code will be reset.
//
// If you need to extend the classes generated to add
// fields or methods to them, please create partial
// declarations in another file.
// </auto-generated>
#pragma warning disable 0109
#pragma warning disable 1591


namespace Quantum.Prototypes.Unity {
  using Photon.Deterministic;
  using Quantum;
  using Quantum.Core;
  using Quantum.Collections;
  using Quantum.Inspector;
  using Quantum.Physics2D;
  using Quantum.Physics3D;
  using Byte = System.Byte;
  using SByte = System.SByte;
  using Int16 = System.Int16;
  using UInt16 = System.UInt16;
  using Int32 = System.Int32;
  using UInt32 = System.UInt32;
  using Int64 = System.Int64;
  using UInt64 = System.UInt64;
  using Boolean = System.Boolean;
  using String = System.String;
  using Object = System.Object;
  using FlagsAttribute = System.FlagsAttribute;
  using SerializableAttribute = System.SerializableAttribute;
  using MethodImplAttribute = System.Runtime.CompilerServices.MethodImplAttribute;
  using MethodImplOptions = System.Runtime.CompilerServices.MethodImplOptions;
  using FieldOffsetAttribute = System.Runtime.InteropServices.FieldOffsetAttribute;
  using StructLayoutAttribute = System.Runtime.InteropServices.StructLayoutAttribute;
  using LayoutKind = System.Runtime.InteropServices.LayoutKind;
  #if QUANTUM_UNITY //;
  using TooltipAttribute = UnityEngine.TooltipAttribute;
  using HeaderAttribute = UnityEngine.HeaderAttribute;
  using SpaceAttribute = UnityEngine.SpaceAttribute;
  using RangeAttribute = UnityEngine.RangeAttribute;
  using HideInInspectorAttribute = UnityEngine.HideInInspector;
  using PreserveAttribute = UnityEngine.Scripting.PreserveAttribute;
  using FormerlySerializedAsAttribute = UnityEngine.Serialization.FormerlySerializedAsAttribute;
  using MovedFromAttribute = UnityEngine.Scripting.APIUpdating.MovedFromAttribute;
  using CreateAssetMenu = UnityEngine.CreateAssetMenuAttribute;
  using RuntimeInitializeOnLoadMethodAttribute = UnityEngine.RuntimeInitializeOnLoadMethodAttribute;
  #endif //;
  
  [System.SerializableAttribute()]
  public unsafe partial class AsteroidsProjectilePrototype : Quantum.QuantumUnityPrototypeAdapter<Quantum.Prototypes.AsteroidsProjectilePrototype> {
    public FP TTL;
    public Quantum.QuantumEntityPrototype Owner;
    partial void ConvertUser(Quantum.QuantumEntityPrototypeConverter converter, ref Quantum.Prototypes.AsteroidsProjectilePrototype prototype);
    public override Quantum.Prototypes.AsteroidsProjectilePrototype Convert(Quantum.QuantumEntityPrototypeConverter converter) {
      var result = new Quantum.Prototypes.AsteroidsProjectilePrototype();
      converter.Convert(this.TTL, out result.TTL);
      converter.Convert(this.Owner, out result.Owner);
      ConvertUser(converter, ref result);
      return result;
    }
  }
  [System.SerializableAttribute()]
  public unsafe partial class BulletFieldsPrototype : Quantum.QuantumUnityPrototypeAdapter<Quantum.Prototypes.BulletFieldsPrototype> {
    public FP Time;
    public Quantum.QuantumEntityPrototype Source;
    public FPVector3 Direction;
    public AssetRef<BulletData> BulletData;
    partial void ConvertUser(Quantum.QuantumEntityPrototypeConverter converter, ref Quantum.Prototypes.BulletFieldsPrototype prototype);
    public override Quantum.Prototypes.BulletFieldsPrototype Convert(Quantum.QuantumEntityPrototypeConverter converter) {
      var result = new Quantum.Prototypes.BulletFieldsPrototype();
      converter.Convert(this.Time, out result.Time);
      converter.Convert(this.Source, out result.Source);
      converter.Convert(this.Direction, out result.Direction);
      converter.Convert(this.BulletData, out result.BulletData);
      ConvertUser(converter, ref result);
      return result;
    }
  }
  [System.SerializableAttribute()]
  public unsafe partial class KCCPrototype : Quantum.QuantumUnityPrototypeAdapter<Quantum.Prototypes.KCCPrototype> {
    public AssetRef<KCCSettings> Settings;
    partial void ConvertUser(Quantum.QuantumEntityPrototypeConverter converter, ref Quantum.Prototypes.KCCPrototype prototype);
    public override Quantum.Prototypes.KCCPrototype Convert(Quantum.QuantumEntityPrototypeConverter converter) {
      var result = new Quantum.Prototypes.KCCPrototype();
      converter.Convert(this.Settings, out result.Settings);
      ConvertUser(converter, ref result);
      return result;
    }
  }
  [System.SerializableAttribute()]
  public unsafe partial class KCCCollisionPrototype : Quantum.QuantumUnityPrototypeAdapter<Quantum.Prototypes.KCCCollisionPrototype> {
    public Quantum.QEnum8<EKCCCollisionSource> Source;
    public Quantum.QuantumEntityPrototype Reference;
    public AssetRef Processor;
    partial void ConvertUser(Quantum.QuantumEntityPrototypeConverter converter, ref Quantum.Prototypes.KCCCollisionPrototype prototype);
    public override Quantum.Prototypes.KCCCollisionPrototype Convert(Quantum.QuantumEntityPrototypeConverter converter) {
      var result = new Quantum.Prototypes.KCCCollisionPrototype();
      converter.Convert(this.Source, out result.Source);
      converter.Convert(this.Reference, out result.Reference);
      converter.Convert(this.Processor, out result.Processor);
      ConvertUser(converter, ref result);
      return result;
    }
  }
  [System.SerializableAttribute()]
  public unsafe partial class KCCIgnorePrototype : Quantum.QuantumUnityPrototypeAdapter<Quantum.Prototypes.KCCIgnorePrototype> {
    public Quantum.QEnum8<EKCCIgnoreSource> Source;
    public Quantum.QuantumEntityPrototype Reference;
    partial void ConvertUser(Quantum.QuantumEntityPrototypeConverter converter, ref Quantum.Prototypes.KCCIgnorePrototype prototype);
    public override Quantum.Prototypes.KCCIgnorePrototype Convert(Quantum.QuantumEntityPrototypeConverter converter) {
      var result = new Quantum.Prototypes.KCCIgnorePrototype();
      converter.Convert(this.Source, out result.Source);
      converter.Convert(this.Reference, out result.Reference);
      ConvertUser(converter, ref result);
      return result;
    }
  }
  [System.SerializableAttribute()]
  public unsafe partial class KCCModifierPrototype : Quantum.QuantumUnityPrototypeAdapter<Quantum.Prototypes.KCCModifierPrototype> {
    public AssetRef Processor;
    public Quantum.QuantumEntityPrototype Entity;
    partial void ConvertUser(Quantum.QuantumEntityPrototypeConverter converter, ref Quantum.Prototypes.KCCModifierPrototype prototype);
    public override Quantum.Prototypes.KCCModifierPrototype Convert(Quantum.QuantumEntityPrototypeConverter converter) {
      var result = new Quantum.Prototypes.KCCModifierPrototype();
      converter.Convert(this.Processor, out result.Processor);
      converter.Convert(this.Entity, out result.Entity);
      ConvertUser(converter, ref result);
      return result;
    }
  }
  [System.SerializableAttribute()]
  public unsafe partial class MechProjectilePrototype : Quantum.QuantumUnityPrototypeAdapter<Quantum.Prototypes.MechProjectilePrototype> {
    public FP TTL;
    public Quantum.QuantumEntityPrototype Owner;
    partial void ConvertUser(Quantum.QuantumEntityPrototypeConverter converter, ref Quantum.Prototypes.MechProjectilePrototype prototype);
    public override Quantum.Prototypes.MechProjectilePrototype Convert(Quantum.QuantumEntityPrototypeConverter converter) {
      var result = new Quantum.Prototypes.MechProjectilePrototype();
      converter.Convert(this.TTL, out result.TTL);
      converter.Convert(this.Owner, out result.Owner);
      ConvertUser(converter, ref result);
      return result;
    }
  }
}
#pragma warning restore 0109
#pragma warning restore 1591
